using System.ComponentModel.DataAnnotations;

public class Client
{
    public int Id { get; set; }

    [Required]
    public string ClientName { get; set; } = string.Empty;

    [Required]
    public string Source { get; set; } = string.Empty;

    [Required]
    public string Services { get; set; } = string.Empty;

    [Required]
    public string Packages { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime? ServiceStartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? ServiceEndDate { get; set; }

    // Opcjonalne pola z automatycznym "Brak"
    private string _contractNumber;
    public string ContractNumber
    {
        get => string.IsNullOrWhiteSpace(_contractNumber) ? "Brak" : _contractNumber;
        set => _contractNumber = value;
    }

    private string _contactPerson;
    public string ContactPerson
    {
        get => string.IsNullOrWhiteSpace(_contactPerson) ? "Brak" : _contactPerson;
        set => _contactPerson = value;
    }

    private string _contactData;
    public string ContactData
    {
        get => string.IsNullOrWhiteSpace(_contactData) ? "Brak" : _contactData;
        set => _contactData = value;
    }

    private string _accessGrantedBy;
    public string AccessGrantedBy
    {
        get => string.IsNullOrWhiteSpace(_accessGrantedBy) ? "Brak" : _accessGrantedBy;
        set => _accessGrantedBy = value;
    }

    private string _metaAccount;
    public string MetaAccount
    {
        get => string.IsNullOrWhiteSpace(_metaAccount) ? "Brak" : _metaAccount;
        set => _metaAccount = value;
    }

    private string _metaAgreement;
    public string MetaAgreement
    {
        get => string.IsNullOrWhiteSpace(_metaAgreement) ? "Brak" : _metaAgreement;
        set => _metaAgreement = value;
    }

    private string _localData;
    public string LocalData
    {
        get => string.IsNullOrWhiteSpace(_localData) ? "Brak" : _localData;
        set => _localData = value;
    }

    private string _socialWebGoogleDrive;
    public string SocialWebGoogleDrive
    {
        get => string.IsNullOrWhiteSpace(_socialWebGoogleDrive) ? "Brak" : _socialWebGoogleDrive;
        set => _socialWebGoogleDrive = value;
    }

    private string _notes;
    public string Notes
    {
        get => string.IsNullOrWhiteSpace(_notes) ? "Brak" : _notes;
        set => _notes = value;
    }

    public string ServiceStatus { get; set; } = "Active";
    public bool IsArchived { get; set; } = false;
}
