using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace The_ATM_Machine {
    public class DatabaseContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder op) {
            op.UseSqlite("Data Source=atmDB.db");
        }
    }
    public class User {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }

    }
    public class Account {      
        public int AccountId { get; set; }
        public int BankNumber { get; set; }
        [MaxLength(4)]
        public int Pincode { get; set; }
        public int Money { get; set; }
        public User User { get; set; }
    }
}