using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Application.Mapper;
using FSI.AccessAuthentication.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSI.AccessAuthentication.Application.Services
{
    public class MessagingAppService : IMessagingAppService
    {
        private readonly IMessagingRepository _repository; // ✅ Declaração

        public MessagingAppService(IMessagingRepository repository) // ✅ Construtor
        {
            _repository = repository;
        }

        #region Methods from IBaseAppService

        public async Task<IEnumerable<MessagingDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(MessagingMapper.ToDto);
        }


        public async Task<MessagingDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : MessagingMapper.ToDto(entity);
        }

        public async Task<long> InsertAsync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            return await _repository.InsertAsync(entity);
        }

        public async Task<bool> UpdateAsync(MessagingDto dto)
        {
            var entity = MessagingMapper.ToEntity(dto);
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            return await _repository.DeleteAsync(id);
        }

        #endregion
    }
}