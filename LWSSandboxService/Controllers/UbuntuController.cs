using System.Net;
using LWSSandboxService.Attribute;
using LWSSandboxService.Model;
using LWSSandboxService.Model.Request;
using LWSSandboxService.Model.Response;
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

        if (createRequest.SshOverridePort <= 30000 || createRequest.SshOverridePort >= 32767)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Port should be in range between 30000 ~ 32767!",
                ErrorPath = HttpContext.Request.Path,
                StatusCodes = StatusCodes.Status400BadRequest
            });
        }

        if (await _ubuntuContainerService.CheckDeploymentExists(accountId, createRequest.DeploymentName))
        {
            return BadRequest(new ErrorResponse
            {
                Message = $"Deployment Name {createRequest.DeploymentName} Already Exists",
                ErrorPath = HttpContext.Request.Path,
                StatusCodes = StatusCodes.Status400BadRequest
            });
        }

        var response = await _ubuntuContainerService.CreateUbuntuDeploymentAsync(createRequest, accountId);
        return Ok(response);
    }
}