using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Platform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Datos iniciales para UserTypes
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserTypes",
                columns: new[] { "Id", "Name", "Description", "Status", "Theme", "DefaultLandingPage", "LogoUrl", "Language", "AdditionalConfig", "CreatedAt", "UpdatedAt" },
                values: new object[] { Guid.NewGuid(), "Administrador", "Tipo de usuario con acceso completo al sistema", true, "light", "/dashboard", null, "es", null, DateTime.Now, null }
            );

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "UserTypes",
                columns: new[] { "Id", "Name", "Description", "Status", "Theme", "DefaultLandingPage", "LogoUrl", "Language", "AdditionalConfig", "CreatedAt", "UpdatedAt" },
                values: new object[] { Guid.NewGuid(), "Usuario", "Tipo de usuario con acceso limitado", true, "light", "/home", null, "es", null, DateTime.Now, null }
            );

            // Datos iniciales para Roles
            var adminRoleId = Guid.NewGuid();
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt", "UpdatedAt" },
                values: new object[] { adminRoleId, "Administrador", "Rol con todos los permisos", true, DateTime.Now, null }
            );

            var userRoleId = Guid.NewGuid();
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Roles",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt", "UpdatedAt" },
                values: new object[] { userRoleId, "Usuario", "Rol con permisos básicos", true, DateTime.Now, null }
            );

            // Datos iniciales para Permissions
            var viewDashboardPermissionId = Guid.NewGuid();
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt", "UpdatedAt" },
                values: new object[] { viewDashboardPermissionId, "ViewDashboard", "Permiso para ver el dashboard", true, DateTime.Now, null }
            );

            var manageUsersPermissionId = Guid.NewGuid();
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "Permissions",
                columns: new[] { "Id", "Name", "Description", "Status", "CreatedAt", "UpdatedAt" },
                values: new object[] { manageUsersPermissionId, "ManageUsers", "Permiso para gestionar usuarios", true, DateTime.Now, null }
            );

            // Asignar permisos a roles
            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { adminRoleId, viewDashboardPermissionId }
            );

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { adminRoleId, manageUsersPermissionId }
            );

            migrationBuilder.InsertData(
                schema: "Auth",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                values: new object[] { userRoleId, viewDashboardPermissionId }
            );

            // Datos iniciales para Countries
            migrationBuilder.InsertData(
                schema: "App",
                table: "Countries",
                columns: new[] { "Alpha2Code", "Name", "Alpha3Code", "Capital", "Region" },
                values: new object[] { "ES", "España", "ESP", "Madrid", "Europa" }
            );

            migrationBuilder.InsertData(
                schema: "App",
                table: "Countries",
                columns: new[] { "Alpha2Code", "Name", "Alpha3Code", "Capital", "Region" },
                values: new object[] { "US", "Estados Unidos", "USA", "Washington D.C.", "América" }
            );

            migrationBuilder.InsertData(
                schema: "App",
                table: "Countries",
                columns: new[] { "Alpha2Code", "Name", "Alpha3Code", "Capital", "Region" },
                values: new object[] { "MX", "México", "MEX", "Ciudad de México", "América" }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar datos de Countries
            migrationBuilder.DeleteData(
                schema: "App",
                table: "Countries",
                keyColumn: "Alpha2Code",
                keyValue: "ES");

            migrationBuilder.DeleteData(
                schema: "App",
                table: "Countries",
                keyColumn: "Alpha2Code",
                keyValue: "US");

            migrationBuilder.DeleteData(
                schema: "App",
                table: "Countries",
                keyColumn: "Alpha2Code",
                keyValue: "MX");

            // Eliminar datos de RolePermissions, Permissions y Roles
            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "Name",
                keyValue: "ViewDashboard");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Permissions",
                keyColumn: "Name",
                keyValue: "ManageUsers");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Administrador");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "Roles",
                keyColumn: "Name",
                keyValue: "Usuario");

            // Eliminar datos de UserTypes
            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "UserTypes",
                keyColumn: "Name",
                keyValue: "Administrador");

            migrationBuilder.DeleteData(
                schema: "Auth",
                table: "UserTypes",
                keyColumn: "Name",
                keyValue: "Usuario");
        }
    }
}
