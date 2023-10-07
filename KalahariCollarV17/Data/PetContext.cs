using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KalahariCollarV17.Models;

namespace KalahariCollarV17.Data
{
    public class PetContext : DbContext
    {
        public PetContext (DbContextOptions<PetContext> options)
            : base(options)
        {
        }

        public DbSet<KalahariCollarV17.Models.Pet> Pet { get; set; } = default!;
    }
}
