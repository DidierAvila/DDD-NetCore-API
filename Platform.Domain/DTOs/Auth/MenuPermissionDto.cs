namespace Platform.Domain.DTOs.Auth
{
    public class MenuPermissionDto
    {
        public Guid MenuId { get; set; }
        public Guid PermissionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public MenuBasicDto? Menu { get; set; }
        public PermissionBasicDto? Permission { get; set; }
    }

    public class CreateMenuPermissionDto
    {
        public Guid MenuId { get; set; }
        public Guid PermissionId { get; set; }
    }

    public class AssignPermissionToMenuDto
    {
        public Guid MenuId { get; set; }
        public Guid PermissionId { get; set; }
    }

    public class AssignMultiplePermissionsToMenuDto
    {
        public Guid MenuId { get; set; }
        public List<Guid> PermissionIds { get; set; } = new();
    }

    public class RemovePermissionFromMenuDto
    {
        public Guid MenuId { get; set; }
        public Guid PermissionId { get; set; }
    }

    public class RemoveMultiplePermissionsFromMenuDto
    {
        public Guid MenuId { get; set; }
        public List<Guid> PermissionIds { get; set; } = new();
    }
}
