using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models.User;
using WebApp.Extensions;
using WebApp.Utils;

namespace WebApp.Application.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService syncLocalStorageService;
        private readonly AuthenticationStateProvider authStateProvider;

        public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.syncLocalStorageService = syncLocalStorageService;
            this.authStateProvider = authStateProvider;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

        /// <summary>
        /// Get user name
        /// </summary>
        /// <returns>user name value</returns>
        public string GetUserName()
        {
            return syncLocalStorageService.GetUserName();
        }

        /// <summary>
        /// Get user token
        /// </summary>
        /// <returns>token value</returns>
        public string GetUserToken()
        {
            return syncLocalStorageService.GetToken();
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <returns>is logged in</returns>
        public async Task<bool> Login(string userName, string password)
        {
            var request = new UserLoginRequest(userName, password);

            var response = await httpClient.PostGetResponseAsync<UserLoginResponse, UserLoginRequest>("auth", request);

            if (!string.IsNullOrEmpty(response.UserToken))
            {
                syncLocalStorageService.SetToken(response.UserToken);
                syncLocalStorageService.SetUserName(response.UserName);

                ((AuthStateProvider)authStateProvider).NotifyUserLogin(response.UserName);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.UserToken);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Logout
        /// </summary>
        public void Logout()
        {
            syncLocalStorageService.RemoveItem("token");
            syncLocalStorageService.RemoveItem("username");

            ((AuthStateProvider)authStateProvider).NotifyUserLogout();

            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
