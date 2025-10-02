using Platform.Application.Core.Auth.Commands.Authentication;
using Platform.Application.Core.Auth.Commands.Handlers;
using Platform.Application.Core.Auth.Commands.Menus;
using Platform.Application.Core.Auth.Commands.Permissions;
using Platform.Application.Core.Auth.Commands.RolePermissions;
using Platform.Application.Core.Auth.Commands.Roles;
using Platform.Application.Core.Auth.Commands.Users;
using Platform.Application.Core.Auth.Commands.UserTypes;
using Platform.Application.Core.Auth.Queries.Handlers;
using Platform.Application.Core.Auth.Queries.Menus;
using Platform.Application.Core.Auth.Queries.Permissions;
using Platform.Application.Core.Auth.Queries.RolePermissions;
using Platform.Application.Core.Auth.Queries.Roles;
using Platform.Application.Core.Auth.Queries.UserMe;
using Platform.Application.Core.Auth.Queries.Users;
using Platform.Application.Core.Auth.Queries.UserTypes;
using Platform.Application.Mappings.Auth;
using Platform.Application.Services;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Repositories;
using Platform.Domain.Repositories.Auth;
using Platform.Infrastructure.Repositories;
using Platform.Infrastructure.Repositories.Auth;

namespace Platform.Api.Extensions
{
    /// <summary>
    /// Clase est�tica que contiene m�todos de extensi�n para configurar servicios en la API Platform.
    /// </summary>
    public static class ExtencionServices
    {
        /// <summary>
        /// Configura los servicios necesarios para la API Platform.
        /// </summary>
        /// <param name="services">La colecci�n de servicios de la aplicaci�n.</param>
        /// <returns>La colecci�n de servicios configurada.</returns>
        public static IServiceCollection AddApiExtention(this IServiceCollection services)
        {

            // AutoMapper
            services.AddAutoMapper(typeof(UserProfile), typeof(UserTypeProfile), typeof(PermissionProfile), typeof(AuthProfile), typeof(RoleProfile), typeof(RolePermissionProfile), typeof(MenuProfile), typeof(MenuPermissionProfile), typeof(UserTypePortalConfigProfile));

            // Authentication Commands
            services.AddScoped<ILoginCommand, LoginCommand>();

            // User Commands
            services.AddScoped<CreateUser>();
            services.AddScoped<UpdateUser>();
            services.AddScoped<DeleteUser>();
            services.AddScoped<ChangePassword>();
            services.AddScoped<UpdateUserAdditionalData>();
            services.AddScoped<AssignMultipleRolesToUser>();
            services.AddScoped<RemoveMultipleRolesFromUser>();
            services.AddScoped<IUserCommandHandler, UserCommandHandler>();

            // User Queries  
            services.AddScoped<GetUserById>();
            services.AddScoped<GetAllUsers>();
            services.AddScoped<GetAllUsersBasic>();
            services.AddScoped<GetAllUsersFiltered>();
            services.AddScoped<IUserQueryHandler, UserQueryHandler>();

            // UserType Commands
            services.AddScoped<CreateUserType>();
            services.AddScoped<UpdateUserType>();
            services.AddScoped<DeleteUserType>();
            services.AddScoped<IUserTypeCommandHandler, UserTypeCommandHandler>();

            // UserType Queries
            services.AddScoped<GetUserTypeById>();
            services.AddScoped<GetAllUserTypes>();
            services.AddScoped<GetActiveUserTypes>();
            services.AddScoped<GetUserTypesSummary>();
            services.AddScoped<GetUserTypesForDropdown>();
            services.AddScoped<GetAllUserTypesFiltered>();
            services.AddScoped<IUserTypeQueryHandler, UserTypeQueryHandler>();

            // Permission Commands
            services.AddScoped<CreatePermission>();
            services.AddScoped<UpdatePermission>();
            services.AddScoped<DeletePermission>();
            services.AddScoped<IPermissionCommandHandler, PermissionCommandHandler>();

            // Permission Queries
            services.AddScoped<GetPermissionById>();
            services.AddScoped<GetAllPermissions>();
            services.AddScoped<GetActivePermissions>();
            services.AddScoped<GetPermissionsSummary>();
            services.AddScoped<GetAllPermissionsFiltered>();
            services.AddScoped<GetPermissionsForDropdown>();
            services.AddScoped<IPermissionQueryHandler, PermissionQueryHandler>();

            // Role Commands
            services.AddScoped<CreateRole>();
            services.AddScoped<UpdateRole>();
            services.AddScoped<DeleteRole>();
            services.AddScoped<RemoveMultiplePermissionsFromRole>();
            services.AddScoped<IRoleCommandHandler, RoleCommandHandler>();

            // Role Queries  
            services.AddScoped<GetRoleById>();
            services.AddScoped<GetAllRoles>();
            services.AddScoped<GetRolesDropdown>();
            services.AddScoped<GetAllRolesFiltered>();
            services.AddScoped<IRoleQueryHandler, RoleQueryHandler>();

            // RolePermission Commands
            services.AddScoped<AssignPermissionToRole>();
            services.AddScoped<AssignMultiplePermissionsToRole>();
            services.AddScoped<RemovePermissionFromRole>();

            // RolePermission Queries
            services.AddScoped<GetAllRolePermissions>();
            services.AddScoped<GetRolesByPermission>();
            services.AddScoped<GetPermissionsByRole>();

            // Menu Commands
            services.AddScoped<CreateMenu>();
            services.AddScoped<UpdateMenu>();
            services.AddScoped<DeleteMenu>();
            services.AddScoped<IMenuCommandHandler, MenuCommandHandler>();

            // Menu Queries
            services.AddScoped<GetMenuById>();
            services.AddScoped<GetAllMenus>();
            services.AddScoped<GetMenuTree>();
            services.AddScoped<IMenuQueryHandler, MenuQueryHandler>();

            // UserMe Queries
            services.AddScoped<GetUserMe>();
            services.AddScoped<IUserMeQueryHandler, UserMeQueryHandler>();

            // Repositories
            services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
            services.AddScoped<IRepositoryBase<Session>, RepositoryBase<Session>>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IRepositoryBase<Role>, RepositoryBase<Role>>();
            services.AddScoped<IRepositoryBase<Permission>, RepositoryBase<Permission>>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IRepositoryBase<UserType>, RepositoryBase<UserType>>();
            services.AddScoped<IRepositoryBase<UserRole>, RepositoryBase<UserRole>>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IMenuPermissionRepository, MenuPermissionRepository>();
            services.AddScoped<IUserTypePortalConfigRepository, UserTypePortalConfigRepository>();

            // Services
            services.AddScoped<SessionInvalidationService>();
            
            // Registrar PermissionAuthorizationService
            services.AddScoped<PermissionAuthorizationService>();

            return services;
        }
    }
}
