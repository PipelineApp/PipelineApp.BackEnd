// <copyright file="BaseEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System;
    using Interfaces.Data;

    /// <summary>
    /// Base class for all data-layer entities.
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity's unique identifier.
        /// </summary>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
