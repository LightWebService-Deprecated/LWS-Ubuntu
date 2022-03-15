namespace LWSSandboxService.Model;

public class UbuntuDeployment : DeploymentBase
{
    public override string Id { get; set; }
    public override DeploymentType DeploymentType { get; set; }
    public override string AccountId { get; set; }
    public override DateTimeOffset CreatedAt { get; set; }
    public string DeploymentName { get; set; }
    public int SshPort { get; set; }
}