using System;
using System.Collections.Generic;
using Lab3.View;
using Microsoft.EntityFrameworkCore;

namespace Lab3;

public partial class PharmacyContext : DbContext
{
    public PharmacyContext()
    {
    }

    public PharmacyContext(DbContextOptions<PharmacyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Incoming> Incomings { get; set; }

    public virtual DbSet<IncomingDetail> IncomingDetails { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<MedicineDetail> MedicineDetails { get; set; }

    public virtual DbSet<MedicinesForDisease> MedicinesForDiseases { get; set; }

    public virtual DbSet<Outgoing> Outgoings { get; set; }

    public virtual DbSet<OutgoingDetail> OutgoingDetails { get; set; }

    public virtual DbSet<Producer> Producers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Data Source=DESKTOP-LA9KVTH\\SQLEXPRESS;Initial Catalog=Pharmacy;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Disease>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Incoming>(entity =>
        {
            entity.ToTable("Incoming");

            entity.Property(e => e.ArrivalDate).HasColumnType("date");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.Provider).HasMaxLength(50);

            entity.HasOne(d => d.MedicineName).WithMany(p => p.Incomings)
                .HasForeignKey(d => d.MedicineNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Incoming_Incoming");
        });

        modelBuilder.Entity<IncomingDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IncomingDetails");

            entity.Property(e => e.ArrivalDate).HasColumnType("date");
            entity.Property(e => e.MedicineName).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProducerName).HasMaxLength(100);
            entity.Property(e => e.Provider).HasMaxLength(50);
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.Property(e => e.ActiveSubstance).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ShortDescription).HasMaxLength(200);
            entity.Property(e => e.StorageLocation).HasMaxLength(50);
            entity.Property(e => e.UnitOfMeasurement).HasMaxLength(50);

            entity.HasOne(d => d.Producer).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.ProducerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Medicines_Producers");
        });

        modelBuilder.Entity<MedicineDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("MedicineDetails");

            entity.Property(e => e.ActiveSubstance).HasMaxLength(50);
            entity.Property(e => e.DiseaseName).HasMaxLength(50);
            entity.Property(e => e.MedicineDescription).HasMaxLength(200);
            entity.Property(e => e.MedicineName).HasMaxLength(50);
            entity.Property(e => e.ProducerCountry).HasMaxLength(20);
            entity.Property(e => e.ProducerName).HasMaxLength(100);
            entity.Property(e => e.StorageLocation).HasMaxLength(50);
            entity.Property(e => e.UnitOfMeasurement).HasMaxLength(50);
        });

        modelBuilder.Entity<MedicinesForDisease>(entity =>
        {
            entity.HasOne(d => d.Diseases).WithMany(p => p.MedicinesForDiseases)
                .HasForeignKey(d => d.DiseasesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicinesForDiseases_Medicines1");

            entity.HasOne(d => d.Midicines).WithMany(p => p.MedicinesForDiseases)
                .HasForeignKey(d => d.MidicinesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicinesForDiseases_Medicines");
        });

        modelBuilder.Entity<Outgoing>(entity =>
        {
            entity.ToTable("Outgoing");

            entity.Property(e => e.ImplementationDate).HasColumnType("date");
            entity.Property(e => e.SellingPrice).HasColumnType("money");

            entity.HasOne(d => d.MedicineName).WithMany(p => p.Outgoings)
                .HasForeignKey(d => d.MedicineNameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Outgoing_Medicines");
        });

        modelBuilder.Entity<OutgoingDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OutgoingDetails");

            entity.Property(e => e.ImplementationDate).HasColumnType("date");
            entity.Property(e => e.MedicineName).HasMaxLength(50);
            entity.Property(e => e.ProducerName).HasMaxLength(100);
            entity.Property(e => e.SellingPrice).HasColumnType("money");
        });

        modelBuilder.Entity<Producer>(entity =>
        {
            entity.Property(e => e.Country).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
