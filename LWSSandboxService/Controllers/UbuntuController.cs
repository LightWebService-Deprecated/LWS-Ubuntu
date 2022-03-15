using LWSSandboxService.Attribute;
using LWSSandboxService.Model;
using LWSSandboxService.Model.Request;
using LWSSandboxService.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;

namespace LWSSandboxService.Controllers;

[ApiController]
[Route("/api/ubuntu")]
public class UbuntuController : ControllerBase
{
    private readonly UbuntuContainerService _ubuntuContainerService;
    private readonly ILogger _logger;

    public UbuntuController(UbuntuContainerService ubuntuContainerService, ILogger<UbuntuController> logger)
    {
        _ubuntuContainerService = ubuntuContainerService;
        _logger = logger;
    }

    [HttpPost]
    [LwsAuthorization(TargetAccountRole = AccountRole.User)]
    public async Task<IActionResult> CreateUbuntuServiceAsync(CreateUbuntuServiceRequest createRequest)
    {
        var accountId = HttpContext.Items["accountId"].ToString();

        try
        {
            var response = await _ubuntuContainerService.CreateUbuntuDeploymentAsync(createRequest, accountId);
            return Ok(response);
        }
        catch (HttpOperationException exception)
        {
            _logger.LogCritical("Error Occurred while sending request: {message}", exception.Message);
            _logger.LogCritical("Stack: {message}", exception.Source);
            _logger.LogCritical("Response: {message}", exception.Response.ReasonPhrase);
            _logger.LogCritical("Response Content: {message}", exception.Response.Content);
        }

        return BadRequest();
    }
}