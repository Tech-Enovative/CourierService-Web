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

        private void UpdateLayout()
        {
            //pickup request count
            var pickupRequestCount = _context.Parcels.Where(p => p.Status == "Pickup Request").Count();
            ViewBag.PickupRequestCount = pickupRequestCount;

            //total percel count according to hubId
            var hubId = Request.Cookies["HubId"];
            var totalParcelCount = _context.Parcels.Where(p => p.HubId == hubId).Count();
            ViewBag.TotalParcelCount = totalParcelCount;

            //delivered parcel count according to hubId
            var deliveredParcelCount = _context.Parcels.Where(p => p.HubId == hubId && p.DeliveryId != null).Count();
            ViewBag.DeliveredParcelCount = deliveredParcelCount;

           

            DateTime todayStart = DateTime.Today;
            DateTime tomorrowStart = todayStart.AddDays(1);
            //all parcel list for today
            var todayParcelList = _context.Parcels
                .Where(p => p.PickupRequestDate >= todayStart && p.PickupRequestDate < tomorrowStart).Include(u => u.Merchant).Include(u => u.Rider)
                .ToList();
            ViewBag.TodayParcelList = todayParcelList;

            //all parcel list for today count
            var todayParcelCount = todayParcelList.Count;
            ViewBag.TodayParcelCount = todayParcelCount;

            //return parcel count according to hubId
            var returnParcelCount = _context.Parcels.Where(p => p.HubId == hubId && p.ReturnId !=null).Count();
            ViewBag.ReturnParcelCount = returnParcelCount;

            //exchange parcel count according to hubId
            var exchangeParcelCount = _context.ExchangeParcels.Where(p => p.HubId == hubId).Count();
            ViewBag.ExchangeParcelCount = exchangeParcelCount;

            //parcel in hub count according to hubId
            var parcelInHubCount = _context.Parcels.Where(p => p.HubId == hubId && p.Status == "Parcel In Hub").Count();
            ViewBag.ParcelInHubCount = parcelInHubCount;

            //parcel Assigned A Rider For Pickup count according to hubId
            var parcelAssignedRiderForPickupCount = _context.Parcels.Where(p => p.HubId == hubId && p.Status == "Assigned A Rider For Pickup").Count();
            ViewBag.ParcelAssignedRiderForPickupCount = parcelAssignedRiderForPickupCount;

            //parcel Assigned For Delivery count according to hubId
            var parcelAssignedForDeliveryCount = _context.Parcels.Where(p => p.HubId == hubId && p.Status == "Assigned For Delivery").Count();
            ViewBag.ParcelAssignedForDeliveryCount = parcelAssignedForDeliveryCount;




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
            UpdateLayout();
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

        //Return parcel
        
        //Status - ReturnParcelInHub
        public IActionResult ReturnParcelHub(string id)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var returnParcel = _context.Parcels.Find(id);
            returnParcel.Status = "Return Parcel In Hub";
            _context.Parcels.Update(returnParcel);
            _context.SaveChanges();
            return RedirectToAction("ReturnParcel");
        }

        //status - ExchangeParcelHub
        public IActionResult ExchangeParcelHub(string id)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var exchangeParcel = _context.Parcels.Find(id);
            exchangeParcel.Status = "Exchange Parcel In Hub";
            _context.Parcels.Update(exchangeParcel);
            _context.SaveChanges();
            return RedirectToAction("ExchangeParcel");
        }
        //assign a parcel
        public IActionResult AssignParcel(string id)
        {

            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Get a list of available riders
            var riders = _context.Riders.Where(u => u.State == "Available");

            // Pass the list of riders to the view
            ViewBag.Riders = riders;


            return View(parcel);
        }

        public IActionResult AssignDeliveryRider(string id)
        {

            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }

            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Get a list of available riders
            var riders = _context.Riders.Where(u => u.State == "Available");

            // Pass the list of riders to the view
            ViewBag.Riders = riders;


            return View(parcel);
        }
        [HttpPost]
        public IActionResult AssignParcel(string id, string riderId)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }


            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Find the rider by ID
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                return NotFound();
            }

            // Assign the rider to the parcel
            parcel.Rider = rider;
            parcel.Status = "Assigned A Rider For Pickup";
            parcel.DispatchDate = DateTime.Now.Date;


            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Parcel Assigned Successfully";
            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Hub");
        }

        [HttpPost]
        public IActionResult AssignDeliveryRider(string id, string riderId)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }


            // Find the parcel by ID
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }

            // Find the rider by ID
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                return NotFound();
            }

            // Assign the rider to the parcel
            parcel.Rider = rider;
            parcel.Status = "Assigned For Delivery";
            //parcel.DispatchDate = DateTime.Now.Date;


            // Save changes to the database
            _context.SaveChanges();
            TempData["success"] = "Assigned Successfully";
            // Redirect to the parcel details page or any other desired page
            return RedirectToAction("Parcel", "Hub");
        }

        //assignRiderToMerchant
        public IActionResult AssignRiderToMerchant(string id)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            // Find the parcel by ID
            var parcel = _context.Parcels.Where(p=>p.Id == id).Include(m=>m.Merchant).FirstOrDefault();
            if (parcel == null)
            {
                return NotFound();
            }

            // Get a list of available riders
            var riders = _context.Riders.Where(u => u.State == "Available");

            // Pass the list of riders to the view
            ViewBag.Riders = riders;


            return View(parcel);
            
        }
        [HttpPost]
        public IActionResult AssignRiderToMerchant(string id, string riderId)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                return NotFound();
            }
            parcel.Rider = rider;
            parcel.Status = "Assigned For Return Parcel";
            //parcel.DispatchDate = DateTime.Now.Date;
            _context.SaveChanges();
            TempData["success"] = "Assigned Rider For Return Parcel Successfully";
            return RedirectToAction("Parcel", "Hub");
        }

        //RiderForExchnage
        public IActionResult RiderForExchange(string id)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Where(p => p.Id == id).Include(m => m.Merchant).FirstOrDefault();
            if (parcel == null)
            {
                return NotFound();
            }
            var riders = _context.Riders.Where(u => u.State == "Available");
            ViewBag.Riders = riders;
            return View(parcel);
        }
        [HttpPost]
        public IActionResult RiderForExchange(string id, string riderId)
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var parcel = _context.Parcels.Find(id);
            if (parcel == null)
            {
                return NotFound();
            }
            var rider = _context.Riders.Find(riderId);
            if (rider == null)
            {
                return NotFound();
            }
            parcel.Rider = rider;
            parcel.Status = "Assigned For Exchange Parcel";
            _context.SaveChanges();
            TempData["success"] = "Assigned Rider For Exchange Parcel Successfully";
            return RedirectToAction("Parcel", "Hub");
        }


        //status change to Parcel In Hub
        public IActionResult ParcelInHub(string id)
        {
            if (!IsHubLoggedIn())
            {

                return RedirectToAction("Login", "Home");
            }

            //var riderId = HttpContext.Request.Cookies["RiderId"];
            ////find rider by riderId
            //var rider = _context.Riders.Find(riderId);
            var parcel = _context.Parcels.Find(id);
            parcel.Status = "Parcel In Hub";
            parcel.DeliveryDate = DateTime.Now.Date;
            //rider.State = "Available";
            _context.Parcels.Update(parcel);
            //_context.Riders.Update(rider);
            _context.SaveChanges();
            return RedirectToAction("Parcel");
        }

        //Profile
        public IActionResult Profile()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            var hub = _context.Hubs.FirstOrDefault(h => h.Id == hubId);
            if (hub == null)
            {
                return NotFound();
            }
            return View(hub);
        }

        //delivery parcel list
        public IActionResult DeliveredParcelList()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            var deliveryParcels = _context.Parcels.Where(p => p.HubId == hubId && p.DeliveryId !=null).Include(u => u.Merchant).Include(u => u.Rider).ToList();
            if (deliveryParcels == null)
            {
                return NotFound();
            }
            return View(deliveryParcels);
        }

        //return parcel list
        public IActionResult ReturnParcelList()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            var returnParcels = _context.Parcels.Where(p => p.HubId == hubId && p.ReturnId !=null).Include(u => u.Merchant).Include(u => u.Rider).ToList();
            if (returnParcels == null)
            {
                return NotFound();
            }
            return View(returnParcels);
        }

        //exchange parcel list
        public IActionResult ExchangeParcelList()
        {
            if (!IsHubLoggedIn())
            {
                return RedirectToAction("Login", "Home");
            }
            var hubId = Request.Cookies["HubId"];
            var exchangeParcels = _context.Parcels.Where(p => p.HubId == hubId && p.ExchangeId != null).Include(u => u.Merchant).Include(u => u.Rider).ToList();
            if (exchangeParcels == null)
            {
                return NotFound();
            }
            return View(exchangeParcels);
        }
    }
}
