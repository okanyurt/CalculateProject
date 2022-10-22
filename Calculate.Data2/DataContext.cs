using Calculate.Data.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Calculate.Data
{
    public class DataContext :  IdentityUserContext<User, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {       
            modelBuilder.Entity<User>().ToTable("users");

            var userLoginConfig = modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("user_logins");
            userLoginConfig.HasNoKey();
            userLoginConfig.Property(e => e.LoginProvider).HasColumnName("login_provider");
            userLoginConfig.Property(e => e.UserId).HasColumnName("user_id");
            userLoginConfig.Property(e => e.ProviderDisplayName).HasColumnName("provider_display_name");
            userLoginConfig.Property(e => e.ProviderKey).HasColumnName("provider_key");

            var userClaimConfig = modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("user_claims");
            userClaimConfig.HasKey(e => e.Id);
            userClaimConfig.Property(e => e.ClaimType).HasColumnName("claim_type");
            userClaimConfig.Property(e => e.ClaimValue).HasColumnName("claim_value");
            userClaimConfig.Property(e => e.Id).HasColumnName("id");
            userClaimConfig.Property(e => e.UserId).HasColumnName("user_id");

            var userTokenConfig = modelBuilder.Entity<IdentityUserToken<int>>().ToTable("user_tokens");
            userTokenConfig.HasNoKey();
            userTokenConfig.Property(e => e.Name).HasColumnName("name");
            userTokenConfig.Property(e => e.LoginProvider).HasColumnName("login_provider");
            userTokenConfig.Property(e => e.UserId).HasColumnName("user_id");
            userTokenConfig.Property(e => e.Value).HasColumnName("value");

        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        private static bool IsHiddenValue(Type entityType, string propertyName)
        {
            return entityType == typeof(User) && propertyName == "Password";
        }

        private static int GetEntityId(Type entityType, EntityEntry entityEntry)
        {          
            if (entityType == typeof(User))
            {
                return ((User)entityEntry.Entity).Id;
            }          
            return 0;
        }

        public override DbSet<User> Users { get; set; }

        public DbSet<Operation> Operations { get; set; }

        //public DbSet<Login> Logins { get; set; }
    }
}
