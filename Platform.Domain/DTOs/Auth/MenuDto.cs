namespace Platform.Domain.DTOs.Auth
{
    public class MenuDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Route { get; set; } = null!;
        public int Order { get; set; }
        public bool IsGroup { get; set; }
        public Guid ParentId { get; set; }
        public bool Status { get; set; }
        public List<MenuDto>? Children { get; set; }
        public List<PermissionDto>? Permissions { get; set; }
    }

    public class CreateMenuDto
    {
        public string Label { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Route { get; set; } = null!;
        public int Order { get; set; }
        public bool IsGroup { get; set; } = false;
        public Guid ParentId { get; set; }
        public bool Status { get; set; } = true;
        public List<Guid>? PermissionIds { get; set; }
    }

    public class UpdateMenuDto
    {
        public string Label { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Route { get; set; } = null!;
        public int Order { get; set; }
        public bool IsGroup { get; set; }
        public Guid ParentId { get; set; }
        public bool Status { get; set; }
        public List<Guid>? PermissionIds { get; set; }
    }

    public class MenuBasicDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Route { get; set; } = null!;
        public int Order { get; set; }
        public bool IsGroup { get; set; }
        public bool Status { get; set; }
    }

    public class MenuFilterDto
    {
        public string? Label { get; set; }
        public bool? IsGroup { get; set; }
        public bool? Status { get; set; }
        public Guid? ParentId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class MenuTreeDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; } = null!;
        public string Icon { get; set; } = null!;
        public string Route { get; set; } = null!;
        public int Order { get; set; }
        public bool IsGroup { get; set; }
        public bool Status { get; set; }
        public List<MenuTreeDto>? Children { get; set; }
    }
}
