namespace FSI.AccessAuthentication.Application.Interfaces
{
    public interface IBaseAppService<TDto>
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(long id);
        Task<long> InsertAsync(TDto dto);
        Task<bool> UpdateAsync(TDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
