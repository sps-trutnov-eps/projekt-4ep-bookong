#!/usr/bin/env bash
# Fail fast on errors and undefined vars; enable pipefail when supported.
set -eu
# pipefail isn't POSIX; guard for shells that don't support it (older bash/dash)
set -o pipefail 2>/dev/null || true

ENVIRONMENT="${1:-staging}"
ROOT_DIR="$(cd "$(dirname "$0")"/.. && pwd)"
COMPOSE_FILE="$ROOT_DIR/deploy/docker-compose.${ENVIRONMENT}.yml"
ENV_FILE="$ROOT_DIR/deploy/.env.${ENVIRONMENT}"
PROJECT_NAME="bookong-${ENVIRONMENT}"

if [ ! -f "$COMPOSE_FILE" ]; then
  echo "Compose file not found: $COMPOSE_FILE" >&2
  exit 1
fi
if [ ! -f "$ENV_FILE" ]; then
  echo "Env file not found: $ENV_FILE" >&2
  exit 1
fi

# Build images (pull base images)
echo "Building images for $ENVIRONMENT (project: $PROJECT_NAME)..."
docker compose --project-name "$PROJECT_NAME" -f "$COMPOSE_FILE" --env-file "$ENV_FILE" build --pull

# Ensure DB is up before running migrations
echo "Starting database..."
docker compose --project-name "$PROJECT_NAME" -f "$COMPOSE_FILE" --env-file "$ENV_FILE" up -d db

# Run migrations and seeding with retry (DB might not be up immediately)
DEMO_FLAG="false"
RESET_FLAG="false"
SEED_ENABLED="true"
if grep -qi '^DEMO_SEED=true' "$ENV_FILE"; then DEMO_FLAG="true"; fi
if grep -qi '^DEMO_RESET=true' "$ENV_FILE"; then RESET_FLAG="true"; fi
if grep -qi '^SEED_ENABLED=false' "$ENV_FILE"; then SEED_ENABLED="false"; fi

ATTEMPTS=15
SLEEP=5

# Always run migrations first
for i in $(seq 1 $ATTEMPTS); do
  echo "Running database migrations (attempt $i/$ATTEMPTS)..."
  # Image entrypoint is 'dotnet', so we pass the CLI DLL directly
  if docker compose --project-name "$PROJECT_NAME" -f "$COMPOSE_FILE" --env-file "$ENV_FILE" run --rm app \
    /app/cli/Bookong.Cli.dll migrate; then
    echo "Database migrations completed."
    break
  fi
  echo "Migration attempt $i failed. Retrying in ${SLEEP}s..."
  sleep "$SLEEP"
done

# Run seeding if enabled
if [ "$SEED_ENABLED" = "true" ]; then
  for i in $(seq 1 $ATTEMPTS); do
    echo "Running database seeding (attempt $i/$ATTEMPTS)..."
    if docker compose --project-name "$PROJECT_NAME" -f "$COMPOSE_FILE" --env-file "$ENV_FILE" run --rm app \
      /app/cli/Bookong.Cli.dll seed --demo="$DEMO_FLAG" $( [ "$RESET_FLAG" = "true" ] && echo --reset ); then
      echo "Database seeding completed."
      break
    fi
    echo "Seeding attempt $i failed. Retrying in ${SLEEP}s..."
    sleep "$SLEEP"
  done
else
  echo "Seeding disabled by SEED_ENABLED=false; skipping."
fi

# Bring services up
echo "Starting services for $ENVIRONMENT..."
docker compose --project-name "$PROJECT_NAME" -f "$COMPOSE_FILE" --env-file "$ENV_FILE" up -d

echo "Deployment complete for $ENVIRONMENT."
