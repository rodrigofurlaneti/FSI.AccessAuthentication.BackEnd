using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Application.Mapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;

namespace FSI.AccessAuthentication.Application.Services
{
    public class AuthenticationAppService : BaseAppService<AuthenticationDto, AuthenticationEntity>, IAuthenticationAppService
    {
        private readonly IAuthenticationRepository _repository;
        private readonly AuthenticationMapper _mapper;

        public AuthenticationAppService(IAuthenticationRepository repository)
            : base(repository, new AuthenticationMapper()) 
        {
            _repository = repository; // ✅ Isso que estava faltando
            _mapper = new AuthenticationMapper();
        }

        public List<string> ValidateAccess(AuthenticationRequestDto dto)
        {
            var errors = new List<string>();

            if (dto == null)
            {
                errors.Add("Request object is null.");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(dto.Username))
                errors.Add("UserName is required.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                errors.Add("Password is required.");

            if (!string.IsNullOrWhiteSpace(dto.Password) && dto.Password.Length < 6)
                errors.Add("Password must be at least 6 characters long.");

            return errors;
        }

        public async Task<AuthenticationDto> ValidationResultAccessAsync(AuthenticationRequestDto dto)
        {
            var result = new AuthenticationDto();

            var validationErrors = ValidateAccess(dto);
            if (validationErrors.Any())
            {
                result.IsAuthentication = false;
                result.ErrorMessage = string.Join(" | ", validationErrors);
                result.Expiration = DateTime.Now;
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = DateTime.Now;
                return result;
            }

            var authenticationRequestEntity = new AuthenticationRequestEntity() 
            { 
                Username = dto.Username, 
                Password = dto.Password 
            };

            var dbEntity = await _repository.GetByAuthenticationAsync(authenticationRequestEntity);

            // 🔥 Agora valida se a autenticação foi bem-sucedida
            if (dbEntity == null || !dbEntity.IsAuthentication)
            {
                result.IsAuthentication = false;
                result.Username = dto.Username;
                result.Password = dto.Password;
                result.Expiration = DateTime.Now;
                result.CreatedAt = DateTime.Now;
                result.UpdatedAt = DateTime.Now;
                result.ErrorMessage = dbEntity?.ErrorMessage ?? "Invalid username or password.";
                return result;
            }
            else
            {
                result.Expiration = dbEntity.Expiration != default ? dbEntity.Expiration : DateTime.Now.AddHours(1);
                result.Username = dbEntity.Username;
                result.Password = dbEntity.Password;
                result.SystemId = dbEntity.SystemId;
                result.IsActive = dbEntity.IsActive;
                result.CreatedAt = dbEntity.CreatedAt != default ? dbEntity.CreatedAt : DateTime.Now;
                result.UpdatedAt = dbEntity.UpdatedAt != default ? dbEntity.UpdatedAt : DateTime.Now;
                result.IsAuthentication = true;
                result.ErrorMessage = string.Empty;
            }

            return result;
        }
    }
}
