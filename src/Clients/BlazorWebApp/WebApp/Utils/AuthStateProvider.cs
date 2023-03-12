using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WebApp.Extensions;
using WebApp.Infrastructure;

namespace WebApp.Utils
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;
        private readonly HttpClient client;
        private readonly AuthenticationState anonymous;
        private readonly AppStateManager appState;

        public AuthStateProvider(ILocalStorageService localStorageService, HttpClient client, AppStateManager appState)
        {
            this.localStorageService = localStorageService;
            this.client = client;
            this.appState = appState;
            this.anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        /// <summary>
        /// Get authentication state and add it to authorization header
        /// </summary>
        /// <returns>authentication state</returns>
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string apiToken = await localStorageService.GetToken();

            if (string.IsNullOrEmpty(apiToken))
                return anonymous;

            string userName = await localStorageService.GetUserName();

            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,userName)
            }, "jwtAuthType"));

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);

            return new AuthenticationState(cp);
        }

        /// <summary>
        /// Inform blazor about user login
        /// </summary>
        /// <param name="userName"></param>
        public void NotifyUserLogin(string userName)
        {
            var cp = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName)
            }, "jwtAuthType"));

            var authState = Task.FromResult(new AuthenticationState(cp));

            NotifyAuthenticationStateChanged(authState);
            appState.LoginChanged(null);
        }

        /// <summary>
        /// Inform blazor about user logout
        /// </summary>
        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(anonymous);

            NotifyAuthenticationStateChanged(authState);
            appState.LoginChanged(null);
        }
    }
}
