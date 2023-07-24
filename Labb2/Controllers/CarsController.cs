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
    public class CarsController : Controller
    {
        private readonly Labb2Context _context;
        private const string Er1 = "Така машина вже додана";

        public CarsController(Labb2Context context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index(int id, bool purchased)
        {
            List<Car> cars;
            ViewBag.ProducerId = id;

            if (purchased)
            {
                ViewBag.Purchased = 1;
            }
            if (id == 0)
            {
                cars=await _context.Cars.Include(c=>c.Producer).ToListAsync();
            }
            else
            {
                ViewBag.Producer = _context.Producers.Find(id).Name;
                cars = await _context.Cars.Where(c => c.ProducerId == id).Include(c => c.Producer).ToListAsync();
            }
            return View(cars);

           //var lab2Context = _context.Cars.Include(c => c.Producer);
            //return View(await lab2Context.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create(int pr_id)
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name");
            return View();
           
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProducerId,Name,Price,Description")] Car car)
        {
            bool duplicate = _context.Cars.Any(s => s.ProducerId == car.ProducerId && s.Name.Equals(car.Name));

            if (duplicate)
            {
                ModelState.AddModelError("Name", Er1);
            }

            if (ModelState.IsValid)
            {
            _context.Add(car);
             await _context.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
            }
             ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", car.ProducerId);
             return View(car);
         
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", car.ProducerId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProducerId,Name,Price,Description")] Car car)
        {
            bool duplicate = _context.Cars.Any(s => s.Id!=car.Id && s.ProducerId==car.ProducerId && s.Name.Equals(car.Name));

            if (duplicate)
            {
                ModelState.AddModelError("Name", Er1);
            }

            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Name", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cars == null)
            {
                return Problem("Entity set 'Lab2Context.Cars'  is null.");
            }
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
