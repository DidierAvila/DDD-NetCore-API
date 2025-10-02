using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Platform.Domain.Entities.Auth;

[Table(name: "Users", Schema ="Auth")]
public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Address { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; }
    public string? Image { get; set; }
    public string? Phone { get; set; }
    public required Guid UserTypeId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public required string ExtraData { get; set; }
    public required bool Status { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = [];

    public virtual ICollection<Session> Sessions { get; set; } = [];

    public virtual ICollection<Role> Roles { get; set; } = [];

    public virtual UserType? UserType { get; set; }
}
