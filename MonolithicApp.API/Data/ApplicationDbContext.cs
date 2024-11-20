using Microsoft.EntityFrameworkCore;
using MonolithicApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonolithicApp.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        }
      
        public DbSet<FileMetadata> FileMetadata { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
