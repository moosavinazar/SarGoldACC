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
    private readonly IGeneralAccountAmountRepository _orderAmountRepository;
    private readonly IMapper _mapper;

    public DocumentService(
        AppDbContext dbContext,
        ICounterpartyService counterpartyService,
        IDocumentRepository documentRepository,
        IInvoiceRepository invoiceRepository,
        IInvoiceRowRepository invoiceRowRepository,
        IGeneralAccountAmountRepository orderAmountRepository,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _counterpartyService = counterpartyService;
        _documentRepository = documentRepository;
        _invoiceRepository = invoiceRepository;
        _invoiceRowRepository = invoiceRowRepository;
        _orderAmountRepository = orderAmountRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> AddCounterpartyOpeningEntry(OrderDto openingEntry)
    {
        var openingEntryCounterparty = await _counterpartyService.GetByIdAndBranchIdAsync(1, openingEntry.branchId);
        try
        {
            var orderAmount = new OrderAmount
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
                GeneralAccountAmount = orderAmount
            };
            var openingEntryGeneralAccountAmount = new OrderAmount
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
                Type = DocumentType.FINAL,
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
    
    public async Task<ResultDto> AddOrderEntry(DocumentItemListDto documentItemList, DocumentType type)
    {
        try
        {
            var document = new Document()
            {
                Date = DateTime.Now,
                Type = type,
                Description = documentItemList.Description,
                Invoices = new List<Invoice>()
            };
            var invoiceSideOne = new Invoice
            {
                CounterpartyId = documentItemList.CounterpartySideOneId,
                Number = "1",
                InvoiceRows = new List<InvoiceRow>()
            };
            foreach (var documentItem in documentItemList.DocumentItems)
            {
                switch (documentItem.Type)
                {
                    case DocumentItemType.ORDER:
                        var orderAmount = new OrderAmount
                        {
                            Description = documentItem.Description,
                            RiyalBed = documentItem.RiyalBed,
                            RiyalBes = documentItem.RiyalBes,
                            WeightBed = documentItem.WeightBed,
                            WeightBes = documentItem.WeightBes,
                        };
                        var invoiceRow = new InvoiceRow
                        {
                            Description = documentItem.Description,
                            GeneralAccountAmount = orderAmount
                        };
                        invoiceSideOne.InvoiceRows.Add(invoiceRow);
                        break;
                }
            }
            document.Invoices.Add(invoiceSideOne);
            
            var groupedLists = documentItemList.DocumentItems
                .GroupBy(item => item.CounterpartySideTwoId);
            foreach (var group in groupedLists)
            {
                var invoice = new Invoice
                {
                    CounterpartyId = group.Key,
                    Number = "2",
                    InvoiceRows = new List<InvoiceRow>()
                };
                foreach (var item in group)
                {
                    switch (item.Type)
                    {
                        case DocumentItemType.ORDER:
                            var orderAmount = new OrderAmount
                            {
                                Description = item.Description,
                                RiyalBed = item.RiyalBes,
                                RiyalBes = item.RiyalBed,
                                WeightBed = item.WeightBes,
                                WeightBes = item.WeightBed,
                            };
                            var invoiceRow = new InvoiceRow
                            {
                                Description = item.Description,
                                GeneralAccountAmount = orderAmount
                            }; 
                            invoice.InvoiceRows.Add(invoiceRow);
                            break;
                    }
                }
                document.Invoices.Add(invoice);
            }
            await _documentRepository.AddAsync(document);
            return new ResultDto
            {
                Success = true,
                Message = "Document added."
            };
        } 
        catch (Exception ex)
        {
            throw new Exception("خطا در ثبت سند: " + ex.Message, ex);
        }
    }
}