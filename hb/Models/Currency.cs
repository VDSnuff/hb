using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hb.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }

        public string Country { get; set; }

        public string CountryCode { get; set; }
    }
}
