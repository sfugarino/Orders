using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
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
        private ContainerConfiguration _containerConfiguration; 

        public DockerSetup(ContainerConfiguration containerConfiguration)
        {
            _dockerClient = new DockerClientConfiguration(new Uri(DockerApiUri()), null, new TimeSpan(0, 2, 0)).CreateClient();
            _containerConfiguration = containerConfiguration;
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
            if (_containerConfiguration == null || _containerConfiguration.CreateContainerParameters == null)
            {
                throw new ArgumentNullException("Image create parameters can not be null");
            }

            await _dockerClient.Images
                .CreateImageAsync(_containerConfiguration.ImagesCreateParameters,
                    new AuthConfig(),
                    new Progress<JSONMessage>());
        }

        private async Task StartContainer()
        {
            if(_containerConfiguration == null || _containerConfiguration.CreateContainerParameters == null)
            {
                throw new ArgumentNullException("Create container parameters can not be null");
            }

            var response = await _dockerClient.Containers.CreateContainerAsync(_containerConfiguration.CreateContainerParameters);

            _containerId = response.ID;

            bool success = await _dockerClient.Containers.StartContainerAsync(_containerId, null);

            // Give container enough time to start
            // this does not work
            // await _dockerClient.Containers.WaitContainerAsync(_containerId, cancellationToken.Token);
            await Task.Delay(10000);
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