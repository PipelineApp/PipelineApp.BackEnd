// <copyright file="GraphEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using Neo4j.Driver.V1;

    /// <summary>
    /// Base class for all data-layer entity objects.
    /// </summary>
    public abstract class GraphEntity
    {
        /// <summary>
        /// Loads the properties of an <see cref="IRecord"/> into the data model.
        /// </summary>
        /// <param name="record">Graph DB record.</param>
        public abstract void LoadRecord(IRecord record);
    }
}
