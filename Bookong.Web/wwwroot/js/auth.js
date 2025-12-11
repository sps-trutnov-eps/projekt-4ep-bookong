(function () {
    const base = window.location.origin;

    async function postJson(path, payload) {
        const url = path.startsWith('http') ? path : `${base}/${path.replace(/^\//, '')}`;
        try {
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify(payload || {})
            });

            const data = await response.json().catch(() => ({}));

            if (!response.ok) {
                return {
                    success: false,
                    isFirstTimeUser: false,
                    message: data?.message || data?.Message || 'Request failed'
                };
            }

            return data;
        } catch (err) {
            return {
                success: false,
                isFirstTimeUser: false,
                message: err?.message || 'Network error'
            };
        }
    }

    window.authApi = {
        login: (request) => postJson('/auth/login', request),
        register: (request) => postJson('/auth/register', request),
        logout: () => postJson('/auth/logout')
    };
})();
