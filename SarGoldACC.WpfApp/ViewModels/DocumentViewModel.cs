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

    private  ObservableCollection<CounterpartyDto> _counterparties;
    public ObservableCollection<CounterpartyDto> Counterparties
    {
        get => _counterparties;
        set
        {
            _counterparties = value;
            OnPropertyChanged();
            FilterCounterparties();
        }
    }
    private ObservableCollection<CounterpartyDto> _filteredCounterparties;
    public ObservableCollection<CounterpartyDto> FilteredCounterparties
    {
        get => _filteredCounterparties;
        set
        {
            _filteredCounterparties = value;
            OnPropertyChanged();
        }
    }
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
    private string _userImagePath = "pack://application:,,,/Resources/Icons/UserLarge.png";
    public string UserImagePath
    {
        get => _userImagePath;
        set
        {
            _userImagePath = value;
            OnPropertyChanged(nameof(UserImagePath));
        }
    }
    public bool IsCounterpartySelected => CounterpartyId != 0;
    private long _counterpartyId;
    public long CounterpartyId
    {
        get => _counterpartyId;
        set
        {
            if (_counterpartyId != value)
            {
                _counterpartyId = value;
                OnPropertyChanged();
                // بدون await فراخوانی async
                _ = LoadCounterpartyAsync(_counterpartyId);
            }
        }
    }
    private async Task LoadCounterpartyAsync(long id)
    {
        var counterparty = await _counterpartyService.GetByIdAsync(id);
        //TODO
        UserImagePath = counterparty.Customer?.Photo ?? "pack://application:,,,/Resources/Icons/UserLarge.png";
        OnPropertyChanged(nameof(IsCounterpartySelected));
    }
    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                FilterCounterparties();
            }
        }
    }
    
    public bool CanAccessCustomerButton => _authorizationService.HasPermission("Customer.View") ||
                                           _authorizationService.HasPermission("Customer.Create") ||
                                           _authorizationService.HasPermission("Customer.Edit") ||
                                           _authorizationService.HasPermission("Customer.Delete");
    public bool CanAccessDocumentCredit => _authorizationService.HasPermission("Document.Credit");
    public bool CanAccessDocumentDate => _authorizationService.HasPermission("Document.Date");
    public bool CanAccessDocumentGoldRisk => _authorizationService.HasPermission("Document.GoldRisk");
    public bool CanAccessDocumentRialRusk => _authorizationService.HasPermission("Document.RialRisk");
    public bool CanAccessDocumentLastTenDeal=> _authorizationService.HasPermission("Document.LastTenDeal");
    public bool CanAccessDocumentReport=> _authorizationService.HasPermission("Document.Report");
    public bool CanAccessDocumentOrder=> _authorizationService.HasPermission("Document.Order");
    public bool CanAccessDocumentMelted=> _authorizationService.HasPermission("Document.Melted");
    public bool CanAccessDocumentMisc=> _authorizationService.HasPermission("Document.Misc");
    public bool CanAccessDocumentMade=> _authorizationService.HasPermission("Document.Made");
    public bool CanAccessDocumentCoin=> _authorizationService.HasPermission("Document.Coin");
    public bool CanAccessDocumentSave=> _authorizationService.HasPermission("Document.Save");
    public bool CanAccessDocumentSaveTemporary=> _authorizationService.HasPermission("Document.SaveTemporary");
    
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
            await LoadCounterpartyAsync();
        }).GetAwaiter().GetResult();
        FilteredCounterparties = new ObservableCollection<CounterpartyDto>(Counterparties);
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
            return new ResultDto
            {
                Success = false,
                Message = ex.Message,
            };
        }
    }
    
    private async Task LoadCounterpartyAsync()
    {
        var counterparties = await _counterpartyService.GetAllAsync();
        Counterparties = new ObservableCollection<CounterpartyDto>(counterparties); // باعث اجرای FilterCounterparties میشه
    }

    
    private void FilterCounterparties()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            CounterpartyId = 0;
            FilteredCounterparties = new ObservableCollection<CounterpartyDto>(Counterparties);
        }
        else
        {
            var filtered = Counterparties
                .Where(c => c.Name != null && c.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();

            FilteredCounterparties = new ObservableCollection<CounterpartyDto>(filtered);
        }
    }
    
}