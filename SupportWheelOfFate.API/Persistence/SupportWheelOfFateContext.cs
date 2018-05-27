using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportWheelOfFate.API.Persistence
{
    public class SupportWheelOfFateContext : DbContext
    {
        public SupportWheelOfFateContext()
        {

        }

        public SupportWheelOfFateContext(DbContextOptions<SupportWheelOfFateContext> options)
            : base(options)
        {
            
        }

        public DbSet<Schedule> Schedules { get; set; }
    }
}
