using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.AccessAuthentication.Application.Mapper
{
    public class SystemMapper : BaseMapper<SystemDto, SystemEntity>
    {
        public override SystemDto ToDto(SystemEntity entity) => new SystemDto
        {
            Id = entity.Id, //BaseEntity
            Name = entity.Name, //SystemEntity
            CreatedAt = entity.CreatedAt, //BaseEntity
            UpdatedAt = entity.UpdatedAt //BaseEntity
            IsActive = entity.IsActive //BaseEntity
        };

        public override SystemEntity ToEntity(SystemDto dto) => new SystemEntity
        {
            Id = dto.Id, //BaseDto
            Name = dto.Name, //SystemDto
            CreatedAt = dto.CreatedAt, //BaseDto
            UpdatedAt = dto.UpdatedAt //BaseDto
            IsActive = dto.IsActive //BaseDto
        };
    }
}
