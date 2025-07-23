using AutoMapper;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class BoxService : IBoxService
{
    private readonly IBoxRepository _boxRepository;
    private readonly IMapper _mapper;

    public BoxService(IBoxRepository boxRepository, IMapper mapper)
    {
        _boxRepository = boxRepository;
        _mapper = mapper;
    }

    public async Task<BoxDto> GetByIdAsync(long id)
    {
        var box = await _boxRepository.GetByIdAsync(id);
        return _mapper.Map<BoxDto>(box);
    }

    public async Task<List<BoxDto>> GetAllAsync()
    {
        var cities = await _boxRepository.GetAllAsync();
        return _mapper.Map<List<BoxDto>>(cities);
    }
    
    public async Task<List<BoxDto>> GetAllByTypeAsync(BoxType type)
    {
        var cities = await _boxRepository.GetAllByTypeAsync(type);
        return _mapper.Map<List<BoxDto>>(cities);
    }

    public async Task<ResultDto> AddAsync(BoxCreateDto boxCreate)
    {
        try
        {
            var box = new Box
            {
                Name = boxCreate.Name,
                Weight = boxCreate.Weight,
                BranchId = boxCreate.BranchId,
                Type = boxCreate.Type,
            };
            await _boxRepository.AddAsync(box);
            return new ResultDto
            {
                Success = true,
                Message = "Box added.",
                Data = _mapper.Map<BoxDto>(box)
            };
        }
        catch (Exception ex)
        {
            return new ResultDto
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }

    public async Task<ResultDto> UpdateAsync(BoxDto boxDto)
    {
        var box = await _boxRepository.GetByIdAsync(boxDto.Id);
        if (box == null)
            throw new Exception("Box not found");

        _mapper.Map(boxDto, box);
        await _boxRepository.UpdateAsync(box);
        return new ResultDto
        {
            Success = true,
            Message = "Box added.",
            Data = _mapper.Map<BoxDto>(box)
        };
    }

    public async Task DeleteAsync(long id)
    {
        var box = await _boxRepository.GetByIdAsync(id);
        if (box == null)
            throw new Exception("Box not found");

        await _boxRepository.DeleteAsync(box);
    }
}