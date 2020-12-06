using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using Xunit;

namespace TestUtils
{
    public class DockerSetup : IAsyncLifetime
    {
        private readonly DockerClient _dockerClient;

        private string _containerId;
        private readonly string _containerImageUri;
        private IDictionary<string, EmptyStruct> _exposedPorts = new Dictionary<string, EmptyStruct>();
        private IDictionary<string, IList<PortBinding>> _portBindings = new Dictionary<string, IList<PortBinding>>();
        private IList<string> _enviromentVarables;
        public DockerSetup(string containerImageUri, int[] exposedPorts, 
            Tuple<int, int>[] portBindings, IList<string> enviromentVarables)
        {
            _dockerClient = new DockerClientConfiguration(new Uri(DockerApiUri())).CreateClient();
            _containerImageUri = containerImageUri;
            
            if (exposedPorts != null)
            {
                foreach(int port in exposedPorts)
                {
                    _exposedPorts.Add(port.ToString(), default(EmptyStruct));
                }

            }

            if(portBindings != null)
            {
                foreach(var tuple in portBindings)
                {
                    _portBindings.Add(tuple.Item1.ToString(), new List<PortBinding> { new PortBinding { HostPort = tuple.Item2.ToString() } });
                }
            }
            _enviromentVarables = enviromentVarables;
        }

        public async Task InitializeAsync()
        {
            await PullImage();
            await StartContainer();
        }

        private string DockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (isWindows)
            {
                return "npipe://./pipe/docker_engine";
            }

            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isLinux)
            {
                return "unix:/var/run/docker.sock";
            }

            throw new Exception(
                "Was unable to determine what OS this is running on, does not appear to be Windows or Linux!?");
        }

        private async Task PullImage()
        {
            await _dockerClient.Images
                .CreateImageAsync(new ImagesCreateParameters
                {
                    FromImage = _containerImageUri,
                    Tag = "latest"
                },
                    new AuthConfig(),
                    new Progress<JSONMessage>());
        }

        private async Task StartContainer()
        {
            var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
            {
                Image = _containerImageUri,
                ExposedPorts = _exposedPorts,
                HostConfig = new HostConfig
                {
                    PortBindings = _portBindings,
                    PublishAllPorts = true,

                },
                Env = _enviromentVarables
            });

            _containerId = response.ID;

            await _dockerClient.Containers.StartContainerAsync(_containerId, null);
        }

        public async Task DisposeAsync()
        {
            if (_containerId != null)
            {
                await _dockerClient.Containers.KillContainerAsync(_containerId, new ContainerKillParameters());
            }
        }
    }
}