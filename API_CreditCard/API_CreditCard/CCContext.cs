using System;
using Microsoft.EntityFrameworkCore;

namespace API_CreditCard
{
	

        public class CCContext : DbContext
        {
            public CCContext(DbContextOptions<CCContext> options) : base(options) { }

            public DbSet<CreditCard> CreditCards { get; set; }
        }

}




