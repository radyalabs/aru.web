using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Trisatech.MWorkforce.Domain.Entities;

namespace Trisatech.MWorkforce.Domain
{
    public class MobileForceContext : DbContext, IDataProtectionKeyContext
    {
        public MobileForceContext(DbContextOptions<MobileForceContext> options):
            base(options)
        {
            
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentDetail> AssignmentDetails { get; set; }
        public DbSet<AssignmentGroup> AssignmentGroups { get; set; }
        public DbSet<AssignmentStatus> AssignmentStatuses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<News> News { get; set; }
		public DbSet<Territory> Territories { get; set; }
		public DbSet<UserTerritory> UserTerritories { get; set; }
        public DbSet<CustomerDetail> CustomerDetails { get; set; }
        public DbSet<RefAssigment> RefAssigments { get; set; }
        public DbSet<CustomerContactAgent> CustomerContactAgents { get; set; }
        public DbSet<SalesManualReport> SalesManualReports { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
