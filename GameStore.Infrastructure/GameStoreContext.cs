using GameStore.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Infrastructure
{
    public class GameStoreContext : DbContext
    {
        public DbSet<PublisherModel> Publishers { get; set; }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<PcGameModel> PcGames { get; set; }
        public DbSet<ConsoleGameModel> ConsoleGames { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<PaymentReceiptModel> PaymentReceipts { get; set; }

        private readonly string _dbPath;

        public GameStoreContext()
        {
            _dbPath = FindSolutionDbPath();
        }

        public GameStoreContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
        {
            _dbPath = FindSolutionDbPath();
        }

        public string DbPath => _dbPath;

        private static string FindSolutionDbPath()
        {
            DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "GameStore.sln")))
                {
                    return Path.Combine(dir.FullName, "gamestore.db");
                }
                dir = dir.Parent;
            }

            DirectoryInfo? cwd = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (cwd != null)
            {
                if (File.Exists(Path.Combine(cwd.FullName, "GameStore.sln")))
                {
                    return Path.Combine(cwd.FullName, "gamestore.db");
                }
                cwd = cwd.Parent;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), "gamestore.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={_dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Table-per-Type (TPT) для наслідування ігор
            modelBuilder.Entity<GameModel>(entity =>
            {
                entity.ToTable("Games");
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Title).IsRequired().HasMaxLength(150);
                entity.Property(g => g.Genre).IsRequired().HasMaxLength(80);
            });

            modelBuilder.Entity<PcGameModel>(entity =>
            {
                entity.ToTable("PcGames");
                entity.Property(p => p.Platform).IsRequired().HasMaxLength(80);
            });

            modelBuilder.Entity<ConsoleGameModel>(entity =>
            {
                entity.ToTable("ConsoleGames");
                entity.Property(c => c.ConsoleType).IsRequired().HasMaxLength(80);
            });

            // Один-до-багатьох (1:N): PublisherModel -> GameModel
            modelBuilder.Entity<PublisherModel>(entity =>
            {
                entity.ToTable("Publishers");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(120);
                entity.Property(p => p.Country).HasMaxLength(80);

                entity.HasMany(p => p.Games)
                    .WithOne(g => g.Publisher)
                    .HasForeignKey(g => g.PublisherId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Один-до-одного (1:1): OrderModel <-> PaymentReceiptModel
            modelBuilder.Entity<OrderModel>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(120);

                entity.HasOne(o => o.Receipt)
                    .WithOne(r => r.Order)
                    .HasForeignKey<PaymentReceiptModel>(r => r.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Багато-до-багатьох (M:N): OrderModel <-> GameModel
                entity.HasMany(o => o.Games)
                    .WithMany(g => g.Orders)
                    .UsingEntity(j => j.ToTable("OrderGames"));
            });

            modelBuilder.Entity<PaymentReceiptModel>(entity =>
            {
                entity.ToTable("PaymentReceipts");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.ReceiptNumber).IsRequired().HasMaxLength(50);
                entity.Property(r => r.PaymentMethod).IsRequired().HasMaxLength(50);
            });
        }
    }
}
