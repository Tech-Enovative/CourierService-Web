using CourierService_Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourierService_Web.Controllers
{
    public class HubController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HubController(ApplicationDbContext context)
        {
            _context = context;
        }

        //ishublogged in
        public bool IsHubLoggedIn()
        {
            if (Request.Cookies["HubId"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Parcel()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcels = _context.Parcels.Include(u => u.Merchant).Include(u => u.Rider).ToList();
            if (parcels == null)
            {
                return NotFound();
            }
            return View(parcels);
        }
    }
}
