using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public class ContainerConfiguration
    {
        public ImagesCreateParameters ImagesCreateParameters { get; private set; }
        public CreateContainerParameters CreateContainerParameters { get; private set; }
        public string ContainerImageUri { get; private set; }
        public IList<string> EnviromentVarables { get; private set; }
        public string Tag { get; private set; }

        public ContainerConfiguration(string containerImageUri, string tag, int[] exposedPorts,
            Tuple<int, int>[] portBindings, IList<string> enviromentVarables)
        {
            this.ContainerImageUri = containerImageUri;
            this.EnviromentVarables = enviromentVarables;
            this.Tag = tag;
            BuildImagesCreateParameters();
            BuildCreateContainerParameters(exposedPorts, portBindings);
        }

        private void BuildImagesCreateParameters()
        {
            this.ImagesCreateParameters = new ImagesCreateParameters()
            {
                FromImage = ContainerImageUri,
                Tag = Tag
            };
        }

        private void BuildCreateContainerParameters(int[] exposedPorts, Tuple<int, int>[] portBindings)
        {
            CreateContainerParameters = new CreateContainerParameters
            {
                Image = ContainerImageUri,
                ExposedPorts = BuildExposedPorts(exposedPorts),
                HostConfig = new HostConfig
                {
                    PortBindings = BuildPortBindings(portBindings),
                    PublishAllPorts = true,

                },
                Env = EnviromentVarables
            };
        }

        private IDictionary<string, EmptyStruct> BuildExposedPorts(int[] exposedPorts)
        {
            IDictionary<string, EmptyStruct> ports = new Dictionary<string, EmptyStruct>();

            if (exposedPorts != null)
            {
                foreach (int port in exposedPorts)
                {
                    ports.Add(port.ToString(), default(EmptyStruct));
                }

            }

            return ports;
        }

        private IDictionary<string, IList<PortBinding>> BuildPortBindings(Tuple<int, int>[] portBindings)
        {
            IDictionary<string, IList<PortBinding>> bindings = new Dictionary<string, IList<PortBinding>>();

            if (portBindings != null)
            {
                foreach (var tuple in portBindings)
                {
                    bindings.Add(tuple.Item1.ToString(), new List<PortBinding> { new PortBinding { HostPort = tuple.Item2.ToString() } });
                }
            }

            return bindings;
        }
    }
}
