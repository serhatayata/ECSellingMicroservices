namespace WebApp.Domain.Models.User
{
    public class UserLoginRequest
    {
        /// <summary>
        /// User name info
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Password info
        /// </summary>
        public string Password { get; set; }

        public UserLoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
