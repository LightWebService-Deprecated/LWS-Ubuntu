using System.Diagnostics.CodeAnalysis;
using LWSSandboxService.Model;

namespace LWSSandboxService.Service;

public interface IAuthorizationService
{
    Task<AccessToken?> AuthorizeAsync(string accessToken);
}

[ExcludeFromCodeCoverage]
public class AuthorizationService : IAuthorizationService
{
    private readonly HttpClient _httpClient;

    public AuthorizationService(IConfiguration configuration)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration.GetConnectionString("LWSGatewayHost"))
        };
    }

    public async Task<AccessToken?> AuthorizeAsync(string accessToken)
    {
        // Add Header
        _httpClient.DefaultRequestHeaders.Add("X-LWS-AUTH", accessToken);

        // Do Request
        var response = await _httpClient.GetAsync("/auth");

        // Clear Header
        _httpClient.DefaultRequestHeaders.Clear();

        // Check Response
        if (!response.IsSuccessStatusCode) return null;

        return await response.Content.ReadFromJsonAsync<AccessToken>();
    }
}