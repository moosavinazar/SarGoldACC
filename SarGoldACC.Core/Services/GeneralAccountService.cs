using AutoMapper;
using SarGoldACC.Core.DTOs.GeneralAccountDto;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class GeneralAccountService : IGeneralAccountService
{
    private readonly IGeneralAccountRepository _generalAccountRepository;
    private readonly IMapper _mapper;

    public GeneralAccountService(IGeneralAccountRepository generalAccountRepository, IMapper mapper)
    {
        _generalAccountRepository = generalAccountRepository;
        _mapper = mapper;
    }

    public async Task<GeneralAccountDto> GetByIdAsync(long id)
    {
        var generalAccount = await _generalAccountRepository.GetByIdAsync(id);
        return _mapper.Map<GeneralAccountDto>(generalAccount);
    }

    public async Task<List<GeneralAccountDto>> GetAllAsync()
    {
        var generalAccounts = await _generalAccountRepository.GetAllAsync();
        return _mapper.Map<List<GeneralAccountDto>>(generalAccounts);
    }
}