using System;
using GasServiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GasServiceApp.Services;

public class GasDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientAddress> ClientAddresses { get; set; }
    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Master> Masters { get; set; }
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<WorkRecord> WorkRecords { get; set; }
    public DbSet<InspectionResult> InspectionResults { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("GAS_SERVICE_DB")
            ?? "Host=10.164.203.35;Port=5432;Database=gas_service_app;Username=postgres;Password=12345678";

        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Clients");
            entity.HasIndex(c => c.AccountNumber).IsUnique();
            entity.Property(c => c.AccountNumber).HasMaxLength(30);
            entity.Property(c => c.FullName).HasMaxLength(200);
            entity.Property(c => c.Phone).HasMaxLength(30);
            entity.Property(c => c.Email).HasMaxLength(100);
            entity.Property(c => c.Notes).HasMaxLength(500);
        });

        modelBuilder.Entity<ClientAddress>(entity =>
        {
            entity.ToTable("ClientAddresses");
            entity.Property(a => a.City).HasMaxLength(100);
            entity.Property(a => a.Street).HasMaxLength(150);
            entity.Property(a => a.House).HasMaxLength(20);
            entity.Property(a => a.Apartment).HasMaxLength(20);
            entity.Property(a => a.Entrance).HasMaxLength(20);
            entity.Property(a => a.Floor).HasMaxLength(20);
            entity.Property(a => a.FullAddress).HasMaxLength(300);
            entity.HasOne<Client>()
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.ToTable("Equipment");
            entity.HasIndex(e => e.SerialNumber).IsUnique();
            entity.Property(e => e.SerialNumber).HasMaxLength(64);
            entity.Property(e => e.Type).HasMaxLength(100);
            entity.Property(e => e.InstallationDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.NextInspectionDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.HasOne<Client>()
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<ClientAddress>()
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Master>(entity =>
        {
            entity.ToTable("Masters");
            entity.Property(m => m.FullName).HasMaxLength(200);
            entity.Property(m => m.Phone).HasMaxLength(30);
            entity.Property(m => m.Specialization).HasMaxLength(150);
            entity.Property(m => m.Qualification).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.ToTable("ServiceRequests");
            entity.Property(r => r.RequestDate).HasColumnType("timestamp without time zone");
            entity.Property(r => r.DeadlineDate).HasColumnType("timestamp without time zone");
            entity.Property(r => r.CompletionDate).HasColumnType("timestamp without time zone");
            entity.Property(r => r.RequestType).HasMaxLength(100);
            entity.Property(r => r.Priority).HasMaxLength(30);
            entity.Property(r => r.Status).HasMaxLength(50);
            entity.Property(r => r.Description).HasMaxLength(1000);
            entity.HasOne<Client>()
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<ClientAddress>()
                .WithMany()
                .HasForeignKey(r => r.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Equipment>()
                .WithMany()
                .HasForeignKey(r => r.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne<Master>()
                .WithMany()
                .HasForeignKey(r => r.MasterId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<WorkRecord>(entity =>
        {
            entity.ToTable("WorkRecords");
            entity.Property(w => w.WorkDate).HasColumnType("timestamp without time zone");
            entity.Property(w => w.WorkType).HasMaxLength(150);
            entity.Property(w => w.MaterialsUsed).HasMaxLength(500);
            entity.Property(w => w.Result).HasMaxLength(1000);
            entity.Property(w => w.Cost).HasPrecision(12, 2);
            entity.HasOne<ServiceRequest>()
                .WithMany()
                .HasForeignKey(w => w.ServiceRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Master>()
                .WithMany()
                .HasForeignKey(w => w.MasterId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<InspectionResult>(entity =>
        {
            entity.ToTable("InspectionResults");
            entity.Property(i => i.InspectionDate).HasColumnType("timestamp without time zone");
            entity.Property(i => i.GasLeakCheck).HasMaxLength(200);
            entity.Property(i => i.VentilationCheck).HasMaxLength(200);
            entity.Property(i => i.AutomationCheck).HasMaxLength(200);
            entity.Property(i => i.Conclusion).HasMaxLength(1000);
            entity.Property(i => i.Recommendations).HasMaxLength(1000);
            entity.HasOne<ServiceRequest>()
                .WithMany()
                .HasForeignKey(i => i.ServiceRequestId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne<Equipment>()
                .WithMany()
                .HasForeignKey(i => i.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
