﻿using System.Collections.ObjectModel;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.Box;
using SarGoldACC.Core.DTOs.Branch;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class BoxViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IBoxService _boxService;
    private readonly IBranchService _branchService;
    
    private long? _editingId = null;
    private long _branchId;
    public long BranchId
    {
        get => _branchId;
        set => SetProperty(ref _branchId, value);
    }
    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    private double _weight;
    public double Weight
    {
        get => _weight;
        set => SetProperty(ref _weight, value);
    }
    private BoxType _type;
    public BoxType Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }
    public ObservableCollection<BoxTypeEnumItem> BoxTypes { get; set; }
    private ObservableCollection<BranchDto> _allBranches = new();
    public ObservableCollection<BranchDto> Branches
    {
        get => _allBranches;
        set => SetProperty(ref _allBranches, value);
    }
    private ObservableCollection<BoxDto> _allBoxes = new();
    public ObservableCollection<BoxDto> AllBoxes
    {
        get => _allBoxes;
        set => SetProperty(ref _allBoxes, value);
    }
    public bool CanAccessBoxView => _authorizationService.HasPermission("Box.View");
    public bool CanAccessBoxCreate => _authorizationService.HasPermission("Box.Create");
    public bool CanAccessBoxEdit => _authorizationService.HasPermission("Box.Edit");
    public bool CanAccessBoxDelete => _authorizationService.HasPermission("Box.Delete");
    public bool CanAccessBoxCreateOrEdit => _authorizationService.HasPermission("Box.Create") ||
                                             _authorizationService.HasPermission("Box.Edit");
    public BoxViewModel(IAuthorizationService authorizationService, 
        IBoxService boxService,
        IBranchService branchService)
    {
        _authorizationService = authorizationService;
        _boxService = boxService;
        _branchService = branchService;
        Branches = new ObservableCollection<BranchDto>();
        BoxTypes = new ObservableCollection<BoxTypeEnumItem>(
            Enum.GetValues(typeof(BoxType))
                .Cast<BoxType>()
                .Select(e => new BoxTypeEnumItem()
                {
                    Id = e,
                    Name = e.ToString()
                })
        );
        // بارگذاری تنظیمات و داده‌ها
        Task.Run(async () =>
        {
            await LoadBranchesAsync();
            await LoadBoxAsync();
        }).GetAwaiter().GetResult();
    }
    private async Task LoadBranchesAsync()
    {
        Branches.Clear();
        var groups = await _branchService.GetAllAsync();
        foreach (var g in groups)
        {
            Branches.Add(g);
        }
    }
    private async Task LoadBoxAsync()
    {
        AllBoxes.Clear();
        var boxes = await _boxService.GetAllAsync();
        foreach (var b in boxes)
        {
            AllBoxes.Add(b);
        }
    }
    public async Task SaveAsync()
    {
        var result = new ResultDto()
        {
            Success = false
        };
        if (_editingId.HasValue)
        {
            var dto = new BoxDto
            {
                Id = _editingId.Value,
                Name = Name
            };
            result = await _boxService.UpdateAsync(dto);
            _editingId = null;
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("شهر با موفقیت ویرایش شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        else
        {
            var boxCreate = new BoxCreateDto
            {
                Name = Name,
                Weight = Weight,
                Type = Type,
                BranchId = BranchId
            };
            result = await _boxService.AddAsync(boxCreate);
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("جعبه با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
        }
        await LoadBoxAsync();
    }
    public async Task DeleteAsync(long id)
    {
        await _boxService.DeleteAsync(id);
    }
    public async Task EditAsync(long id)
    {
        _editingId = id;
        var box = await _boxService.GetByIdAsync(id);
        Name = box.Name;
        Weight = box.Weight;
        Type = box.Type;
    }
    
}