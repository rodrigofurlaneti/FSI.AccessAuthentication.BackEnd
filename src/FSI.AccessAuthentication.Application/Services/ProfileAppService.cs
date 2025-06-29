using FSI.AccessAuthentication.Application.Dtos;
using FSI.AccessAuthentication.Application.Interfaces;
using FSI.AccessAuthentication.Application.Mapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;

namespace FSI.AccessAuthentication.Application.Services
{
    public class ProfileAppService : BaseAppService<ProfileDto, ProfileEntity>, IProfileAppService
    {
        public ProfileAppService(IProfileRepository repository)
            : base(repository, new ProfileMapper()) { }
    }
}
