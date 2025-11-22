using Rise.Shared.Common;
using Rise.Shared.Contact;
using System.Net.Http.Json;

namespace Rise.Client.Contact;

public class ContactService(HttpClient httpClient) : IContactService
{
    public Task<Result<ContactResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
    {
        var url = $"/api/contact?skip={request.Skip}&take={request.Take}";
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            url += $"&searchTerm={Uri.EscapeDataString(request.SearchTerm)}";
        if (!string.IsNullOrWhiteSpace(request.OrderBy))
            url += $"&orderBy={Uri.EscapeDataString(request.OrderBy)}&orderDescending={(request.OrderDescending ? "true" : "false")}";

        var result = httpClient.GetFromJsonAsync<Result<ContactResponse.Index>>(url, cancellationToken: ctx);
        return result!;
    }

    public Task<Result<ContactResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx = default)
    {
        var result = httpClient.GetFromJsonAsync<Result<ContactResponse.Detail>>($"/api/contact/{id}", cancellationToken: ctx);
        return result!;
    }
}