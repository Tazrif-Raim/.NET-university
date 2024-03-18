using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Zero_Hunger.Models.EF
{
    public class ZeroHungerDbContext : DbContext
    {
        public DbSet<Access> Accesses { get; set; }
        public DbSet<Benefactor> Benefactors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Food>  Foods { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
    }
}