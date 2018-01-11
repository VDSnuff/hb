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
    public class TransactionsController : BaseController
    {
        public TransactionsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : base(userManager, context) { }


        // GET: Transactions
        public async Task<IActionResult> Index()
        {
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

        //// GET: Transactions/Create
        //public IActionResult Create()
        //{
        //    string currentUserId = ViewBag.userId;
        //    // var predefinedRecipientsList = _context.Recipient.Where(r => r.Sender.Id == currentUserId).Select(r => new { Id = r.Id, Value = r.Recipient.UserName });
        //     var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
        //    //var predefinedRecipientsBankAccountsList = _context.

        //    CreateTransaction viewModel = new CreateTransaction
        //    {
        //        SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value"),
        //       // PredefinedRecipientsList = new SelectList(predefinedRecipientsList, "Id", "Value"),
        //       // PredefinedRecipientsBankAccountsList = new SelectList(predefinedRecipientsBankAccountsList, "Id", "Value"),
        //        Title = "Default Transaction",
        //        Date = DateTime.Now,
        //        Sum = 1.00
        //    };
        
        //    return View(viewModel);
        //}

        // GET: Transactions/Create
        public IActionResult Create()
        {
            string currentUserId = ViewBag.userId;
            var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
            CreateTransaction viewModel = new CreateTransaction
            {
                SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value"),
                Title = "Default Transaction",
                Date = DateTime.Now,
                Sum = 1
            };
            return View(viewModel);
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
                    var recipientBankAccount = _context.BankAccounts.Where(x => x.Number == newTransaction.RecipientBankAccount).FirstOrDefault();
                    var recipientUniqueName = _context.Users.Where(x => x.NormalizedUserName == newTransaction.RecipientUniqueName.ToUpper().Trim()).FirstOrDefault();
                    if (recipientUniqueName == null)
                    {
                        ModelState.AddModelError("RecipientUniqueName", "Recipient name does not exist.");
                        return View();
                    }
                    if (recipientBankAccount == null)
                    {
                        ModelState.AddModelError("RecipientBankAccount", "Recipient account number does not exist.");
                        return View();
                    }

                    Transaction transaction = new Transaction()
                    {
                        Date = newTransaction.Date,
                        Sender = ViewBag.userId,
                        Reciver = _context.Users.Where(r => r.NormalizedUserName == newTransaction.RecipientUniqueName).FirstOrDefault(),
                        Title = newTransaction.Title,
                        Sum = newTransaction.Sum,
                        SenderAccount = _context.BankAccounts.Where(s => s.Id == newTransaction.SenderBankAccountId).FirstOrDefault(),
                        ReciverAccount = _context.BankAccounts.Where(s => s.Number == newTransaction.RecipientBankAccount).FirstOrDefault()
                    };

                    _context.Add(transaction);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(newTransaction);
            }
            catch (Exception)
            {
                throw;
            }
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
