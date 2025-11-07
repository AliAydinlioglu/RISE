using Rise.Shared.Contact;

namespace Rise.Server.Endpoints.Contact;

public class Detail(IContactService service) : Endpoint<ContactRequest.Detail, Result<ContactResponse.Detail>>
{
    public override void Configure()
    {
        Get("/api/contact/{Id}");
        AllowAnonymous();
    }
    
    public override Task<Result<ContactResponse.Detail>> ExecuteAsync(ContactRequest.Detail req, CancellationToken ctx)
    {
        
        return service.GetDetailByIdAsync(req.Id, ctx);
    }
    
}