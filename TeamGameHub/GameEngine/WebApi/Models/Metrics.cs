using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamGameHub.GameEngine.WebApi.Models
{
    /// <summary>
    /// This are price performance metrics about the cluster
    /// </summary>
    public class Metrics
    {
        /// <summary>
        /// Was there an error while calculating metrics?
        /// </summary>
        public bool MetricsError { get; set; }

        /// <summary>
        /// The total price.
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// The total amount of memory.
        /// </summary>
        public int TotalMemory { get; set; }

        /// <summary>
        /// The total amount of cores.
        /// </summary>
        public int TotalCores { get; set; }

        /// <summary>
        /// The list of all nodes.
        /// </summary>
        public Node[] Nodes { get; set; }
    }
}
