// <copyright file="GremlinConfig.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.Configuration
{
    /// <summary>
    /// Wrapper class for application settings related to graph database queries.
    /// </summary>
    public class GraphDbAppSettings
    {
        /// <summary>
        /// Gets or sets the graph database hostname.
        /// </summary>
        public string Hostname { get; set; }

        /// <summary>
        /// Gets or sets the graph database port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the graph database authentication key.
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// Gets or sets the graph database name.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the graph database collection ID.
        /// </summary>
        public string Collection { get; set; }
    }
}
