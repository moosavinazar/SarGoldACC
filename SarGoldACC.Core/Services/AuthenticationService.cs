using AutoMapper;
using SarGoldACC.Core.DTOs.Auth.User;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationRepository _authenticationRepository;
    private readonly IMapper _mapper;

    public AuthenticationService(IAuthenticationRepository authenticationRepository, IMapper mapper)
    {
        _authenticationRepository = authenticationRepository;
        _mapper = mapper;
    }

    public async Task<UserDto> AuthenticateUserAsync(string username, string password)
    {
        var user = await _authenticationRepository.AuthenticateUserAsync(username, password);
        return _mapper.Map<UserDto>(user);
    }
}
