﻿using CourierService_Web.Data;
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
            var deliveredParcelCount = _context.Parcels.Where(p => p.HubId == hubId && p.DeliveryParcel.Id != null).Count();
            ViewBag.DeliveredParcelCount = deliveredParcelCount;

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
    }
}
