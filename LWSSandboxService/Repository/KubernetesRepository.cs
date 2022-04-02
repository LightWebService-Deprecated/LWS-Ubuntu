using k8s;
using k8s.Models;

namespace LWSSandboxService.Repository;

public class KubernetesRepository
{
    private readonly IKubernetes _kubernetesClient;

    public KubernetesRepository(IConfiguration configuration)
    {
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(configuration["KubePath"]);
        _kubernetesClient = new Kubernetes(config);
    }

    public async Task CreateDeploymentAsync(V1Deployment deploymentDefinition, string @namespace)
    {
        await _kubernetesClient.CreateNamespacedDeploymentAsync(deploymentDefinition, @namespace);
    }

    public async Task CreateServiceAsync(V1Service serviceDefinition, string @namespace)
    {
        await _kubernetesClient.CreateNamespacedServiceAsync(serviceDefinition, @namespace);
    }

    public async Task<bool> CheckDeploymentExists(string @namespace, string deploymentName)
    {
        var deployment = await _kubernetesClient.ListNamespacedDeploymentAsync(@namespace);

        return deployment?.Items.Any(a => a.Metadata.Name == deploymentName) == true;
    }
}