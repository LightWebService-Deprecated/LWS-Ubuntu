using LWSSandboxService.Attribute;
using LWSSandboxService.Model;
using LWSSandboxService.Model.Request;
using LWSSandboxService.Service;
using Microsoft.AspNetCore.Mvc;

namespace LWSSandboxService.Controllers;

[ApiController]
[Route("/api/ubuntu")]
public class UbuntuController : ControllerBase
{
    private readonly UbuntuContainerService _ubuntuContainerService;

    public UbuntuController(UbuntuContainerService ubuntuContainerService)
    {
        _ubuntuContainerService = ubuntuContainerService;
    }

    [HttpPost]
    [LwsAuthorization(TargetAccountRole = AccountRole.User)]
    public async Task<IActionResult> CreateUbuntuServiceAsync(CreateUbuntuServiceRequest createRequest)
    {
        var accountId = HttpContext.Items["accountId"].ToString();

        var response = await _ubuntuContainerService.CreateUbuntuDeploymentAsync(createRequest, accountId);
        return Ok(response);
    }
}