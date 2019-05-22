// <copyright file="VersionEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System;
    using Interfaces.Data;

    /// <summary>
    /// Data-layer representation of a version of a particular object.
    /// </summary>
    public class VersionEntity<TBase> : BaseEntity
        where TBase : IEntity
    {
        /// <summary>
        /// Gets or sets the date at which the version was created.
        /// </summary>
        public DateTime UtcDateTime { get; set; }
    }
}
