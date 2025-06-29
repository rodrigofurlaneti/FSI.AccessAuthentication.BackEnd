using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Domain.Entities;

namespace FSI.AccessAuthentication.Application.Mapper
{
    public class UserMapper : BaseMapper<UserDto, UserEntity>
    {
        public override UserDto ToDto(UserEntity entity) => new UserDto
        {
            Id = entity.Id, //BaseEntity
            Username = entity.Username, //UserEntity
            Password = entity.Password, //UserEntity
            Email = entity.Email, //UserEntity
            PhoneNumber = entity.PhoneNumber, //UserEntity
            SystemId = entity.SystemId, //UserEntity
            ProfileId = entity.ProfileId, //UserEntity
            CreatedAt = entity.CreatedAt, //BaseEntity
            UpdatedAt = entity.UpdatedAt //BaseEntity
            IsActive = entity.IsActive //BaseEntity
        };

        public override UserEntity ToEntity(UserDto dto) => new UserEntity
        {
            Id = dto.Id, //BaseDto
            Username = dto.Username, //UserDto
            Password = dto.Password, //UserDto
            Email = dto.Email, //UserDto
            PhoneNumber = dto.PhoneNumber, //UserDto
            SystemId = dto.SystemId, //UserDto
            ProfileId = dto.ProfileId, //UserDto
            CreatedAt = dto.CreatedAt, //BaseDto
            UpdatedAt = dto.UpdatedAt //BaseDto
            IsActive = dto.IsActive //BaseDto
        };
    }
}
