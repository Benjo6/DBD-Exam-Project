using lib.Models;
using Microsoft.EntityFrameworkCore;

namespace PrescriptionService.Data;

public class PostgresContext: DbContext
{
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<PersonalDatum> PersonalData => Set<PersonalDatum>();
    public DbSet<Pharmaceut> Pharmaceuts => Set<Pharmaceut>();
    public DbSet<Pharmacy> Pharmacies => Set<Pharmacy>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();

    public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.PersonalData)
            .WithMany(p => p.Doctors)
            .HasForeignKey(d => d.PersonalDataId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<Patient>()
            .HasOne(d => d.PersonalData)
            .WithMany(p => p.Patients)
            .HasForeignKey(d => d.PersonalDataId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        modelBuilder.Entity<PersonalDatum>(entity =>
        {
            entity.HasOne(d => d.Address)
                .WithMany(p => p.PersonalData)
                .HasForeignKey(d => d.AddressId);

            entity.HasOne(d => d.Login)
                .WithOne(p => p.PersonalDatum)
                .HasForeignKey<PersonalDatum>(d => d.LoginId);

            entity.HasOne(d => d.Role)
                .WithMany(p => p.PersonalData)
                .HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<Pharmaceut>(entity =>
        {
            entity.HasOne(d => d.PersonalData)
                .WithMany(p => p.Pharmaceuts)
                .HasForeignKey(d => d.PersonalDataId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Pharmacy)
                .WithMany(p => p.Pharamceuts)
                .HasForeignKey(d => d.PharamacyId);
        });

        modelBuilder.Entity<Pharmacy>()
            .HasOne(d => d.Address)
            .WithMany(p => p.Pharmacies)
            .HasForeignKey(d => d.AddressId);

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasOne(d => d.LastAdministeredByNavigation)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.LastAdministeredBy);

            entity.HasOne(d => d.Medicine)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PrescribedByNavigation)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.PrescribedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PrescribedToNavigation)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.PrescribedTo)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
    }
}
