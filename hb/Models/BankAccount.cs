using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hb.Models
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public decimal Balance { get; set; }

        public Currency Currency { get; set; }

        public ApplicationUser User { get; set; }

        [NotMapped]
        public SelectList CurrencyList { get; set; }
    }
}
