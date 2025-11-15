using Rise.Shared.Common;
using Rise.Shared.Contact;

namespace Rise.Server.Endpoints.Contact;

public class Index(IContactService service) : Endpoint<QueryRequest.SkipTake, Result<ContactResponse.Index>>
{
    public override void Configure()
    {
        Get("/api/contact");
        AllowAnonymous(); 
    }

    public override Task<Result<ContactResponse.Index>> ExecuteAsync(QueryRequest.SkipTake req, CancellationToken ct)
    {
        return service.GetIndexAsync(req, ct);
    }
}