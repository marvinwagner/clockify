namespace Clockify.Tracking.Domain.Queries.ViewModels
{
    public class UserTokenViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }

    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserTokenViewModel UserToken { get; set; }
    }
}
