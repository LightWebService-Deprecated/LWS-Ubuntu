namespace LWSSandboxService.Model;

public class UbuntuDeployment
{
    public string AccountId { get; set; }
    public string DeploymentName { get; set; }
    public int SshPort { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}