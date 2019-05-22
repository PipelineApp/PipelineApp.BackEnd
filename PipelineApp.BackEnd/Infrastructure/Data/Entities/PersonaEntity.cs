// <copyright file="PersonaEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    /// <summary>
    /// Data-layer representation of a persona.
    /// </summary>
    public class PersonaEntity : BaseEntity
    {
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
    }
}
