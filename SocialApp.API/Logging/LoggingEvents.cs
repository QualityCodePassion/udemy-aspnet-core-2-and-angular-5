namespace SocialApp.API.Logging
{
    // Based on the Microft docs, the LoggingEvents class is used to
    // map an "event ID" to different types of logging events. Read more from:
    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x
    public class LoggingEvents
    {
        public const int UserExists = 1000;
        public const int InvalidModelState = 1001;
    }
}