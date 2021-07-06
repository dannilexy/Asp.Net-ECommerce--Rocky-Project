using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RockyProject.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RockyProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category>  Category { get; set; }
        public DbSet<ApplicationType>  ApplicationType { get; set; }
        public DbSet<Product>  Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

    }
}
