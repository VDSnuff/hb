using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using hb.Data;
using hb.Models;

namespace hb.BL
{
    public class BusinessLogic
    {
        private readonly ApplicationDbContext _context;

        public BusinessLogic(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GenerateNumber()
        {
            Random rnd = new Random();
            int rndNumber = rnd.Next(1111111111, 2147483647);

            var num = _context.BankAccounts.Where(n => n.Number == rndNumber).FirstOrDefault();
            if (num == null) return rndNumber;

            else return GenerateNumber();
        }

    }
}
