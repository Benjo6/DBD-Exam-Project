using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib.Models;

[Table("personal_data", Schema = "prescriptions")]
public class PersonalDatum
{
    public PersonalDatum(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    [Column("id"), Key]
    public int Id { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("email")]
    public string? Email{ get; set; }
    [Column("login_id")]
    public int? LoginId { get; set; }
    [Column("role_id")]
    public int? RoleId { get; set; }
    [Column("address_id")]
    public int? AddressId { get; set; }

    public Address? Address { get; set; }
    public LoginInfo? Login { get; set; }
    public UserRole? Role { get; set; }
    public List<Doctor> Doctors { get; set; } = new();
    public List<Patient> Patients { get; set; } = new();
    public List<Pharmaceut> Pharmaceuts { get; set; } = new();
}