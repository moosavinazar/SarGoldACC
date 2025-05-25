using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Storage;
using PersianDateControlsPlus.PersianDate;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Customer;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.Enums;
using SarGoldACC.Core.Services.Interfaces;
using SarGoldACC.WpfApp.Helpers;

namespace SarGoldACC.WpfApp.ViewModels;

public class DocumentViewModel : ViewModelBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentService _documentService;
    private readonly AppDbContext _dbContext;
    
    public ObservableCollection<CounterpartyDto> Counterparties { get; }
    public ObservableCollection<DocumentItemDto> DocumentItems { get; set; } = new();
    private DateTime _date;
    private PersianDate _persianDate = PersianDate.Today;
    public DateTime Date
    {
        get => _date;
        set
        {
            if (SetProperty(ref _date, value))
            {
                PersianDate = new PersianDate(value);
            }
        }
    }
    public PersianDate PersianDate
    {
        get => _persianDate;
        set
        {
            if (SetProperty(ref _persianDate, value))
            {
                Date = value.ToDateTime();
            }
        }
    }
    private long _counterpartyId;
    public long CounterpartyId
    {
        get => _counterpartyId;
        set => SetProperty(ref _counterpartyId, value);
    }
    public DocumentViewModel(IAuthorizationService authorizationService, AppDbContext appDbContext,
        ICounterpartyService counterpartyService, IDocumentService documentService)
    {
        _authorizationService = authorizationService;
        _dbContext = appDbContext;
        _counterpartyService = counterpartyService;
        _documentService = documentService;
        Counterparties = new ObservableCollection<CounterpartyDto>();
        
        Task.Run(async () =>
        {
            await LoadCustomersAsync();
        }).GetAwaiter().GetResult();
    }

    public async Task<ResultDto> SaveDocument(DocumentType type)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var result = new ResultDto()
            {
                Success = false
            };
            var documentItemListDto = new DocumentItemListDto
            {
                CounterpartySideOneId = CounterpartyId,
                Description = "test",
                Date = Date,
                DocumentItems = DocumentItems
            };
            result = await _documentService.AddOrderEntry(documentItemListDto, type);
            await transaction.CommitAsync();
            if (result.Success)
            {
                MessageBoxHelper.ShowSuccess("سند با موفقیت ذخیره شد.");
            }
            else
            {
                MessageBoxHelper.ShowError(result.Message);
            }
            return new ResultDto
            {
                Success = true,
                Message = "document added.",
                Data = documentItemListDto
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine(ex);
            return new ResultDto
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }
    
    private async Task LoadCustomersAsync()
    {
        Counterparties.Clear();
        var counterparties = await _counterpartyService.GetAllAsync();
        foreach (var c in counterparties)
        {
            Counterparties.Add(c);
        }
    }
    
}