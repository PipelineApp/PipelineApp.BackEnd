// <copyright file="PipelineRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using Entities;
    using Interfaces.Repositories;
    using Neo4jClient;

    /// <inheritdoc cref="IFandomRepository" />
    public class PipelineRepository : BaseRepository<PipelineEntity>, IPipelineRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineRepository"/> class.
        /// </summary>
        /// <param name="graphClient">The graph client.</param>
        public PipelineRepository(IGraphClient graphClient)
            : base(graphClient)
        {
        }
    }
}
