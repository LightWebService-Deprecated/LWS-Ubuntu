using k8s.Models;
using LWSSandboxService.Model;
using LWSSandboxService.Model.Request;
using LWSSandboxService.Repository;

namespace LWSSandboxService.Service;

public class UbuntuContainerService
{
    private readonly KubernetesRepository _kubernetesRepository;

    public UbuntuContainerService(KubernetesRepository kubernetesRepository)
    {
        _kubernetesRepository = kubernetesRepository;
    }

    public V1Container UbuntuContainerDefinition => new()
    {
        Image = "kangdroid/multiarch-sshd",
        Name = $"ubuntu-sshd-kdr-{Guid.NewGuid().ToString()}",
        Ports = new List<V1ContainerPort>
        {
            new(containerPort: 22, protocol: "TCP")
        }
    };

    public V1Deployment UbuntuDeployment(string deploymentName)
    {
        var matchLabel = new Dictionary<string, string>
        {
            ["matchname"] = Ulid.NewUlid().ToString().ToLower()
        };

        return new V1Deployment()
        {
            Metadata = new V1ObjectMeta
            {
                Name = deploymentName
            },
            Spec = new V1DeploymentSpec
            {
                Replicas = 1,
                Selector = new V1LabelSelector
                {
                    MatchLabels = matchLabel
                },
                Template = new V1PodTemplateSpec
                {
                    Metadata = new V1ObjectMeta
                    {
                        Labels = matchLabel
                    },
                    Spec = new V1PodSpec
                    {
                        Containers = new List<V1Container> {UbuntuContainerDefinition}
                    }
                }
            }
        };
    }

    private V1Service UbuntuService(V1Deployment deployment) => new()
    {
        ApiVersion = "v1",
        Kind = "Service",
        Metadata = new V1ObjectMeta
        {
            Name = Ulid.NewUlid().ToString().ToLower()
        },
        Spec = new V1ServiceSpec
        {
            Type = "NodePort",
            Ports = new List<V1ServicePort>
            {
                new()
                {
                    Port = 30020,
                    NodePort = 22
                }
            },
            Selector = new Dictionary<string, string>
            {
                ["name"] = deployment.Metadata.Name
            }
        }
    };

    public async Task<UbuntuDeployment> CreateUbuntuDeploymentAsync(CreateUbuntuServiceRequest request, string userId)
    {
        var deploymentDefinition = UbuntuDeployment(request.DeploymentName);
        var serviceDefinition = UbuntuService(deploymentDefinition);

        // Do
        await _kubernetesRepository.CreateDeploymentAsync(deploymentDefinition, userId);
        await _kubernetesRepository.CreateServiceAsync(serviceDefinition, userId);

        return new UbuntuDeployment
        {
            AccountId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            DeploymentName = request.DeploymentName,
            SshPort = 22
        };
    }
}