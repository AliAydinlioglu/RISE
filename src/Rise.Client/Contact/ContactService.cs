using Rise.Shared.Common;
using Rise.Shared.Contact;
using System.Net.Http.Json;

namespace Rise.Client.Contact;

public class ContactService(HttpClient httpClient) : IContactService
{
    public Task<Result<ContactResponse.Index>> GetByCampusAsync(string campusName, CancellationToken ctx = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ContactResponse.Index>> GetByCategoryAsync(string categoryName, CancellationToken ctx = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ContactResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx = default)
    {
        var result = httpClient.GetFromJsonAsync<Result<ContactResponse.Detail>>($"/api/contact/{id}", cancellationToken: ctx);
        return result!;
    }

    public Task<Result<ContactResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default)
    {
        var result = httpClient.GetFromJsonAsync<Result<ContactResponse.Index>>($"/api/contact?skip={request.Skip}&take={request.Take}", cancellationToken: ctx);
        return result!;
    }

    public Task<Result<ContactResponse.Index>> GetStaticServicesAsync(CancellationToken ctx = default)
    {
        throw new NotImplementedException();
    }
}
