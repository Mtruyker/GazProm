using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GasServiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GasServiceApp.Services;

public class GasService
{
    private readonly GasDbContext _context;

    public GasService(GasDbContext context)
    {
        _context = context;
    }

    public Task<List<Client>> GetClientsAsync()
        => _context.Clients.AsNoTracking().OrderBy(c => c.Id).ToListAsync();

    public Task<List<ClientAddress>> GetAddressesAsync()
        => _context.ClientAddresses.AsNoTracking().OrderBy(a => a.Id).ToListAsync();

    public Task<List<Equipment>> GetEquipmentAsync()
        => _context.Equipment.AsNoTracking().OrderBy(e => e.Id).ToListAsync();

    public Task<List<Master>> GetMastersAsync()
        => _context.Masters.AsNoTracking().OrderBy(m => m.Id).ToListAsync();

    public Task<List<ServiceRequest>> GetRequestsAsync()
        => _context.ServiceRequests.AsNoTracking()
            .OrderByDescending(r => r.RequestDate)
            .ThenBy(r => r.DeadlineDate)
            .ToListAsync();

    public Task<List<WorkRecord>> GetWorkRecordsAsync()
        => _context.WorkRecords.AsNoTracking().OrderByDescending(w => w.WorkDate).ToListAsync();

    public Task<List<InspectionResult>> GetInspectionResultsAsync()
        => _context.InspectionResults.AsNoTracking().OrderByDescending(i => i.InspectionDate).ToListAsync();

    public async Task<List<CompletedWorkReportItem>> GetCompletedWorkReportAsync()
    {
        var query =
            from request in _context.ServiceRequests.AsNoTracking()
            join client in _context.Clients.AsNoTracking() on request.ClientId equals client.Id
            join address in _context.ClientAddresses.AsNoTracking() on request.AddressId equals address.Id
            join equipment in _context.Equipment.AsNoTracking() on request.EquipmentId equals equipment.Id
            join work in _context.WorkRecords.AsNoTracking() on request.Id equals work.ServiceRequestId
            join master in _context.Masters.AsNoTracking() on work.MasterId equals master.Id
            where request.CompletionDate != null
            orderby request.CompletionDate descending
            select new CompletedWorkReportItem
            {
                RequestId = request.Id,
                CompletionDate = request.CompletionDate!.Value,
                Client = client.FullName,
                Address = address.FullAddress,
                Equipment = equipment.Type + " " + equipment.Manufacturer + " " + equipment.Model,
                Master = master.FullName,
                WorkType = work.WorkType,
                Cost = work.Cost,
                Result = work.Result
            };

        return await query.ToListAsync();
    }

    public async Task<List<OverdueRequestReportItem>> GetOverdueRequestsAsync()
    {
        var today = DateTime.Today;

        var query =
            from request in _context.ServiceRequests.AsNoTracking()
            join client in _context.Clients.AsNoTracking() on request.ClientId equals client.Id
            join address in _context.ClientAddresses.AsNoTracking() on request.AddressId equals address.Id
            join master in _context.Masters.AsNoTracking() on request.MasterId equals master.Id into masters
            from assignedMaster in masters.DefaultIfEmpty()
            where request.CompletionDate == null && request.DeadlineDate < today
            orderby request.DeadlineDate
            select new OverdueRequestReportItem
            {
                RequestId = request.Id,
                RequestDate = request.RequestDate,
                DeadlineDate = request.DeadlineDate,
                Client = client.FullName,
                Address = address.FullAddress,
                Master = assignedMaster == null ? "\u041d\u0435 \u043d\u0430\u0437\u043d\u0430\u0447\u0435\u043d" : assignedMaster.FullName,
                Status = request.Status,
                Description = request.Description
            };

        return await query.ToListAsync();
    }

    public async Task AddClientAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
    }

    public async Task AddAddressAsync(ClientAddress address)
    {
        _context.ClientAddresses.Add(address);
        await _context.SaveChangesAsync();
    }

    public async Task AddEquipmentAsync(Equipment equipment)
    {
        _context.Equipment.Add(equipment);
        await _context.SaveChangesAsync();
    }

    public async Task AddMasterAsync(Master master)
    {
        _context.Masters.Add(master);
        await _context.SaveChangesAsync();
    }

    public async Task AddRequestAsync(ServiceRequest request)
    {
        _context.ServiceRequests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task AddWorkRecordAsync(WorkRecord workRecord)
    {
        _context.WorkRecords.Add(workRecord);
        await _context.SaveChangesAsync();
    }

    public async Task AddInspectionResultAsync(InspectionResult inspectionResult)
    {
        _context.InspectionResults.Add(inspectionResult);
        await _context.SaveChangesAsync();
    }
}
