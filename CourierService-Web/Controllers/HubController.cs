using Microsoft.AspNetCore.Mvc;

namespace CourierService_Web.Controllers
{
    public class HubController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
