using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Application.Mapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;

namespace FSI.AccessAuthentication.Application.Services
{
    public class SystemAppService : BaseAppService<SystemDto, SystemEntity>, ISystemAppService
    {
        public SystemAppService(ISystemRepository repository)
            : base(repository, new SystemMapper()) { }
    }
}
