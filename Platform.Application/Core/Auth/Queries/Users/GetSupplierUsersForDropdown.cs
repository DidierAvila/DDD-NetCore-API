using Platform.Domain.Repositories;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using Platform.Domain.Enums;

namespace Platform.Application.Core.Auth.Queries.Users
{
    public class GetSupplierUsersForDropdown
    {
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IRepositoryBase<UserType> _userTypeRepository;

        public GetSupplierUsersForDropdown(
            IRepositoryBase<User> userRepository,
            IRepositoryBase<UserType> userTypeRepository)
        {
            _userRepository = userRepository;
            _userTypeRepository = userTypeRepository;
        }

        public async Task<IEnumerable<UserDropdownDto>> HandleAsync(CancellationToken cancellationToken)
        {
            // Primero obtenemos el tipo de usuario "Supplier"
            var supplierUserType = await _userTypeRepository.Find(
                x => x.Name.ToLower() == UserTypeEnum.Proveedor.ToString() && x.Status, 
                cancellationToken);
            
            if (supplierUserType == null)
                return new List<UserDropdownDto>();
                
            // Obtener solo los usuarios activos del tipo Supplier, ordenados alfabéticamente
            var activeSuppliers = await _userRepository.Finds(
                x => x.Status && x.UserTypeId == supplierUserType.Id, 
                cancellationToken);
            
            // Validar que no sea null y mapear solo Id y Name para máximo rendimiento
            if (activeSuppliers == null)
                return new List<UserDropdownDto>();
            
            return activeSuppliers
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