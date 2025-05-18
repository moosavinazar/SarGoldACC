using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using SarGoldACC.Core.Data;
using SarGoldACC.Core.DTOs;
using SarGoldACC.Core.DTOs.CounterParty;
using SarGoldACC.Core.DTOs.Document;
using SarGoldACC.Core.DTOs.Invoice;
using SarGoldACC.Core.Enums;
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

    public async Task<ResultDto> AddCounterpartyOpeningEntry(OrderDto openingEntry)
    {
        var openingEntryCounterparty = await _counterpartyService.GetByIdAndBranchIdAsync(1, openingEntry.branchId);
        try
        {
            var generalAccountAmount = new GeneralAccountAmount
            {
                Description = "سند افتتاحیه",
                RiyalBed = openingEntry.RiyalBed,
                RiyalBes = openingEntry.RiyalBes,
                WeightBed = openingEntry.WeightBed,
                WeightBes = openingEntry.WeightBes,
            };
            var invoiceRow = new InvoiceRow
            {
                Description = "سند افتتاحیه",
                GeneralAccountAmount = generalAccountAmount
            };
            var openingEntryGeneralAccountAmount = new GeneralAccountAmount
            {
                Description = "سند افتتاحیه",
                RiyalBed = openingEntry.RiyalBes,
                RiyalBes = openingEntry.RiyalBed,
                WeightBed = openingEntry.WeightBes,
                WeightBes = openingEntry.WeightBed,
            };
            var openingEntryInvoiceRow = new InvoiceRow
            {
                Description = "سند افتتاحیه",
                GeneralAccountAmount = openingEntryGeneralAccountAmount
            };
            var openingEntryInvoice = new Invoice
            {
                CounterpartyId = openingEntryCounterparty.Id,
                Number = "2",
                InvoiceRows = new List<InvoiceRow>()
            };
            openingEntryInvoice.InvoiceRows.Add(invoiceRow);
            var invoice = new Invoice
            {
                CounterpartyId = openingEntry.counterpartyId,
                Number = "1",
                InvoiceRows = new List<InvoiceRow>()
            };
            invoice.InvoiceRows.Add(openingEntryInvoiceRow);
            var document = new Document()
            {
                Date = DateTime.Now,
                Description = "سند افتتاحیه",
                Invoices = new List<Invoice>()
            };
            document.Invoices.Add(invoice);
            document.Invoices.Add(openingEntryInvoice);
            await _documentRepository.AddAsync(document);
            return new ResultDto
            {
                Success = true,
                Message = "Document added."
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("خطا در ثبت سند افتتاحیه: " + ex.Message, ex);
        }
    }
    
    public async Task<ResultDto> AddOrderEntry(DocumentItemListDto documentItemList)
    {
        var groupedLists = documentItemList.DocumentItems
            .GroupBy(item => item.CounterpartySideTwoId);
        var document = new Document();
        foreach (var group in groupedLists)
        {
            var invoice = new Invoice();
            foreach (var item in group)
            {
                switch (item.Type)
                {
                    case DocumentItemType.ORDER:
                        var generalAccountAmount = new GeneralAccountAmount
                        {
                            Description = item.Description,
                            RiyalBed = item.RiyalBed,
                            RiyalBes = item.RiyalBes,
                            WeightBed = item.WeightBed,
                            WeightBes = item.WeightBes,
                        };
                        var invoiceRow = new InvoiceRow
                        {
                            Description = item.Description,
                            GeneralAccountAmount = generalAccountAmount
                        };
                        invoice.InvoiceRows.Add(invoiceRow);
                        break;
                }
            }
            document.Invoices.Add(invoice);
        }
    }
}