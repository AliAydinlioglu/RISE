using Rise.Shared.Common;

namespace Rise.Shared.Contact;

public interface IContactService
{
    Task<Result<ContactResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default);
    
    Task<Result<ContactResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx = default);
    
    Task<Result<ContactResponse.Index>> GetByCategoryAsync(string categoryName, CancellationToken ctx = default);
    
    Task<Result<ContactResponse.Index>> GetByCampusAsync(string campusName, CancellationToken ctx = default);
    
    Task<Result<ContactResponse.Index>> GetStaticServicesAsync(CancellationToken ctx = default);
}

