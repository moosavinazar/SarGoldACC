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
    private readonly IOrderAmountRepository _orderAmountRepository;
    private readonly IMeltedRepository _meltedRepository;
    private readonly IMapper _mapper;

    public DocumentService(
        AppDbContext dbContext,
        ICounterpartyService counterpartyService,
        IDocumentRepository documentRepository,
        IInvoiceRepository invoiceRepository,
        IInvoiceRowRepository invoiceRowRepository,
        IOrderAmountRepository orderAmountRepository,
        IMeltedRepository meltedRepository,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _counterpartyService = counterpartyService;
        _documentRepository = documentRepository;
        _invoiceRepository = invoiceRepository;
        _invoiceRowRepository = invoiceRowRepository;
        _orderAmountRepository = orderAmountRepository;
        _meltedRepository = meltedRepository;
        _mapper = mapper;
    }

    public async Task<ResultDto> AddCounterpartyOpeningEntry(OrderDto openingEntry)
    {
        var openingEntryCounterparty = await _counterpartyService.GetByIdAndBranchIdAsync(1, openingEntry.branchId);
        try
        {
            InvoiceRowAccType typeRiyal;
            InvoiceRowAccType typeWeight;
            if (openingEntry.RiyalBed > 0)
            {
                typeRiyal = InvoiceRowAccType.BED;
            }
            else
            {
                typeRiyal = InvoiceRowAccType.BES;
            }
            if (openingEntry.WeightBed > 0)
            {
                typeWeight = InvoiceRowAccType.BED;
            }
            else
            {
                typeWeight = InvoiceRowAccType.BES;
            }
            var orderAmountRiyal = new OrderAmount
            {
                Description = "سند افتتاحیه",
                Riyal = openingEntry.RiyalBed > 0 ? openingEntry.RiyalBed : openingEntry.RiyalBes,
                InvoiceRows = new List<InvoiceRow>()
            };
            var orderAmountWeight = new OrderAmount
            {
                Description = "سند افتتاحیه",
                Weight = openingEntry.WeightBed > 0 ? openingEntry.WeightBed : openingEntry.WeightBes,
                InvoiceRows = new List<InvoiceRow>()
            };
            var invoiceRowRiyal = new InvoiceRow
            {
                AccType = typeRiyal,
                Description = "سند افتتاحیه",
                OrderAmount = orderAmountRiyal,
            };
            var invoiceRowWeight = new InvoiceRow
            {
                AccType = typeWeight,
                Description = "سند افتتاحیه",
                OrderAmount = orderAmountWeight
            };
            var openingEntryInvoice = new Invoice
            {
                CounterpartyId = openingEntryCounterparty.Id,
                Number = "2",
                InvoiceRows = new List<InvoiceRow>()
            };
            openingEntryInvoice.InvoiceRows.Add(invoiceRowRiyal);
            openingEntryInvoice.InvoiceRows.Add(invoiceRowWeight);
            
            var openingEntryInvoiceRowRiyal = new InvoiceRow
            {
                AccType = typeRiyal == InvoiceRowAccType.BED ? InvoiceRowAccType.BES : InvoiceRowAccType.BED,
                Description = "سند افتتاحیه",
                OrderAmount = orderAmountRiyal,
            };
            var openingEntryInvoiceRowWeight = new InvoiceRow
            {
                AccType = typeWeight == InvoiceRowAccType.BED ? InvoiceRowAccType.BES : InvoiceRowAccType.BED,
                Description = "سند افتتاحیه",
                OrderAmount = orderAmountWeight
            };
            var invoice = new Invoice
            {
                CounterpartyId = openingEntry.counterpartyId,
                Number = "1",
                InvoiceRows = new List<InvoiceRow>()
            };
            invoice.InvoiceRows.Add(openingEntryInvoiceRowRiyal);
            invoice.InvoiceRows.Add(openingEntryInvoiceRowWeight);
            
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
                        {
                            if (item.RiyalBed > 0 || item.RiyalBes > 0)
                            {
                                InvoiceRowAccType typeRiyal;
                                if (item.RiyalBed > 0)
                                {
                                    typeRiyal = InvoiceRowAccType.BED;
                                }
                                else
                                {
                                    typeRiyal = InvoiceRowAccType.BES;
                                }

                                var orderAmountRiyal = new OrderAmount
                                {
                                    Description = item.Description,
                                    Riyal = item.RiyalBed > 0 ? item.RiyalBed : item.RiyalBes,
                                    InvoiceRows = new List<InvoiceRow>()
                                };
                                var invoiceRowRiyalSideOne = new InvoiceRow
                                {
                                    AccType = typeRiyal,
                                    Description = item.Description,
                                    OrderAmount = orderAmountRiyal,
                                };
                                var invoiceRowRiyal = new InvoiceRow
                                {
                                    AccType = typeRiyal == InvoiceRowAccType.BED
                                        ? InvoiceRowAccType.BES
                                        : InvoiceRowAccType.BED,
                                    Description = item.Description,
                                    OrderAmount = orderAmountRiyal,
                                };
                                invoice.InvoiceRows.Add(invoiceRowRiyal);
                                invoiceSideOne.InvoiceRows.Add(invoiceRowRiyalSideOne);
                            }

                            if (item.WeightBed > 0 || item.WeightBes > 0)
                            {
                                InvoiceRowAccType typeWeight;
                                if (item.WeightBed > 0)
                                {
                                    typeWeight = InvoiceRowAccType.BED;
                                }
                                else
                                {
                                    typeWeight = InvoiceRowAccType.BES;
                                }

                                var orderAmountWeight = new OrderAmount
                                {
                                    Description = item.Description,
                                    Weight = item.WeightBed > 0 ? item.WeightBed : item.WeightBes,
                                    InvoiceRows = new List<InvoiceRow>()
                                };
                                var invoiceRowWeightSideOne = new InvoiceRow
                                {
                                    AccType = typeWeight,
                                    Description = item.Description,
                                    OrderAmount = orderAmountWeight,
                                };
                                var invoiceRowWeight = new InvoiceRow
                                {
                                    AccType = typeWeight == InvoiceRowAccType.BED
                                        ? InvoiceRowAccType.BES
                                        : InvoiceRowAccType.BED,
                                    Description = item.Description,
                                    OrderAmount = orderAmountWeight,
                                };
                                invoice.InvoiceRows.Add(invoiceRowWeight);
                                invoiceSideOne.InvoiceRows.Add(invoiceRowWeightSideOne);
                            }
                            document.Invoices.Add(invoice);
                            break;
                        }
                        case DocumentItemType.MELTED:
                        {
                            if (item.WeightBes > 0)
                            {
                                var melted =
                                    await _meltedRepository.GetByAngAndLaboratoryIdAsync(item.Ang, item.LaboratoryId) ??
                                    new Melted
                                    {
                                        Ang = item.Ang,
                                        Ayar = item.Ayar,
                                        Certain = item.Certain,
                                        LaboratoryId = item.LaboratoryId,
                                        SubMelteds = new List<SubMelted>()
                                    };
                                var subMelted = new SubMelted
                                {
                                    Weight = item.WeightBes,
                                    BoxId = item.BoxId,
                                    Melted = melted,
                                    InvoiceRows = new List<InvoiceRow>()
                                };
                                var invoiceRow = new InvoiceRow
                                {
                                    AccType = InvoiceRowAccType.BES,
                                    Description = item.Description,
                                    SubMelted = subMelted
                                };
                                invoiceSideOne.InvoiceRows.Add(invoiceRow);
                            }
                            break;
                        }
                    }
                }
            }
            document.Invoices.Add(invoiceSideOne);
            foreach (var invoice in document.Invoices)
            {
                Console.WriteLine(invoice.CounterpartyId);
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