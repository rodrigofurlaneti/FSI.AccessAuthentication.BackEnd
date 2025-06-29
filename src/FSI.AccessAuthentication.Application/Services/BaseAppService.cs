using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Application.Mapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;

namespace FSI.AccessAuthentication.Application.Services
{
    public abstract class BaseAppService<TDto, TEntity> : IBaseAppService<TDto>
         where TDto : class
         where TEntity : BaseEntity, new()
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly BaseMapper<TDto, TEntity> _mapper;

        protected BaseAppService(IBaseRepository<TEntity> repository, BaseMapper<TDto, TEntity> mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => _mapper.ToDto(e));
        }

        public virtual async Task<TDto?> GetByIdAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.ToDto(entity);
        }

        public virtual async Task<long> InsertAsync(TDto dto)
        {
            var entity = _mapper.ToEntity(dto);
            return await _repository.InsertAsync(entity);
        }

        public virtual async Task<bool> UpdateAsync(TDto dto)
        {
            var entity = _mapper.ToEntity(dto);
            return await _repository.UpdateAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(long id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;
            entity.Deactivate();
            return await _repository.UpdateAsync(entity);
        }
    }
}