using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Platform.Domain.Entities.Auth;

[Table("Accounts", Schema = "Auth")]
public partial class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string Type { get; set; } = null!;

    public string Provider { get; set; } = null!;

    public string ProviderAccountId { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public string? AccessToken { get; set; }

    public long? ExpiresAt { get; set; }

    public string? IdToken { get; set; }

    public string? Scope { get; set; }

    public string? SessionState { get; set; }

    public string? TokenType { get; set; }

    public virtual User? User { get; set; }
}
