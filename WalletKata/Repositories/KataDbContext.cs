using Microsoft.EntityFrameworkCore;
using WalletKata.Models;

namespace WalletKata.Repositories
{
    public class KataDbContext : DbContext
    {
        public KataDbContext(DbContextOptions<KataDbContext> options) : base(options) { }

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
                new User { UserId = 1, },
                new User { UserId = 2 },
                new User { UserId = 3 }
            );

            // Seed currencies
            modelBuilder.Entity<Currency>().HasData(
                new Currency { CurrencyId = 1, Code = "ARS" },
                new Currency { CurrencyId = 2, Code = "EUR" },
                new Currency { CurrencyId = 3, Code = "USD" },
                new Currency { CurrencyId = 4, Code = "BRL" }, // Brazilian Real
                new Currency { CurrencyId = 5, Code = "COP" }  // Colombian Peso
            );

            // Seed exchange rates
            modelBuilder.Entity<ExchangeRate>().HasData(
             // ARS Exchange Rates
             new ExchangeRate { ExchangeRateId = 1, SourceCurrencyCode = "ARS", TargetCurrencyCode = "EUR", Rate = 0.000833m },
             new ExchangeRate { ExchangeRateId = 2, SourceCurrencyCode = "ARS", TargetCurrencyCode = "USD", Rate = 0.000870m },
             new ExchangeRate { ExchangeRateId = 3, SourceCurrencyCode = "ARS", TargetCurrencyCode = "BRL", Rate = 0.20m },
             new ExchangeRate { ExchangeRateId = 4, SourceCurrencyCode = "ARS", TargetCurrencyCode = "COP", Rate = 0.00027m },
             // EUR Exchange Rates
             new ExchangeRate { ExchangeRateId = 5, SourceCurrencyCode = "EUR", TargetCurrencyCode = "ARS", Rate = 1200m },
             new ExchangeRate { ExchangeRateId = 6, SourceCurrencyCode = "EUR", TargetCurrencyCode = "USD", Rate = 1.05m },
             new ExchangeRate { ExchangeRateId = 7, SourceCurrencyCode = "EUR", TargetCurrencyCode = "BRL", Rate = 5.0m },
             new ExchangeRate { ExchangeRateId = 8, SourceCurrencyCode = "EUR", TargetCurrencyCode = "COP", Rate = 4000m },
             // USD Exchange Rates
             new ExchangeRate { ExchangeRateId = 9, SourceCurrencyCode = "USD", TargetCurrencyCode = "ARS", Rate = 1150m },
             new ExchangeRate { ExchangeRateId = 10, SourceCurrencyCode = "USD", TargetCurrencyCode = "EUR", Rate = 0.952m },
             new ExchangeRate { ExchangeRateId = 11, SourceCurrencyCode = "USD", TargetCurrencyCode = "BRL", Rate = 5.0m },
             new ExchangeRate { ExchangeRateId = 12, SourceCurrencyCode = "USD", TargetCurrencyCode = "COP", Rate = 3400m },
             // BRL Exchange Rates
             new ExchangeRate { ExchangeRateId = 13, SourceCurrencyCode = "BRL", TargetCurrencyCode = "ARS", Rate = 5.0m },
             new ExchangeRate { ExchangeRateId = 14, SourceCurrencyCode = "BRL", TargetCurrencyCode = "EUR", Rate = 0.20m },
             new ExchangeRate { ExchangeRateId = 15, SourceCurrencyCode = "BRL", TargetCurrencyCode = "USD", Rate = 0.2m },
             new ExchangeRate { ExchangeRateId = 16, SourceCurrencyCode = "BRL", TargetCurrencyCode = "COP", Rate = 10000m },
             // COP Exchange Rates
             new ExchangeRate { ExchangeRateId = 17, SourceCurrencyCode = "COP", TargetCurrencyCode = "ARS", Rate = 4.0m },
             new ExchangeRate { ExchangeRateId = 18, SourceCurrencyCode = "COP", TargetCurrencyCode = "EUR", Rate = 0.00025m },
             new ExchangeRate { ExchangeRateId = 19, SourceCurrencyCode = "COP", TargetCurrencyCode = "USD", Rate = 0.00029m },
             new ExchangeRate { ExchangeRateId = 20, SourceCurrencyCode = "COP", TargetCurrencyCode = "BRL", Rate = 0.0001m }
         );
        }

    }
}
