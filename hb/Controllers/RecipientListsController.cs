using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hb.Data;
using hb.Models;
using Microsoft.AspNetCore.Authorization;

namespace hb.Controllers
{
    [Authorize]
    public class RecipientListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipientListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecipientLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipient.Include(r => r.Recipient).Include(a => a.RecipientAccount).Include(c => c.RecipientAccount.Currency).ToListAsync());
        }

        // GET: RecipientLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipientList = await _context.Recipient.Include(a => a.Recipient).Include(a => a.Sender).Include(a => a.RecipientAccount).Include(a => a.RecipientAccount.Currency).SingleOrDefaultAsync(m => m.Id == id);
            if (recipientList == null)
            {
                return NotFound();
            }

            return View(recipientList);
        }

        // GET: RecipientLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecipientLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] RecipientList recipientList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipientList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipientList);
        }

        // GET: RecipientLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipientList = await _context.Recipient.Include(r => r.Sender).Include(r => r.Recipient).Include(a => a.RecipientAccount).Include(c => c.RecipientAccount.Currency).SingleOrDefaultAsync(m => m.Id == id);
            if (recipientList == null)
            {
                return NotFound();
            }
            return View(recipientList);
        }

        // POST: RecipientLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecipientList recipientList)
        {
            //int id, [Bind("Id")]  RecipientList recipientList

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipientList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipientListExists(recipientList.Id))
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
            return View(recipientList);
        }

        // GET: RecipientLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipientList = await _context.Recipient
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recipientList == null)
            {
                return NotFound();
            }

            return View(recipientList);
        }

        // POST: RecipientLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipientList = await _context.Recipient.SingleOrDefaultAsync(m => m.Id == id);
            _context.Recipient.Remove(recipientList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipientListExists(int id)
        {
            return _context.Recipient.Any(e => e.Id == id);
        }
    }
}
