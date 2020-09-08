using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace identity.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        // public override DbSet<ApplicationUser> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // "Server=tcp:mathiastestdb.database.windows.net,1433;Database=coreDB;User ID=<username>;Password=<password>;Encrypt=true;Connection Timeout=30;"
            //  "Server=localhost;Database=Saas.Identity;Trusted_Connection=True;"
            optionsBuilder.UseSqlServer("Server=tcp:mathiastestdb.database.windows.net,1433;Database=coreDB;User ID=mathiasR;Password=ykk8sCqK8j;Encrypt=true;Connection Timeout=30;");

        }

    }
}

