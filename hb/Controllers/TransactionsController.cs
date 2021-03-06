﻿using System;
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
using Microsoft.AspNetCore.Authorization;

namespace hb.Controllers
{
    [Authorize]
    public class TransactionsController : BaseController
    {
        public TransactionsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : base(userManager, context) { }

        // GET: Transactions
        public async Task<IActionResult> Index() => View(await _context.Transactions.Include(a => a.ReciverAccount).Include(u => u.Reciver).Include(c => c.SenderAccount.Currency).ToListAsync());

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
            string currentUserId = _userManager.GetUserId(HttpContext.User);
            
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
                string currentUserId = _userManager.GetUserId(HttpContext.User);

                if (ModelState.IsValid)
                {
                    var recipientBankAccount = _context.BankAccounts.Where(x => x.Number == newTransaction.RecipientBankAccount).FirstOrDefault();
                    var recipientUniqueName = _context.Users.Where(x => x.NormalizedUserName == newTransaction.RecipientUniqueName.ToUpper().Trim()).FirstOrDefault();
                    var senderAccountAmount = _context.BankAccounts.Where(s => s.Id == newTransaction.SenderBankAccountId).FirstOrDefault();

                    //If Adding Errore
                    if (senderAccountAmount.Balance < newTransaction.Sum)
                    {
                        var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
                        newTransaction.SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value");
                        ModelState.AddModelError("Sum", "Not enough amount of money on your account");
                        return View(newTransaction);
                    }
                    if (recipientUniqueName == null)
                    {
                        var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
                        newTransaction.SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value");
                        ModelState.AddModelError("RecipientUniqueName", "Recipient name does not exist.");
                        return View(newTransaction);
                    }
                    if (recipientBankAccount == null)
                    {
                        var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
                        newTransaction.SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value");
                        ModelState.AddModelError("RecipientBankAccount", "Recipient account number does not exist.");
                        return View(newTransaction);
                    }

                    Transaction transaction = new Transaction()
                    {
                        Date = newTransaction.Date,
                        Sender = _context.Users.Where(s => s.Id == currentUserId).FirstOrDefault(),
                        Reciver = _context.Users.Where(r => r.NormalizedUserName == newTransaction.RecipientUniqueName).FirstOrDefault(),
                        Title = newTransaction.Title,
                        Sum = newTransaction.Sum,
                        SenderAccount = _context.BankAccounts.Where(s => s.Id == newTransaction.SenderBankAccountId).FirstOrDefault(),
                        ReciverAccount = _context.BankAccounts.Where(s => s.Number == newTransaction.RecipientBankAccount).FirstOrDefault()
                    };

                    senderAccountAmount.Balance = senderAccountAmount.Balance - newTransaction.Sum;
                    recipientBankAccount.Balance = recipientBankAccount.Balance + newTransaction.Sum;
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






        // GET: Transactions/Create
        public IActionResult CreateFromRecipientList(int id)
        {
            string currentUserId = _userManager.GetUserId(HttpContext.User);

            var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });

            var predefinedRecipient = _context.Recipient.Include(r => r.Sender).Include(r => r.Recipient).Include(a => a.RecipientAccount).Include(c => c.RecipientAccount.Currency).FirstOrDefault(m => m.Id == id);

            CreateTransaction viewModel = new CreateTransaction
            {
                SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value"),
                Title = "Default Transaction",
                Date = DateTime.Now,
                Sum = 1,
                RecipientBankAccount = predefinedRecipient.RecipientAccount.Number,
                RecipientUniqueName = predefinedRecipient.Recipient.UserName
            };
            return View("Create", viewModel);

        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Date,Title,Sum")] Transaction transaction)
        public async Task<IActionResult> CreateFromRecipientList(CreateTransaction newTransaction)
        {
            try
            {
                string currentUserId = _userManager.GetUserId(HttpContext.User);

                if (ModelState.IsValid)
                {
                    var recipientBankAccount = _context.BankAccounts.Where(x => x.Number == newTransaction.RecipientBankAccount).FirstOrDefault();
                    var recipientUniqueName = _context.Users.Where(x => x.NormalizedUserName == newTransaction.RecipientUniqueName.ToUpper().Trim()).FirstOrDefault();
                    var senderAccountAmount = _context.BankAccounts.Where(s => s.Id == newTransaction.SenderBankAccountId).FirstOrDefault();

                    //If Adding Errore
                    if (senderAccountAmount.Balance < newTransaction.Sum)
                    {
                        var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
                        newTransaction.SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value");
                        ModelState.AddModelError("Sum", "Not enough amount of money on your account");
                        return View(newTransaction);
                    }
                    if (recipientUniqueName == null)
                    {
                        var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
                        newTransaction.SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value");
                        ModelState.AddModelError("RecipientUniqueName", "Recipient name does not exist.");
                        return View(newTransaction);
                    }
                    if (recipientBankAccount == null)
                    {
                        var senderBankAccountsList = _context.BankAccounts.Where(a => a.User.Id == currentUserId).Select(b => new { Id = b.Id, Value = b.Number });
                        newTransaction.SenderBankAccountsList = new SelectList(senderBankAccountsList, "Id", "Value");
                        ModelState.AddModelError("RecipientBankAccount", "Recipient account number does not exist.");
                        return View(newTransaction);
                    }

                    Transaction transaction = new Transaction()
                    {
                        Date = newTransaction.Date,
                        Sender = _context.Users.Where(s => s.Id == currentUserId).FirstOrDefault(),
                        Reciver = _context.Users.Where(r => r.NormalizedUserName == newTransaction.RecipientUniqueName).FirstOrDefault(),
                        Title = newTransaction.Title,
                        Sum = newTransaction.Sum,
                        SenderAccount = _context.BankAccounts.Where(s => s.Id == newTransaction.SenderBankAccountId).FirstOrDefault(),
                        ReciverAccount = _context.BankAccounts.Where(s => s.Number == newTransaction.RecipientBankAccount).FirstOrDefault()
                    };

                    senderAccountAmount.Balance = senderAccountAmount.Balance - newTransaction.Sum;
                    recipientBankAccount.Balance = recipientBankAccount.Balance + newTransaction.Sum;
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
