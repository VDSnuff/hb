using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hb.Data;
using hb.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using hb.Models.TransactionViewModels;

namespace hb.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;


        public TransactionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            ViewBag.userId = _userManager.GetUserId(HttpContext.User);
            return View(await _context.Transactions.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            CreateTransaction model = new CreateTransaction();

            string currentUserId = ViewBag.userId;

            var senderBankAccounts = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });

            //For Predefined Recipient
            var recipients = _context.Recipient.Where(r => r.Sender.Id == currentUserId).Select(r => new { Id = r.Id, Value = r.Recipient.UserName });

            var recipientsBankAccounts = _context.Recipient.Where(a => a.Recipient.Id == currentUserId);

            model.BankAccountsList = new SelectList(senderBankAccounts, "Id", "Value");
            model.RecipientsList = new SelectList(recipients, "Id", "Value");
            model.SenderBankAccountsList = new SelectList(senderBankAccounts, "Id", "Value");
            model.Transaction.Date = DateTime.Now;

            return View(model);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Date,Title,Sum")] Transaction transaction)
        public async Task<IActionResult> Create(CreateTransaction newTransaction)
        {
            try
            {
                string currentUserId = ViewBag.userId;

                if (ModelState.IsValid)
                {

                    if(newTransaction.BankAccountsList == null || newTransaction.RecipientsList == null)
                    {
                        Transaction transaction = new Transaction();
                        transaction.Date = newTransaction.Transaction.Date;
                        transaction.Title = newTransaction.Transaction.Title;
                        transaction.Sum = newTransaction.Transaction.Sum;
                        transaction.Reciver = _context.Users.Where( r => r.UserName == newTransaction.NewRecipient).FirstOrDefault();
                        transaction.ReciverAccount = _context.BankAccounts.Where(a => a.Number == newTransaction.NewBankAccount).FirstOrDefault();
                        transaction.Sender = _context.Users.Where(s => s.UserName == currentUserId).FirstOrDefault();
                       // transaction.SenderAccount = _context.BankAccounts.Where(a => a.);
                       //transaction.SenderAccount = _context.BankAccounts.Where(a => a.Number == newTransaction.Nu).
                    }
                    


                    return RedirectToAction(nameof(Index));
                }

                return View(newTransaction);
               
            }
            catch (Exception)
            {
                throw;
            }



            //if (ModelState.IsValid)
            //{
            //    _context.Add(transaction);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.SingleOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Title,Sum")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .SingleOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(m => m.Id == id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
