using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Discovery.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Defect> Defects { get; set; }
        public DbSet<DefectCategory> DefectCategories { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}