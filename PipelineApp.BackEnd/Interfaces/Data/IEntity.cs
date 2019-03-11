// <copyright file="IEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Data
{
    using System;

    /// <summary>
    /// Base interface for all data-layer entities.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets or sets the entity's unique identifier.
        /// </summary>
        Guid Id { get; set; }
    }
}
