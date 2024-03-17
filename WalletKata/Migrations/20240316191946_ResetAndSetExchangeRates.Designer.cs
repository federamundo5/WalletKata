﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WalletKata.Repositories;

#nullable disable

namespace WalletKata.Migrations
{
    [DbContext(typeof(KataDbContext))]
    [Migration("20240316191946_ResetAndSetExchangeRates")]
    partial class ResetAndSetExchangeRates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.22");

            modelBuilder.Entity("WalletKata.Models.Currency", b =>
                {
                    b.Property<long>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currency", (string)null);

                    b.HasData(
                        new
                        {
                            CurrencyId = 1L,
                            Code = "ARS"
                        },
                        new
                        {
                            CurrencyId = 2L,
                            Code = "EUR"
                        },
                        new
                        {
                            CurrencyId = 3L,
                            Code = "USD"
                        });
                });

            modelBuilder.Entity("WalletKata.Models.CurrencyWallet", b =>
                {
                    b.Property<long>("CurrencyWalletId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<long>("CurrencyId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("WalletId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CurrencyWalletId");

                    b.ToTable("CurrencyWallet", (string)null);
                });

            modelBuilder.Entity("WalletKata.Models.ExchangeRate", b =>
                {
                    b.Property<long>("ExchangeRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Rate")
                        .HasColumnType("TEXT");

                    b.Property<string>("SourceCurrencyCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("TargetCurrencyCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ExchangeRateId");

                    b.ToTable("ExchangeRate", (string)null);

                    b.HasData(
                        new
                        {
                            ExchangeRateId = 1L,
                            Rate = 0.000833m,
                            SourceCurrencyCode = "ARS",
                            TargetCurrencyCode = "EUR"
                        },
                        new
                        {
                            ExchangeRateId = 2L,
                            Rate = 0.000870m,
                            SourceCurrencyCode = "ARS",
                            TargetCurrencyCode = "USD"
                        },
                        new
                        {
                            ExchangeRateId = 3L,
                            Rate = 1.05m,
                            SourceCurrencyCode = "EUR",
                            TargetCurrencyCode = "USD"
                        },
                        new
                        {
                            ExchangeRateId = 4L,
                            Rate = 1200m,
                            SourceCurrencyCode = "EUR",
                            TargetCurrencyCode = "ARS"
                        },
                        new
                        {
                            ExchangeRateId = 5L,
                            Rate = 1150m,
                            SourceCurrencyCode = "USD",
                            TargetCurrencyCode = "ARS"
                        },
                        new
                        {
                            ExchangeRateId = 6L,
                            Rate = 0.952m,
                            SourceCurrencyCode = "USD",
                            TargetCurrencyCode = "EUR"
                        });
                });

            modelBuilder.Entity("WalletKata.Models.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("User", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = 1L
                        },
                        new
                        {
                            UserId = 2L
                        },
                        new
                        {
                            UserId = 3L
                        });
                });

            modelBuilder.Entity("WalletKata.Models.Wallet", b =>
                {
                    b.Property<long>("WalletId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("WalletId");

                    b.ToTable("Wallet", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
