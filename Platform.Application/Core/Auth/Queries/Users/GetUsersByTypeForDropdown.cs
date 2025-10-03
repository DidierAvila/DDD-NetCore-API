using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Core.Auth.Queries.Users
{
    public class GetUsersByTypeForDropdown
    {
        private readonly IRepositoryBase<User> _userRepository;

        public GetUsersByTypeForDropdown(IRepositoryBase<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDropdownDto>> HandleAsync(Guid userTypeId, CancellationToken cancellationToken)
        {
            // Obtener solo los usuarios activos del tipo especificado, ordenados alfabéticamente
            var activeUsers = await _userRepository.Finds(
                x => x.Status && x.UserTypeId == userTypeId, 
                cancellationToken);
            
            // Validar que no sea null y mapear solo Id y Name para máximo rendimiento
            if (activeUsers == null)
                return new List<UserDropdownDto>();
            
            return activeUsers
                .OrderBy(u => u.Name)
                .Select(u => new UserDropdownDto 
                { 
                    Value = u.Id, 
                    Label = u.Name 
                })
                .ToList();
        }
    }
}