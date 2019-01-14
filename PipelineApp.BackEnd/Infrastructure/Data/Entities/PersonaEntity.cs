// <copyright file="PersonaEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System.Collections.Generic;
    using Neo4j.Driver.V1;
    using Providers;

    /// <summary>
    /// Data-layer representation of a persona.
    /// </summary>
    public class PersonaEntity : GraphEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user that owns the persona.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the persona's unique URL slug.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the persona's name.
        /// </summary>
        public string PersonaName { get; set; }

        /// <summary>
        /// Gets or sets the persona's description.
        /// </summary>
        public string Description { get; set; }

        /// <inheritdoc />
        public override void LoadRecord(IRecord record)
        {
            var root = record.GetOrDefault("v", (INode)null);
            if (root == null)
            {
                return;
            }
            Id = root.GetOrDefault<string>("id", null);
            Slug = root.GetOrDefault<string>("slug", null);
            PersonaName = root.GetOrDefault<string>("personaname", null);
            Description = root.GetOrDefault<string>("description", null);
        }
    }
}
