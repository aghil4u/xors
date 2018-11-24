using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Server.Models.Equipment> Equipment { get; set; }
        public DbSet<Server.Models.Verification> Verification { get; set; }
        public DbSet<Server.Models.Employee> Employee { get; set; }
    }
}
