using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hb.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [MinLength(3), MaxLength(150)]
        public string Title { get; set; }
        [Required]
        public decimal Sum { get; set; }

        public ApplicationUser Sender { get; set; }

        public ApplicationUser Reciver { get; set; }
     
        public BankAccount SenderAccount { get; set; }
     
        public BankAccount ReciverAccount { get; set; }

    }
}
