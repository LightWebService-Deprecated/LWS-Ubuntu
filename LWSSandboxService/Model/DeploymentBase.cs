namespace LWSSandboxService.Model;

public abstract class DeploymentBase
{
    public abstract string Id { get; set; }
    public abstract DeploymentType DeploymentType { get; set; }
    public abstract string AccountId { get; set; }
    public abstract DateTimeOffset CreatedAt { get; set; }
}