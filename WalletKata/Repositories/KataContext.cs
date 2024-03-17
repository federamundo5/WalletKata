using Microsoft.EntityFrameworkCore;
using WalletKata.Models;

namespace WalletKata.Repositories
{
    public class KataContext : DbContext
    {
        public KataContext(DbContextOptions<KataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyWallet> CurrencyWallets { get; set; }
        public DbSet<ExchangeRate> ExchangeRate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().ToTable("Wallet");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Currency>().ToTable("Currency");
            modelBuilder.Entity<CurrencyWallet>().ToTable("CurrencyWallet");
            modelBuilder.Entity<ExchangeRate>().ToTable("ExchangeRate");

            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1,},
                new User { UserId = 2 },
                new User { UserId = 3 }
            );

            // Seed currencies
            modelBuilder.Entity<Currency>().HasData(
                new Currency { CurrencyId = 1, Code = "ARS" },
                new Currency { CurrencyId = 2, Code = "EUR" },
                new Currency { CurrencyId = 3, Code = "USD" }
            );

            // Seed exchange rates
            modelBuilder.Entity<ExchangeRate>().HasData(
       new ExchangeRate { ExchangeRateId = 1, SourceCurrencyCode = "ARS", TargetCurrencyCode = "EUR", Rate = 0.000833m }, // 1 ARS = 0.000833 EUR (1 / 1200)
       new ExchangeRate { ExchangeRateId = 2, SourceCurrencyCode = "ARS", TargetCurrencyCode = "USD", Rate = 0.000870m }, // 1 ARS = 0.000870 USD (1 / 1150)
       new ExchangeRate { ExchangeRateId = 3, SourceCurrencyCode = "EUR", TargetCurrencyCode = "USD", Rate = 1.05m }, // 1 EUR = 1.05 USD
       new ExchangeRate { ExchangeRateId = 4, SourceCurrencyCode = "EUR", TargetCurrencyCode = "ARS", Rate = 1200m }, // 1 EUR = 1200 ARS
       new ExchangeRate { ExchangeRateId = 5, SourceCurrencyCode = "USD", TargetCurrencyCode = "ARS", Rate = 1150m }, // 1 USD = 1150 ARS
       new ExchangeRate { ExchangeRateId = 6, SourceCurrencyCode = "USD", TargetCurrencyCode = "EUR", Rate = 0.952m } // 1 USD = 0.952 EUR (1 / 1.05)
   );


        }
    
    }
}
