using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hb.Models
{
    public class RecipientList
    {
        [Key]
        public int Id { get; set; }

        public ApplicationUser  Sender { get; set; }

        public ApplicationUser Recipient { get; set; }

        public BankAccount RecipientAccount { get; set; }
    }
}
