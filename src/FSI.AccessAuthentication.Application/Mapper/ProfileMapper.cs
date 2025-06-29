using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Domain.Entities;

namespace FSI.AccessAuthentication.Application.Mapper
{
    public class ProfileMapper : BaseMapper<ProfileDto, ProfileEntity>
    {
        public override ProfileDto ToDto(ProfileEntity entity) => new ProfileDto
        {
            Id = entity.Id, //BaseEntity
            Name = entity.Name, //ProfileEntity
            CreatedAt = entity.CreatedAt, //BaseEntity
            UpdatedAt = entity.UpdatedAt, //BaseEntity
            IsActive = entity.IsActive //BaseEntity
        };

        public override ProfileEntity ToEntity(ProfileDto dto) => new ProfileEntity
        {
            Id = dto.Id, //BaseDto
            Name = dto.Name, //ProfileDto
            CreatedAt = dto.CreatedAt, //BaseDto
            UpdatedAt = dto.UpdatedAt, //BaseDto
            IsActive = dto.IsActive //BaseDto
        };
    }
}