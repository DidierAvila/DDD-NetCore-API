using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Platform.Domain.Entities.Auth;

[Table(name: "Sessions", Schema = "Auth")]
public partial class Session
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string SessionToken { get; set; }

    public Guid? UserId { get; set; }

    public DateTime Expires { get; set; }

    public virtual User? User { get; set; }
}
