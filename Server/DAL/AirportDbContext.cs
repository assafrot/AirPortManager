using Microsoft.EntityFrameworkCore;
using Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.DAL
{
    public class AirportDbContext : DbContext
    {
        public AirportDbContext(DbContextOptions<AirportDbContext> opts) : base(opts) {}

        public DbSet<FlightDB> Airplanes { get; set; }
        public DbSet<StationDB> Stations { get; set; }
        public DbSet<StationEventDB> Events { get; set; }
        public DbSet<FlightDB> Flights { get; set; }
    }
}
