using Rise.Shared.Common;

namespace Rise.Shared.Contact;

public interface IContactService
{
    Task<Result<ContactResponse.Index>> GetIndexAsync(QueryRequest.SkipTake request, CancellationToken ctx = default);
    
    Task<Result<ContactResponse.Detail>> GetDetailByIdAsync(int id, CancellationToken ctx = default);
}

