using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.APP.EF;
using Domain.Base;

namespace WebApp.Controllers
{
    public class TranslationController : Controller
    {
        private readonly AppDbContext _context;

        public TranslationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Translation
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Translations.Include(t => t.LangString);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Translation/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translation = await _context.Translations
                .Include(t => t.LangString)
                .FirstOrDefaultAsync(m => m.Culture == id);
            if (translation == null)
            {
                return NotFound();
            }

            return View(translation);
        }

        // GET: Translation/Create
        public IActionResult Create()
        {
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id");
            return View();
        }

        // POST: Translation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Culture,Value,LangStringId")] Translation translation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(translation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id", translation.LangStringId);
            return View(translation);
        }

        // GET: Translation/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translation = await _context.Translations.FindAsync(id);
            if (translation == null)
            {
                return NotFound();
            }
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id", translation.LangStringId);
            return View(translation);
        }

        // POST: Translation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Culture,Value,LangStringId")] Translation translation)
        {
            if (id != translation.Culture)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(translation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TranslationExists(translation.Culture))
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
            ViewData["LangStringId"] = new SelectList(_context.LangStrings, "Id", "Id", translation.LangStringId);
            return View(translation);
        }

        // GET: Translation/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var translation = await _context.Translations
                .Include(t => t.LangString)
                .FirstOrDefaultAsync(m => m.Culture == id);
            if (translation == null)
            {
                return NotFound();
            }

            return View(translation);
        }

        // POST: Translation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var translation = await _context.Translations.FindAsync(id);
            _context.Translations.Remove(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TranslationExists(string id)
        {
            return _context.Translations.Any(e => e.Culture == id);
        }
    }
}
