using generic_api.Model.Auth;

namespace generic_api.Model.Messages.Responses
{
    public class LoginResponse 
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        //Should these be able to be 'set' ?
        public AccessToken AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
