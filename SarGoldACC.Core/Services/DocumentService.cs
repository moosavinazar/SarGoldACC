using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.Invoice;
using SarGoldACC.Core.Models;
using SarGoldACC.Core.Repositories;
using SarGoldACC.Core.Repositories.Interfaces;
using SarGoldACC.Core.Services.Interfaces;

namespace SarGoldACC.Core.Services;

public class DocumentService : IDocumentService
{
    private readonly AppDbContext _dbContext;
    private readonly ICounterpartyService _counterpartyService;
    private readonly IDocumentRepository _documentRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IInvoiceRowRepository _invoiceRowRepository;
    private readonly IGeneralAccountAmountRepository _generalAccountAmountRepository;
    private readonly IMapper _mapper;

    public DocumentService(
        AppDbContext dbContext,
        ICounterpartyService counterpartyService,
        IDocumentRepository documentRepository,
        IInvoiceRepository invoiceRepository,
        IInvoiceRowRepository invoiceRowRepository,
        IGeneralAccountAmountRepository generalAccountAmountRepository,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _counterpartyService = counterpartyService;
        _documentRepository = documentRepository;
        _invoiceRepository = invoiceRepository;
        _invoiceRowRepository = invoiceRowRepository;
        _generalAccountAmountRepository = generalAccountAmountRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> AddCounterpartyOpeningEntry(CounterPartyOpeningEntryDto openingEntry)
    {
        // await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        // var counterparty = await _counterpartyService.GetByIdAsync(openingEntry.counterpartyId);
        var openingEntryCounterparty = await _counterpartyService.GetByIdAndBranchIdAsync(1, openingEntry.branchId);
        try
        {
            /*if (counterparty.CustomerId != null)
            {
                var customer = await _customerService.GetByIdAsync((long)counterparty.CustomerId);
                name = customer.Name;
            }*/

            var documentCreate = new DocumentCreateDto
            {
                Date = DateTime.Now,
                Description = "سند افتتاحیه"
            };
            Document document = _mapper.Map<Document>(documentCreate);
            var addedDocument = await _documentRepository.AddAsync(document);
            var invoice = new Invoice
            {
                DocumentId = addedDocument.Id,
                CounterpartyId = openingEntry.counterpartyId,
                Number = "1"
            };
            var addedInvoice = await _invoiceRepository.AddAsync(invoice);
            var openingEntryInvoice = new Invoice
            {
                DocumentId = addedDocument.Id,
                CounterpartyId = openingEntryCounterparty.Id,
                Number = "2"
            };
            var addedOpeningEntryInvoice = await _invoiceRepository.AddAsync(openingEntryInvoice);
            var generalAccountAmount = new GeneralAccountAmount
            {
                RiyalBed = openingEntry.RiyalBed,
                RiyalBes = openingEntry.RiyalBes,
                WeightBed = openingEntry.WeightBed,
                WeightBes = openingEntry.WeightBes,
            };
            var addedGeneralAccountAmount = await _generalAccountAmountRepository.AddAsync(generalAccountAmount);
            var invoiceRow = new InvoiceRow
            {
                InvoiceId = addedInvoice.Id,
                GeneralAccountAmountId = addedGeneralAccountAmount.Id,
                Description = "سند افتتاحیه"
            };
            await _invoiceRowRepository.AddAsync(invoiceRow);
            var openingEntrygeneralAccountAmount = new GeneralAccountAmount
            {
                RiyalBed = openingEntry.RiyalBes,
                RiyalBes = openingEntry.RiyalBed,
                WeightBed = openingEntry.WeightBes,
                WeightBes = openingEntry.WeightBed,
            };
            var addedOpeningEntryGeneralAccountAmount = await _generalAccountAmountRepository.AddAsync(openingEntrygeneralAccountAmount);
            var openingEntryInvoiceRow = new InvoiceRow
            {
                InvoiceId = addedOpeningEntryInvoice.Id,
                GeneralAccountAmountId = addedOpeningEntryGeneralAccountAmount.Id,
                Description = "سند افتتاحیه"
            };
            await _invoiceRowRepository.AddAsync(openingEntryInvoiceRow);
            return new ResultDto
            {
                Success = true,
                Message = "Document added."
            };
        }
        catch (Exception ex)
        {
            throw new Exception("خطا در ثبت سند افتتاحیه: " + ex.Message, ex);
        }
    }
}