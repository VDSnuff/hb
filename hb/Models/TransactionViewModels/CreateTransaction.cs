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
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Sum { get; set; }
        [Required]
        [Display(Name = "Recipient")]
        public string RecipientUniqueName { get; set; }
        [Required]
        [Display(Name = "Recipient Account Number")]
        public int RecipientBankAccount { get; set; }
        [Display(Name = "Your Account Number")]
        public int SenderBankAccountId { get; set; }
        public SelectList SenderBankAccountsList { get; set; }
        
        //public SelectList PredefinedRecipientsBankAccountsList { get; set; }
        //public SelectList PredefinedRecipientsList { get; set; }
        //public CreateTransaction()
        //{
        //    this.NewTransaction = new Transaction();
        //}
    }
}
