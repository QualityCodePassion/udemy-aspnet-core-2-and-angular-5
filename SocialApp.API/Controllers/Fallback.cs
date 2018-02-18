using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SocialApp.API.Controllers
{
    [AllowAnonymous]
    public class Fallback : Controller
    {
        public IActionResult Index()
        {
            // Return the generated Angular files built by Angular cli
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), 
                "wwwroot", "index.html"), "text/HTML");
        }
    }
}
