using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Labb2.Models;

namespace Labb2.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly Labb2Context _context;

        public PurchasesController(Labb2Context context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index(int id)
        {
            List<Purchase> purchases;
            List<Car> car;

            ViewBag.CustomerId = id;

            if (id == 0)
            {
                purchases = await _context.Purchases.Include(p => p.Customer).Include(p => p.Car).ToListAsync();
            }
            else
            {
                ViewBag.Customer = _context.Customers.Find(id).Email;
                purchases = await _context.Purchases.Where(p => p.CustomerId == id).Include(p => p.Car).ToListAsync();
            }

            foreach (var p in purchases)
            {
                car = await _context.Cars.Where(s => s.Id == p.CarId).Include(s => s.Producer).ToListAsync();
                p.Car = car[0];
            }

            return View(purchases);
        }

        public IActionResult Purchase(int carId, int prId)
        {
            FillViewBag(carId, prId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(Customer model, int carId, int prId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email.Equals(model.Email));
            bool duplicate = customer == null ? false : _context.Purchases.Any(p => p.CarId == carId && p.CustomerId == customer.Id);

            if (duplicate)
            {
                ModelState.AddModelError("Email", "Ви вже придбали цей продукт");
            }

            if (ModelState.IsValid)
            {
                if (customer == null)
                {
                    customer = model;
                    await _context.Customers.AddAsync(customer);
                    await _context.SaveChangesAsync();
                }

                var purchase = new Purchase() { CustomerId = customer.Id, CarId = carId, Date = DateTime.Now };
                await _context.Purchases.AddAsync(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cars", new { id = prId, purchased = true });
            }

            FillViewBag(carId, prId);
            return View(model);
        }
        [HttpGet]
        public void FillViewBag(int? carId, int? prId)
        {
            ViewBag.ProducerId = prId;
            ViewBag.CarId = carId;
            ViewBag.Car = _context.Cars.Find(carId).Name;
        }


    }
}
