using System.Net.Http.Json;

namespace WebApp.Extensions
{
    public static class HttpClientExtension
    {
        /// <summary>
        /// Post with response async method
        /// </summary>
        /// <typeparam name="TResult">Post request's response value</typeparam>
        /// <typeparam name="TValue">Value which will be posted</typeparam>
        /// <param name="client">HttpClient extended</param>
        /// <param name="url">url post address</param>
        /// <param name="value">value to post</param>
        /// <returns>Post reqeust's response value</returns>
        public async static Task<TResult> PostGetResponseAsync<TResult,TValue>(this HttpClient client, string url, TValue value)
        {
            var httpRes = await client.PostAsJsonAsync(url, value);

            if (httpRes.IsSuccessStatusCode)
                return await httpRes.Content.ReadFromJsonAsync<TResult>();

            return default;
        }

        /// <summary>
        /// Post async
        /// </summary>
        /// <typeparam name="TValue">Post request value</typeparam>
        /// <param name="client">http client</param>
        /// <param name="url">url address</param>
        /// <param name="value">value to post</param>
        /// <returns></returns>
        public async static Task PostAsync<TValue>(this HttpClient client, string url, TValue value)
        {
            await client.PostAsJsonAsync(url, value);
        }

        /// <summary>
        /// Get request async
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="client">http client</param>
        /// <param name="url">url address</param>
        /// <returns>get request's response</returns>
        public async static Task<T> GetResponseAsync<T>(this HttpClient client, string url)
        {
            return await client.GetFromJsonAsync<T>(url);
        }
    }
}
