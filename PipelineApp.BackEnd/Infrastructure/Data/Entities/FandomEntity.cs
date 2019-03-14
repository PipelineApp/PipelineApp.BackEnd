// <copyright file="FandomEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using Interfaces.Data;

    /// <summary>
    /// Data-layer representation of a fandom.
    /// </summary>
    /// <seealso cref="IEntity" />
    public class FandomEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the fandom's name.
        /// </summary>
        public string Name { get; set; }
    }
}
