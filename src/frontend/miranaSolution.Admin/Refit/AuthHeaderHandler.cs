using System.Net.Http.Headers;

namespace miranaSolution.Admin.Refit;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var accessToken = httpContext!.Session.GetString(Constants.AccessToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        return base.SendAsync(request, cancellationToken);
    }
}