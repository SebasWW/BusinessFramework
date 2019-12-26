using System;
using Microsoft.EntityFrameworkCore;
using MyCompany.MyApp.EntityFramework.Entity;

namespace MyCompany.MyApp.EntityFramework
{
	public class MyAppDbContext : DbContext
	{
		public DbSet<DTCompany> Companies { get; set; }
		public DbSet<DTUser> Users { get; set; }
		public DbSet<DTCompanyUser> CompanyUsers { get; set; }
		public DbSet<DTOrder> Orders { get; set; }

		public MyAppDbContext(DbContextOptions options) : base(options)
		{
		}

		public MyAppDbContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlite("Filename=MyAppDb.db");
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DTCompany>(
				b =>
				{
					b.HasIndex(t => t.Id)
						.IsUnique();

					b.Property(t => t.Id)
						.ValueGeneratedOnAdd()
						.IsRequired();

					b.Property(t => t.Name)
						.IsRequired();
				}
			);

			modelBuilder.Entity<DTUser>(
				b =>
				{
					b.HasIndex(t => t.Id)
						.IsUnique();

					b.Property(t => t.Id)
						.ValueGeneratedOnAdd()
						.IsRequired();

					b.Property(t => t.Name)
						.IsRequired();
				}
			);

			modelBuilder.Entity<DTCompanyUser>(
				b =>
				{
					b.HasIndex(t => t.Id)
						.IsUnique();

					b.Property(t => t.Id)
						.ValueGeneratedOnAdd()
						.IsRequired();

					b.HasIndex(t => new { t.CompanyId, t.UserId })
						.IsUnique();

					b.HasOne(t => t.Company)
						.WithMany(m => m.CompanyUsers)
						.HasForeignKey(t => t.CompanyId);

					b.HasOne(t => t.User)
						.WithMany(m => m.CompanyUsers)
						.HasForeignKey(t => t.UserId);
				}
			);

			modelBuilder.Entity<DTOrder>(
				b =>
				{
					b.HasIndex(t => t.Id)
						.IsUnique();

					b.Property(t => t.Id)
						.ValueGeneratedOnAdd()
						.IsRequired();

					b.Property(t => t.Name)
						.IsRequired();

					b.HasOne(t => t.Company1)
						.WithMany(m => m.Orders1)
						.HasForeignKey(t => t.Company1Id);

					b.HasOne(t => t.Company2)
						.WithMany(m => m.Orders2)
						.HasForeignKey(t => t.Company2Id);

					b.HasOne(t => t.User)
						.WithMany(m => m.Orders)
						.HasForeignKey(t => t.UserId);
				}
			);
		}
	}
}
