// <copyright file="FandomEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using Interfaces;
    using Neo4j.Driver.V1;
    using Providers;

    /// <summary>
    /// Data-layer representation of a fandom.
    /// </summary>
    /// <seealso cref="IEntity" />
    public class FandomEntity : GraphEntity
    {
        /// <summary>
        /// Gets or sets the fandom's name.
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc />
        public override void LoadRecord(IRecord record)
        {
            var root = record.GetOrDefault("v", (INode)null);
            if (root == null)
            {
                return;
            }
            Id = root.GetOrDefault<string>("id", null);
            Name = root.GetOrDefault<string>("name", null);
        }
    }
}
