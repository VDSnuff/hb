using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hb.Models.TransactionViewModels
{
    public class CreateTransaction
    {
        public Transaction Transaction { get; set; }

        public BankAccount BankAccounts { get; set; }

        public BankAccount SenderBankAccounts { get; set; }

        public RecipientList Recipients { get; set; }

        [Display(Name = "Recipient")]
        public string NewRecipient { get; set; }

        [Display(Name = "Bank Account")]
        public int NewBankAccount { get; set; }

        public SelectList BankAccountsList { get; set; }

        public SelectList RecipientsList { get; set; }

        public SelectList SenderBankAccountsList { get; set; }

        public CreateTransaction()
        {
            this.Transaction = new Transaction();
            this.BankAccounts = new BankAccount();
            this.Recipients = new RecipientList();
            this.SenderBankAccounts = new BankAccount();
        }

    }
}
