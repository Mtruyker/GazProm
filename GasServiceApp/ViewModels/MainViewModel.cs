using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using GasServiceApp.Models;
using GasServiceApp.Services;

namespace GasServiceApp.ViewModels;

public class MainViewModel : NotifyPropertyChangedBase
{
    private readonly GasService _service;
    private List<Client> _clients = [];
    private List<ClientAddress> _addresses = [];
    private List<Equipment> _equipment = [];
    private List<Master> _masters = [];
    private List<ServiceRequest> _requests = [];
    private List<WorkRecord> _workRecords = [];
    private List<InspectionResult> _inspectionResults = [];
    private List<CompletedWorkReportItem> _completedWorkReport = [];
    private List<OverdueRequestReportItem> _overdueRequests = [];
    private string _statusMessage = "\u0417\u0430\u0433\u0440\u0443\u0437\u043a\u0430 \u0434\u0430\u043d\u043d\u044b\u0445...";
    private string _clientsSummary = "\u0410\u0431\u043e\u043d\u0435\u043d\u0442\u043e\u0432: 0";
    private string _requestsSummary = "\u0417\u0430\u044f\u0432\u043e\u043a: 0";
    private string _overdueSummary = "\u041f\u0440\u043e\u0441\u0440\u043e\u0447\u0435\u043d\u043e: 0";

    public MainViewModel(GasService service)
    {
        _service = service;
        AddClientCommand = new RelayCommand(async _ => await AddClientAsync());
        AddAddressCommand = new RelayCommand(async _ => await AddAddressAsync());
        AddEquipmentCommand = new RelayCommand(async _ => await AddEquipmentAsync());
        AddMasterCommand = new RelayCommand(async _ => await AddMasterAsync());
        AddRequestCommand = new RelayCommand(async _ => await AddRequestAsync());
        AddWorkRecordCommand = new RelayCommand(async _ => await AddWorkRecordAsync());
        AddInspectionCommand = new RelayCommand(async _ => await AddInspectionAsync());
        _ = LoadDataAsync();
    }

    public ICommand AddClientCommand { get; }
    public ICommand AddAddressCommand { get; }
    public ICommand AddEquipmentCommand { get; }
    public ICommand AddMasterCommand { get; }
    public ICommand AddRequestCommand { get; }
    public ICommand AddWorkRecordCommand { get; }
    public ICommand AddInspectionCommand { get; }

    public List<Client> Clients { get => _clients; set => SetProperty(ref _clients, value); }
    public List<ClientAddress> Addresses { get => _addresses; set => SetProperty(ref _addresses, value); }
    public List<Equipment> Equipment { get => _equipment; set => SetProperty(ref _equipment, value); }
    public List<Master> Masters { get => _masters; set => SetProperty(ref _masters, value); }
    public List<ServiceRequest> Requests { get => _requests; set => SetProperty(ref _requests, value); }
    public List<WorkRecord> WorkRecords { get => _workRecords; set => SetProperty(ref _workRecords, value); }
    public List<InspectionResult> InspectionResults { get => _inspectionResults; set => SetProperty(ref _inspectionResults, value); }
    public List<CompletedWorkReportItem> CompletedWorkReport { get => _completedWorkReport; set => SetProperty(ref _completedWorkReport, value); }
    public List<OverdueRequestReportItem> OverdueRequests { get => _overdueRequests; set => SetProperty(ref _overdueRequests, value); }

    public string StatusMessage { get => _statusMessage; set => SetProperty(ref _statusMessage, value); }
    public string ClientsSummary { get => _clientsSummary; set => SetProperty(ref _clientsSummary, value); }
    public string RequestsSummary { get => _requestsSummary; set => SetProperty(ref _requestsSummary, value); }
    public string OverdueSummary { get => _overdueSummary; set => SetProperty(ref _overdueSummary, value); }

    public string NewClientAccountNumber { get; set; } = string.Empty;
    public string NewClientFullName { get; set; } = string.Empty;
    public string NewClientPhone { get; set; } = string.Empty;
    public string NewClientEmail { get; set; } = string.Empty;
    public string NewClientNotes { get; set; } = string.Empty;

    public string NewAddressClientId { get; set; } = string.Empty;
    public string NewAddressCity { get; set; } = "\u0421\u0430\u0440\u0430\u0442\u043e\u0432";
    public string NewAddressStreet { get; set; } = string.Empty;
    public string NewAddressHouse { get; set; } = string.Empty;
    public string NewAddressApartment { get; set; } = string.Empty;
    public string NewAddressEntrance { get; set; } = string.Empty;
    public string NewAddressFloor { get; set; } = string.Empty;

    public string NewEquipmentClientId { get; set; } = string.Empty;
    public string NewEquipmentAddressId { get; set; } = string.Empty;
    public string NewEquipmentSerialNumber { get; set; } = string.Empty;
    public string NewEquipmentType { get; set; } = "\u0413\u0430\u0437\u043e\u0432\u044b\u0439 \u043a\u043e\u0442\u0435\u043b";
    public string NewEquipmentManufacturer { get; set; } = string.Empty;
    public string NewEquipmentModel { get; set; } = string.Empty;
    public string NewEquipmentLocation { get; set; } = string.Empty;
    public string NewEquipmentStatus { get; set; } = "\u0412 \u044d\u043a\u0441\u043f\u043b\u0443\u0430\u0442\u0430\u0446\u0438\u0438";
    public string NewEquipmentInstallationDate { get; set; } = DateTime.Today.ToString("dd.MM.yyyy");
    public string NewEquipmentNextInspectionDate { get; set; } = DateTime.Today.AddYears(1).ToString("dd.MM.yyyy");

    public string NewMasterFullName { get; set; } = string.Empty;
    public string NewMasterPhone { get; set; } = string.Empty;
    public string NewMasterSpecialization { get; set; } = string.Empty;
    public string NewMasterQualification { get; set; } = string.Empty;

    public string NewRequestClientId { get; set; } = string.Empty;
    public string NewRequestAddressId { get; set; } = string.Empty;
    public string NewRequestEquipmentId { get; set; } = string.Empty;
    public string NewRequestMasterId { get; set; } = string.Empty;
    public string NewRequestType { get; set; } = "\u0420\u0435\u043c\u043e\u043d\u0442";
    public string NewRequestPriority { get; set; } = "\u041e\u0431\u044b\u0447\u043d\u044b\u0439";
    public string NewRequestStatus { get; set; } = "\u041e\u0442\u043a\u0440\u044b\u0442\u0430";
    public string NewRequestDeadlineDate { get; set; } = DateTime.Today.AddDays(3).ToString("dd.MM.yyyy");
    public string NewRequestDescription { get; set; } = string.Empty;

    public string NewWorkRequestId { get; set; } = string.Empty;
    public string NewWorkMasterId { get; set; } = string.Empty;
    public string NewWorkType { get; set; } = string.Empty;
    public string NewWorkMaterials { get; set; } = string.Empty;
    public string NewWorkCost { get; set; } = "0";
    public string NewWorkResult { get; set; } = string.Empty;

    public string NewInspectionRequestId { get; set; } = string.Empty;
    public string NewInspectionEquipmentId { get; set; } = string.Empty;
    public bool NewInspectionIsSafe { get; set; } = true;
    public string NewInspectionGasLeakCheck { get; set; } = "\u0423\u0442\u0435\u0447\u0435\u043a \u043d\u0435\u0442";
    public string NewInspectionVentilationCheck { get; set; } = "\u0412\u0435\u043d\u0442\u0438\u043b\u044f\u0446\u0438\u044f \u0438\u0441\u043f\u0440\u0430\u0432\u043d\u0430";
    public string NewInspectionAutomationCheck { get; set; } = "\u0410\u0432\u0442\u043e\u043c\u0430\u0442\u0438\u043a\u0430 \u0438\u0441\u043f\u0440\u0430\u0432\u043d\u0430";
    public string NewInspectionConclusion { get; set; } = string.Empty;
    public string NewInspectionRecommendations { get; set; } = string.Empty;

    private async Task LoadDataAsync()
    {
        try
        {
            Clients = await _service.GetClientsAsync();
            Addresses = await _service.GetAddressesAsync();
            Equipment = await _service.GetEquipmentAsync();
            Masters = await _service.GetMastersAsync();
            Requests = await _service.GetRequestsAsync();
            WorkRecords = await _service.GetWorkRecordsAsync();
            InspectionResults = await _service.GetInspectionResultsAsync();
            CompletedWorkReport = await _service.GetCompletedWorkReportAsync();
            OverdueRequests = await _service.GetOverdueRequestsAsync();

            ClientsSummary = "\u0410\u0431\u043e\u043d\u0435\u043d\u0442\u043e\u0432: " + Clients.Count;
            RequestsSummary = "\u0417\u0430\u044f\u0432\u043e\u043a: " + Requests.Count;
            OverdueSummary = "\u041f\u0440\u043e\u0441\u0440\u043e\u0447\u0435\u043d\u043e: " + OverdueRequests.Count;
            StatusMessage = "\u0414\u0430\u043d\u043d\u044b\u0435 \u0443\u0441\u043f\u0435\u0448\u043d\u043e \u0437\u0430\u0433\u0440\u0443\u0436\u0435\u043d\u044b \u0438\u0437 PostgreSQL.";
        }
        catch (Exception ex)
        {
            StatusMessage = "\u041e\u0448\u0438\u0431\u043a\u0430 \u043f\u043e\u0434\u043a\u043b\u044e\u0447\u0435\u043d\u0438\u044f \u043a PostgreSQL: " + ex.Message;
        }
    }

    private async Task AddClientAsync()
    {
        await ExecuteSaveAsync(async () =>
        {
            Require(NewClientAccountNumber, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u043b\u0438\u0446\u0435\u0432\u043e\u0439 \u0441\u0447\u0435\u0442.");
            Require(NewClientFullName, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0424\u0418\u041e \u0430\u0431\u043e\u043d\u0435\u043d\u0442\u0430.");
            await _service.AddClientAsync(new Client { AccountNumber = NewClientAccountNumber.Trim(), FullName = NewClientFullName.Trim(), Phone = NewClientPhone.Trim(), Email = NewClientEmail.Trim(), Notes = NewClientNotes.Trim() });
        }, "\u0410\u0431\u043e\u043d\u0435\u043d\u0442 \u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d.");
    }

    private async Task AddAddressAsync()
    {
        await ExecuteSaveAsync(async () =>
        {
            var clientId = ParseInt(NewAddressClientId, "ID \u0430\u0431\u043e\u043d\u0435\u043d\u0442\u0430");
            Require(NewAddressStreet, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0443\u043b\u0438\u0446\u0443.");
            Require(NewAddressHouse, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0434\u043e\u043c.");
            var fullAddress = "\u0433. " + NewAddressCity + ", \u0443\u043b. " + NewAddressStreet + ", \u0434. " + NewAddressHouse;
            if (!string.IsNullOrWhiteSpace(NewAddressApartment)) fullAddress += ", \u043a\u0432. " + NewAddressApartment;
            await _service.AddAddressAsync(new ClientAddress { ClientId = clientId, City = NewAddressCity.Trim(), Street = NewAddressStreet.Trim(), House = NewAddressHouse.Trim(), Apartment = NewAddressApartment.Trim(), Entrance = NewAddressEntrance.Trim(), Floor = NewAddressFloor.Trim(), FullAddress = fullAddress });
        }, "\u0410\u0434\u0440\u0435\u0441 \u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d.");
    }

    private async Task AddEquipmentAsync()
    {
        await ExecuteSaveAsync(async () =>
        {
            await _service.AddEquipmentAsync(new Equipment { ClientId = ParseInt(NewEquipmentClientId, "ID \u0430\u0431\u043e\u043d\u0435\u043d\u0442\u0430"), AddressId = ParseInt(NewEquipmentAddressId, "ID \u0430\u0434\u0440\u0435\u0441\u0430"), SerialNumber = RequiredValue(NewEquipmentSerialNumber, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0441\u0435\u0440\u0438\u0439\u043d\u044b\u0439 \u043d\u043e\u043c\u0435\u0440."), Type = RequiredValue(NewEquipmentType, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0442\u0438\u043f \u043e\u0431\u043e\u0440\u0443\u0434\u043e\u0432\u0430\u043d\u0438\u044f."), InstallationDate = ParseDate(NewEquipmentInstallationDate, "\u0414\u0430\u0442\u0430 \u0443\u0441\u0442\u0430\u043d\u043e\u0432\u043a\u0438"), NextInspectionDate = ParseOptionalDate(NewEquipmentNextInspectionDate, "\u0414\u0430\u0442\u0430 \u0441\u043b\u0435\u0434\u0443\u044e\u0449\u0435\u0439 \u043f\u0440\u043e\u0432\u0435\u0440\u043a\u0438"), Manufacturer = NewEquipmentManufacturer.Trim(), Model = NewEquipmentModel.Trim(), Location = NewEquipmentLocation.Trim(), Status = NewEquipmentStatus.Trim() });
        }, "\u041e\u0431\u043e\u0440\u0443\u0434\u043e\u0432\u0430\u043d\u0438\u0435 \u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d\u043e.");
    }

    private async Task AddMasterAsync()
    {
        await ExecuteSaveAsync(async () =>
        {
            await _service.AddMasterAsync(new Master { FullName = RequiredValue(NewMasterFullName, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0424\u0418\u041e \u043c\u0430\u0441\u0442\u0435\u0440\u0430."), Phone = NewMasterPhone.Trim(), Specialization = NewMasterSpecialization.Trim(), Qualification = NewMasterQualification.Trim(), IsActive = true });
        }, "\u041c\u0430\u0441\u0442\u0435\u0440 \u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d.");
    }

    private async Task AddRequestAsync()
    {
        await ExecuteSaveAsync(async () =>
        {
            int? masterId = string.IsNullOrWhiteSpace(NewRequestMasterId) ? null : ParseInt(NewRequestMasterId, "ID \u043c\u0430\u0441\u0442\u0435\u0440\u0430");
            await _service.AddRequestAsync(new ServiceRequest { ClientId = ParseInt(NewRequestClientId, "ID \u0430\u0431\u043e\u043d\u0435\u043d\u0442\u0430"), AddressId = ParseInt(NewRequestAddressId, "ID \u0430\u0434\u0440\u0435\u0441\u0430"), EquipmentId = ParseInt(NewRequestEquipmentId, "ID \u043e\u0431\u043e\u0440\u0443\u0434\u043e\u0432\u0430\u043d\u0438\u044f"), MasterId = masterId, RequestDate = DateTime.Now, DeadlineDate = ParseDate(NewRequestDeadlineDate, "\u0421\u0440\u043e\u043a \u0432\u044b\u043f\u043e\u043b\u043d\u0435\u043d\u0438\u044f"), RequestType = RequiredValue(NewRequestType, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0442\u0438\u043f \u0437\u0430\u044f\u0432\u043a\u0438."), Priority = NewRequestPriority.Trim(), Status = NewRequestStatus.Trim(), Description = NewRequestDescription.Trim() });
        }, "\u0417\u0430\u044f\u0432\u043a\u0430 \u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d\u0430.");
    }

    private async Task AddWorkRecordAsync()
    {
        await ExecuteSaveAsync(async () =>
        {
            await _service.AddWorkRecordAsync(new WorkRecord { ServiceRequestId = ParseInt(NewWorkRequestId, "ID \u0437\u0430\u044f\u0432\u043a\u0438"), MasterId = ParseInt(NewWorkMasterId, "ID \u043c\u0430\u0441\u0442\u0435\u0440\u0430"), WorkDate = DateTime.Now, WorkType = RequiredValue(NewWorkType, "\u0423\u043a\u0430\u0436\u0438\u0442\u0435 \u0432\u0438\u0434 \u0440\u0430\u0431\u043e\u0442\u044b."), MaterialsUsed = NewWorkMaterials.Trim(), Cost = ParseDecimal(NewWorkCost, "\u0421\u0442\u043e\u0438\u043c\u043e\u0441\u0442\u044c"), Result = NewWorkResult.Trim() });
        }, "\u0420\u0430\u0431\u043e\u0442\u0430 \u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d\u0430.");
    }

    private async Task AddInspectionAsync()
    {
        await ExecuteSaveAsync(async () =>
        {
            await _service.AddInspectionResultAsync(new InspectionResult { ServiceRequestId = ParseInt(NewInspectionRequestId, "ID \u0437\u0430\u044f\u0432\u043a\u0438"), EquipmentId = ParseInt(NewInspectionEquipmentId, "ID \u043e\u0431\u043e\u0440\u0443\u0434\u043e\u0432\u0430\u043d\u0438\u044f"), InspectionDate = DateTime.Now, IsSafe = NewInspectionIsSafe, GasLeakCheck = NewInspectionGasLeakCheck.Trim(), VentilationCheck = NewInspectionVentilationCheck.Trim(), AutomationCheck = NewInspectionAutomationCheck.Trim(), Conclusion = NewInspectionConclusion.Trim(), Recommendations = NewInspectionRecommendations.Trim() });
        }, "\u0420\u0435\u0437\u0443\u043b\u044c\u0442\u0430\u0442 \u043f\u0440\u043e\u0432\u0435\u0440\u043a\u0438 \u0434\u043e\u0431\u0430\u0432\u043b\u0435\u043d.");
    }

    private async Task ExecuteSaveAsync(Func<Task> action, string successMessage)
    {
        try { await action(); await LoadDataAsync(); StatusMessage = successMessage; }
        catch (Exception ex) { StatusMessage = "\u041e\u0448\u0438\u0431\u043a\u0430 \u0441\u043e\u0445\u0440\u0430\u043d\u0435\u043d\u0438\u044f: " + ex.Message; }
    }

    private static void Require(string value, string message) { if (string.IsNullOrWhiteSpace(value)) throw new InvalidOperationException(message); }
    private static string RequiredValue(string value, string message) { Require(value, message); return value.Trim(); }
    private static int ParseInt(string value, string fieldName) { if (!int.TryParse(value, out var result)) throw new InvalidOperationException(fieldName + ": \u0443\u043a\u0430\u0436\u0438\u0442\u0435 \u0446\u0435\u043b\u043e\u0435 \u0447\u0438\u0441\u043b\u043e."); return result; }
    private static DateTime ParseDate(string value, string fieldName) { if (!DateTime.TryParse(value, out var result)) throw new InvalidOperationException(fieldName + ": \u0443\u043a\u0430\u0436\u0438\u0442\u0435 \u0434\u0430\u0442\u0443, \u043d\u0430\u043f\u0440\u0438\u043c\u0435\u0440 25.05.2026."); return result; }
    private static DateTime? ParseOptionalDate(string value, string fieldName) { return string.IsNullOrWhiteSpace(value) ? null : ParseDate(value, fieldName); }
    private static decimal ParseDecimal(string value, string fieldName) { if (!decimal.TryParse(value, out var result)) throw new InvalidOperationException(fieldName + ": \u0443\u043a\u0430\u0436\u0438\u0442\u0435 \u0447\u0438\u0441\u043b\u043e."); return result; }
}
