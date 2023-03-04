namespace WebApp.Domain.Models.User
{
    public class UserLoginResponse
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User token from identity api
        /// </summary>
        public string UserToken { get; set; }
    }
}
