namespace LWSSandboxService.Model.Request;

public class CreateUbuntuServiceRequest
{
    public string DeploymentName { get; set; }
    public int SshOverridePort { get; set; }
}