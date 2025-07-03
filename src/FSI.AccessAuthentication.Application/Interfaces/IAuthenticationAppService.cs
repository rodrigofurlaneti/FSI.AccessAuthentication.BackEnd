using FSI.AccessAuthentication.Application.Dtos;

namespace FSI.AccessAuthentication.Application.Interfaces
{
    public interface IAuthenticationAppService : IBaseAppService<AuthenticationDto>
    {
        List<string> ValidateAccess(AuthenticationRequestDto dto);
        Task<AuthenticationDto> ValidationResultAccessAsync(AuthenticationRequestDto authenticationRequestsDto);
    }
}
