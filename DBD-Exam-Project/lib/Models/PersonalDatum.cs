using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MessagePack;

namespace lib.Models;

[Table("personal_data", Schema = "prescriptions"), MessagePackObject(keyAsPropertyName: true)]
public class PersonalDatum
{
    public PersonalDatum(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    [Column("id"), System.ComponentModel.DataAnnotations.Key]
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
    [IgnoreMember]
    public LoginInfo? Login { get; set; }
    [IgnoreMember]
    public UserRole? Role { get; set; }
    [IgnoreMember]
    public List<Doctor> Doctors { get; set; } = new();
    [IgnoreMember]
    public List<Patient> Patients { get; set; } = new();
    [IgnoreMember]
    public List<Pharmaceut> Pharmaceuts { get; set; } = new();
}