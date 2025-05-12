using AutoMapper;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.Invoice;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class DocumentService : IDocumentService
{
    private readonly AppDbContext _dbContext;
    private readonly CounterpartyService _counterpartyService;
    private readonly CustomerService _customerService;
    private readonly GeneralAccountService _generalAccountService;
    
    private readonly DocumentRepository _documentRepository;
    private readonly IMapper _mapper;

    public DocumentService(
        AppDbContext dbContext,
        CounterpartyService counterpartyService,
        CustomerService customerService,
        GeneralAccountService generalAccountService,
        DocumentRepository documentRepository,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _counterpartyService = counterpartyService;
        _customerService = customerService;
        _generalAccountService = generalAccountService;
        _documentRepository = documentRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> AddCounterpartyOpeningEntry(CounterPartyOpeningEntryDto openingEntry)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var counterparty = await _counterpartyService.GetByIdAsync(openingEntry.counterpartyId);
        var generalAccount = await _generalAccountService.GetByIdAsync(1);
        var openingEntryCounterparty = await _counterpartyService.GetByIdAndBranchIdAsync(generalAccount.Id, counterparty.BranchId);
        string name = "";
        if (counterparty.CustomerId != null)
        {
            var customer = await _customerService.GetByIdAsync((long)counterparty.CustomerId);
            name = customer.Name;
        }
        var documentCreate = new DocumentCreateDto
        {
            Date = DateTime.Now,
            Description = "سند افتتاحیه برای " + name
        };
        Document document = _mapper.Map<Document>(documentCreate);
        var addedDocument = _documentRepository.AddWithoutSave(document);
        await _dbContext.SaveChangesAsync();
        var invoiceCreate = new InvoiceCreateDto
        {
            DocumentId = addedDocument.Id,
            CounterpartyId = counterparty.Id,
            Number = "1"
        };
        var openingEntryInvoiceCreate = new InvoiceCreateDto
        {
            DocumentId = addedDocument.Id,
            CounterpartyId = openingEntryCounterparty.Id,
            Number = "2"
        };
        var addedInvoiceCreate = .AddWithoutSave(document);
        await _dbContext.SaveChangesAsync();
        var addedOpeningEntryInvoiceCreate = _documentRepository.AddWithoutSave(document);
        await _dbContext.SaveChangesAsync();
    }
}