using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestDataApi
{
    public partial class PrescriptionContext : DbContext
    {
        public PrescriptionContext()
        {
        }

        public PrescriptionContext(DbContextOptions<PrescriptionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Doctor> Doctors { get; set; } = null!;
        public virtual DbSet<LoginInfo> LoginInfos { get; set; } = null!;
        public virtual DbSet<Medicine> Medicines { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<PersonalDatum> PersonalData { get; set; } = null!;
        public virtual DbSet<Pharmaceut> Pharmaceuts { get; set; } = null!;
        public virtual DbSet<Pharmacy> Pharmacies { get; set; } = null!;
        public virtual DbSet<Prescription> Prescriptions { get; set; } = null!;
        public virtual DbSet<UserPermission> UserPermissions { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=prescription-database;Port=5432;Database=prescription_db;Include Error Detail=true;Username=prescription_user;Password=prescription_pw");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Streetname)
                    .HasMaxLength(64)
                    .HasColumnName("streetname");

                entity.Property(e => e.Streetnumber)
                    .HasMaxLength(8)
                    .HasColumnName("streetnumber");

                entity.Property(e => e.Zipcode)
                    .HasMaxLength(32)
                    .HasColumnName("zipcode");
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("doctor", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonalDataId).HasColumnName("personal_data_id");

                entity.HasOne(d => d.PersonalData)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.PersonalDataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_personal_data");
            });

            modelBuilder.Entity<LoginInfo>(entity =>
            {
                entity.ToTable("login_info", "prescriptions");

                entity.HasIndex(e => e.Username, "login_info_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(64)
                    .HasColumnName("password");

                entity.Property(e => e.Salt)
                    .HasMaxLength(32)
                    .HasColumnName("salt");

                entity.Property(e => e.Username)
                    .HasMaxLength(64)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Medicine>(entity =>
            {
                entity.ToTable("medicine", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(64)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cpr)
                    .HasMaxLength(10)
                    .HasColumnName("cpr");

                entity.Property(e => e.PersonalDataId).HasColumnName("personal_data_id");

                entity.HasOne(d => d.PersonalData)
                    .WithMany(p => p.Patients)
                    .HasForeignKey(d => d.PersonalDataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_personal_data");
            });

            modelBuilder.Entity<PersonalDatum>(entity =>
            {
                entity.ToTable("personal_data", "prescriptions");

                entity.HasIndex(e => e.LoginId, "personal_data_login_id_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(64)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(64)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(64)
                    .HasColumnName("last_name");

                entity.Property(e => e.LoginId).HasColumnName("login_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.PersonalData)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("fk_address");

                entity.HasOne(d => d.Login)
                    .WithOne(p => p.PersonalDatum)
                    .HasForeignKey<PersonalDatum>(d => d.LoginId)
                    .HasConstraintName("fk_login");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PersonalData)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("fk_role");
            });

            modelBuilder.Entity<Pharmaceut>(entity =>
            {
                entity.ToTable("pharmaceut", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PersonalDataId).HasColumnName("personal_data_id");

                entity.Property(e => e.PharmacyId).HasColumnName("pharmacy_id");

                entity.HasOne(d => d.PersonalData)
                    .WithMany(p => p.Pharmaceuts)
                    .HasForeignKey(d => d.PersonalDataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_personal_data");

                entity.HasOne(d => d.Pharmacy)
                    .WithMany(p => p.Pharmaceuts)
                    .HasForeignKey(d => d.PharmacyId)
                    .HasConstraintName("fk_pharmacy");
            });

            modelBuilder.Entity<Pharmacy>(entity =>
            {
                entity.ToTable("pharmacy", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.PharmacyName)
                    .HasMaxLength(64)
                    .HasColumnName("pharmacy_name");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Pharmacies)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("fk_address");
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.ToTable("prescription", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Creation)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("creation");

                entity.Property(e => e.Expiration).HasColumnName("expiration");

                entity.Property(e => e.ExpirationWarningSent).HasColumnName("expiration_warning_sent");

                entity.Property(e => e.LastAdministeredBy).HasColumnName("last_administered_by");

                entity.Property(e => e.MedicineId).HasColumnName("medicine_id");

                entity.Property(e => e.PrescribedBy).HasColumnName("prescribed_by");

                entity.Property(e => e.PrescribedTo).HasColumnName("prescribed_to");

                entity.Property(e => e.PrescribedToCpr)
                    .HasMaxLength(10)
                    .HasColumnName("prescribed_to_cpr");

                entity.HasOne(d => d.LastAdministeredByNavigation)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.LastAdministeredBy)
                    .HasConstraintName("fk_administered_by");

                entity.HasOne(d => d.Medicine)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.MedicineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_medicine");

                entity.HasOne(d => d.PrescribedByNavigation)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.PrescribedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_prescriber");

                entity.HasOne(d => d.PrescribedToNavigation)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.PrescribedTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_prescribee");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.ToTable("user_permission", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_role", "prescriptions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.HasMany(d => d.Permissions)
                    .WithMany(p => p.Roles)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserRoleUserPermission",
                        l => l.HasOne<UserPermission>().WithMany().HasForeignKey("PermissionId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_permission"),
                        r => r.HasOne<UserRole>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_role"),
                        j =>
                        {
                            j.HasKey("RoleId", "PermissionId").HasName("user_role_user_permission_pkey");

                            j.ToTable("user_role_user_permission", "prescriptions");

                            j.IndexerProperty<int>("RoleId").HasColumnName("role_id");

                            j.IndexerProperty<int>("PermissionId").HasColumnName("permission_id");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
