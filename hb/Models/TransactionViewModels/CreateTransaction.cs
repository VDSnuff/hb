using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hb.Models.TransactionViewModels
{
    public class CreateTransaction
    {
        public Transaction Transaction { get; set; }

        public BankAccount BankAccounts { get; set; }

        public RecipientList Recipients { get; set; }

        public SelectList BankAccountsList { get; set; }

        public SelectList RecipientsList { get; set; }
    }
}
