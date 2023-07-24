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
    public class ProducersController : Controller
    {
        private const string ER1 = "В бд вже є даний виробник";
        private readonly Labb2Context _context;

        public ProducersController(Labb2Context context)
        {
            _context = context;
        }

        // GET: Producers
        public async Task<IActionResult> Index()
        {
            var lab2Context = _context.Producers.Include(p => p.Country);
            return View(await lab2Context.ToListAsync());
        }

        // GET: Producers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Producers == null)
            {
                return NotFound();
            }
            

            var producer = await _context.Producers
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producer == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Cars", new { id = producer.Id });
        }

        // GET: Producers/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CountryId,Name")] Producer producer)
        {
            bool duplicate = await _context.Producers.AnyAsync(p => p.Name.Equals(producer.Name));
            if (duplicate)
            {
                ModelState.AddModelError("Name", ER1);
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(producer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", producer.CountryId);
            return View(producer);
        }

        // GET: Producers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Producers == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers.FindAsync(id);
            if (producer == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", producer.CountryId);
            return View(producer);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Producer producer)
        {
            bool duplicate = await _context.Producers.AnyAsync(p => p.Name.Equals(producer.Name) && p.CountryId == producer.CountryId && p.Id != producer.Id);
            if (duplicate)
            {
                ModelState.AddModelError("Name", ER1);
            } 

            if (ModelState.IsValid)
            {
                  _context.Update(producer);
                    await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", producer.CountryId);
            return View(producer);
        }

        // GET: Producers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Producers == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers
                .Include(p => p.Country)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Producers == null)
            {
                return Problem("Entity set 'Lab2Context.Producers'  is null.");
            }
            var producer = await _context.Producers.FindAsync(id);
            
            
            if (producer != null)
            {
                
                _context.Producers.Remove(producer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducerExists(int id)
        {
          return (_context.Producers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
