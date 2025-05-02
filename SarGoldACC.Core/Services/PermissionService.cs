using AutoMapper;
using SarGoldACC.Core.DTOs.Auth;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IMapper _mapper;

    public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }
    
    public async Task<PermissionDto> GetByIdAsync(int id)
    {
        var permission = await _permissionRepository.GetByIdAsync(id);
        return _mapper.Map<PermissionDto>(permission);
    }

    public async Task<List<PermissionDto>> GetAllAsync()
    {
        var permissions = await _permissionRepository.GetAllAsync();
        return _mapper.Map<List<PermissionDto>>(permissions);
    }
}