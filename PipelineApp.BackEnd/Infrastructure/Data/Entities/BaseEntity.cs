// <copyright file="BaseEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System;
    using Interfaces.Data;

    /// <inheritdoc />
    public abstract class BaseEntity : IEntity
    {
        /// <inheritdoc />
        public Guid Id { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntity"/> class.
        /// </summary>
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
