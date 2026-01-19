FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Bookong.sln ./
COPY Bookong.Web/Bookong.Web.csproj Bookong.Web/
COPY Bookong.Application/Bookong.Application.csproj Bookong.Application/
COPY Bookong.Domain/Bookong.Domain.csproj Bookong.Domain/
COPY Bookong.Infrastructure/Bookong.Infrastructure.csproj Bookong.Infrastructure/
COPY Bookong.Cli/Bookong.Cli.csproj Bookong.Cli/

RUN dotnet restore Bookong.Web/Bookong.Web.csproj \
 && dotnet restore Bookong.Cli/Bookong.Cli.csproj

COPY . .

RUN dotnet publish Bookong.Web/Bookong.Web.csproj -c Release -o /out/web \
 && dotnet publish Bookong.Cli/Bookong.Cli.csproj -c Release -o /out/cli

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install native dependencies required by System.DirectoryServices.Protocols (LDAP)
RUN apt-get update \
    && apt-get install -y --no-install-recommends libldap-2.5-0 libsasl2-2 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /out/web ./web
COPY --from=build /out/cli ./cli

ENV ASPNETCORE_URLS="http://0.0.0.0:8080"
EXPOSE 8080

# Set content root to the web app directory so static files resolve correctly
WORKDIR /app/web

# Default: run Web app. Can override by providing a different DLL as the container command.
ENTRYPOINT ["dotnet"]
CMD ["Bookong.Web.dll"]
