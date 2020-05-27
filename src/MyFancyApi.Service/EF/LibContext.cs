using Microsoft.EntityFrameworkCore;
using MyFancyApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service
{
    public class LibContext : DbContext
    {
        public LibContext(DbContextOptions<LibContext> options) : base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Review> Reviews { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().ToTable("Author");
            modelBuilder.Entity<Book>().ToTable("Enrollment");
            modelBuilder.Entity<Review>().ToTable("Student");
        }
    }
}
