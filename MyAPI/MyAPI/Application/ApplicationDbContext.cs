using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using MyAPI.Model;
using PundoPH.Model;
using System;

namespace MyAPI.Application
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; } // Example entity
        public DbSet<ContributionModel> Contributions { get; set; }
        public DbSet<WithdrawModel> Withdraws { get; set; }
    }
}
