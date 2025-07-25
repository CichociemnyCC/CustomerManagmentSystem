public class ClientsListViewModel
{
    public List<Client> Clients { get; set; } = new();
    public string? Search { get; set; }
    public string? StatusFilter { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
