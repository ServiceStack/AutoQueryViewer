using ServiceStack;

namespace AutoQueryViewer.ServiceInterface
{
    public class CustomUserSession : AuthUserSession
    {
        public string DefaultProfileUrl { get; set; }
    }
}