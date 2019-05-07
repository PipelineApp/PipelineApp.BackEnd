// <copyright file="BaseRelationship.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Relationships
{
    using System;

    /// <summary>
    /// Base class for all representations of relationships between nodes.
    /// </summary>
    public abstract class BaseRelationship
    {
        /// <summary>
        /// Gets or sets the ID of the relationship's source node.
        /// </summary>
        public Guid SourceId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the relationship's target node.
        /// </summary>
        public Guid TargetId { get; set; }
    }
}
