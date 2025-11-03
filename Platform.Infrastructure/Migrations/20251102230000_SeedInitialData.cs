using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // UserTypes
            var adminTypeId = new Guid("11111111-1111-1111-1111-111111111111");
            var userTypeId = new Guid("22222222-2222-2222-2222-222222222222");
            
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserTypes",
                columns: new[] { "Id", "Name", "Description", "Status", "Theme", "DefaultLandingPage", "LogoUrl", "Language", "CreatedAt" },
                values: new object[] { adminTypeId, "Administrador", "Administrador del sistema", true, "default", "/dashboard", null, "es", DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserTypes",
                columns: new[] { "Id", "Name", "Description", "Status", "Theme", "DefaultLandingPage", "LogoUrl", "Language", "CreatedAt" },
                values: new object[] { userTypeId, "Usuario", "Usuario estándar", true, "default", "/home", null, "es", DateTime.UtcNow });

            // Roles
            var adminRoleId = new Guid("33333333-3333-3333-3333-333333333333");
            var userRoleId = new Guid("44444444-4444-4444-4444-444444444444");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt" },
                values: new object[] { adminRoleId, "Administrador", "Rol con acceso completo", true, DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt" },
                values: new object[] { userRoleId, "Usuario", "Rol con acceso limitado", true, DateTime.UtcNow });

            // Permissions
            var viewDashboardPermId = new Guid("55555555-5555-5555-5555-555555555555");
            var manageUsersPermId = new Guid("66666666-6666-6666-6666-666666666666");
            var viewReportsPermId = new Guid("77777777-7777-7777-7777-777777777777");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt" },
                values: new object[] { viewDashboardPermId, "view:dashboard", "Ver dashboard", true, DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt" },
                values: new object[] { manageUsersPermId, "manage:users", "Administrar usuarios", true, DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt" },
                values: new object[] { viewReportsPermId, "view:reports", "Ver reportes", true, DateTime.UtcNow });

            // RolePermissions
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { adminRoleId, viewDashboardPermId });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { adminRoleId, manageUsersPermId });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { adminRoleId, viewReportsPermId });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { userRoleId, viewDashboardPermId });

            // Menus
            var dashboardMenuId = new Guid("88888888-8888-8888-8888-888888888888");
            var usersMenuId = new Guid("99999999-9999-9999-9999-999999999999");
            var reportsMenuId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Menus",
                columns: new[] { "Id", "Label", "Icon", "Route", "Order", "IsGroup", "ParentId", "Status", "CreatedAt" },
                values: new object[] { dashboardMenuId, "Dashboard", "dashboard", "/dashboard", 1, false, null, true, DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Menus",
                columns: new[] { "Id", "Label", "Icon", "Route", "Order", "IsGroup", "ParentId", "Status", "CreatedAt" },
                values: new object[] { usersMenuId, "Usuarios", "people", "/users", 2, false, null, true, DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Menus",
                columns: new[] { "Id", "Label", "Icon", "Route", "Order", "IsGroup", "ParentId", "Status", "CreatedAt" },
                values: new object[] { reportsMenuId, "Reportes", "assessment", "/reports", 3, false, null, true, DateTime.UtcNow });

            // MenuPermissions
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "MenuPermissions",
                columns: new[] { "MenuId", "PermissionId", "CreatedAt" },
                values: new object[] { dashboardMenuId, viewDashboardPermId, DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "MenuPermissions",
                columns: new[] { "MenuId", "PermissionId", "CreatedAt" },
                values: new object[] { usersMenuId, manageUsersPermId, DateTime.UtcNow });

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "MenuPermissions",
                columns: new[] { "MenuId", "PermissionId", "CreatedAt" },
                values: new object[] { reportsMenuId, viewReportsPermId, DateTime.UtcNow });

            // Admin User
            var adminId = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Users",
                columns: new[] { "Id", "Name", "Email", "Password", "UserTypeId", "Status", "ExtraData", "CreatedAt" },
                values: new object[] { adminId, "Administrador", "admin@example.com", 
                    // Contraseña: Admin123
                    "$2a$11$gVqx5hRrKs7i6/j1q3SH3uQBQKDrJJGpJyTYzPsRK3xVBUtI5c22S", 
                    adminTypeId, true, "{}", DateTime.UtcNow });

            // UserRoles
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { adminId, adminRoleId });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar datos en orden inverso para evitar problemas de integridad referencial
            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "UserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Users",
                keyColumn: "Id",
                keyValue: new object[] { });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "MenuPermissions",
                keyColumns: new[] { "MenuId", "PermissionId" },
                keyValues: new object[] { });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Menus",
                keyColumn: "Id",
                keyValue: new object[] { });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "RolePermissions",
                keyColumns: new[] { "RoleId", "PermissionId" },
                keyValues: new object[] { });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new object[] { });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Id",
                keyValue: new object[] { });

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "UserTypes",
                keyColumn: "Id",
                keyValue: new object[] { });
        }
    }
}