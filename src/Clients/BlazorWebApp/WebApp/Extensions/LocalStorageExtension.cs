using Blazored.LocalStorage;

namespace WebApp.Extensions
{
    public static class LocalStorageExtension
    {
        /// <summary>
        /// Get user name from local storage
        /// </summary>
        /// <param name="localStorageService">local storage service</param>
        /// <returns>username value</returns>
        public static string GetUserName(this ISyncLocalStorageService localStorageService)
        {
            return localStorageService.GetItemAsString("username");
        }

        /// <summary>
        /// Get user name from local storage async
        /// </summary>
        /// <param name="localStorageService"></param>
        /// <returns>username value</returns>
        public async static Task<string> GetUserName(this ILocalStorageService localStorageService)
        {
            return await localStorageService.GetItemAsync<string>("username");
        }

        /// <summary>
        /// Set user name
        /// </summary>
        /// <param name="localStorageService">local storage service</param>
        /// <param name="value">username value to set</param>
        public static void SetUserName(this ISyncLocalStorageService localStorageService, string value)
        {
            localStorageService.SetItem("username", value);
        }

        /// <summary>
        /// Set user name async
        /// </summary>
        /// <param name="localStorageService">local storage service</param>
        /// <param name="value">username value to set</param>
        /// <returns></returns>
        public async static Task SetUserName(this ILocalStorageService localStorageService, string value)
        {
            await localStorageService.SetItemAsync("username", value);
        }


        /// <summary>
        /// Get token
        /// </summary>
        /// <param name="localStorageService">local storage service</param>
        /// <returns>token value</returns>
        public static string GetToken(this ISyncLocalStorageService localStorageService)
        {
            return localStorageService.GetItem<string>("token");
        }

        /// <summary>
        /// Get token async
        /// </summary>
        /// <param name="localStorageService">local storage service</param>
        /// <returns>token value</returns>
        public async static Task<string> GetToken(this ILocalStorageService localStorageService)
        {
            return await localStorageService.GetItemAsync<string>("token");
        }

        /// <summary>
        /// Set token
        /// </summary>
        /// <param name="localStorageService">local storage service</param>
        /// <param name="value">token value</param>
        public static void SetToken(this ISyncLocalStorageService localStorageService, string value)
        {
            localStorageService.SetItem("token", value);
        }

        /// <summary>
        /// Set token async
        /// </summary>
        /// <param name="localStorageService"></param>
        /// <param name="value"></param>
        public static async void SetToken(this ILocalStorageService localStorageService, string value)
        {
            await localStorageService.SetItemAsync("token", value);
        }
    }
}
