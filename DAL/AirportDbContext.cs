using Microsoft.EntityFrameworkCore;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;

namespace DAL
{
    public class AirportDbContext : DbContext
    {
        public AirportDbContext(DbContextOptions<AirportDbContext> opts) : base(opts) {}

        public DbSet<Flight> Flights { get; set; }
        public DbSet<StationDB> Stations { get; set; }
        public DbSet<StationsLinks> StationsLinks { get; set; }
        public DbSet<StationEventDB> Events { get; set; }
    }
}
