using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Domain.Entities;

namespace FSI.AccessAuthentication.Application.Mapper
{
    public class AuthenticationMapper : BaseMapper<AuthenticationDto, AuthenticationEntity>
    {
        public override AuthenticationDto ToDto(AuthenticationEntity entity) => new AuthenticationDto
        {
            Id = entity.Id,
            SystemId = entity.SystemId,
            Password = entity.Password,
            IsAuthentication = entity.IsAuthentication,
            Expiration = entity.Expiration,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsActive = entity.IsActive
        };

        public override AuthenticationEntity ToEntity(AuthenticationDto dto) => new AuthenticationEntity
        {
            Id = dto.Id,
            SystemId = dto.SystemId,
            Password = dto.Password,
            IsAuthentication = dto.IsAuthentication,
            Expiration = dto.Expiration,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            IsActive = dto.IsActive
        };
    }
}