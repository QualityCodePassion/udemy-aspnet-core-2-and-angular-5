using System;
using Microsoft.AspNetCore.Http;

namespace SocialApp.API.Helpers
{
    // Extension Methods
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Application-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Application-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;

            if(birthDate.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}
