// <copyright file="FandomRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using Entities;
    using Interfaces.Repositories;
    using Neo4jClient;

    /// <inheritdoc />
    public class FandomRepository : BaseRepository<FandomEntity>, IFandomRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FandomRepository"/> class.
        /// </summary>
        /// <param name="graphClient">The graph client.</param>
        public FandomRepository(GraphClient graphClient)
            : base(graphClient)
        {
        }
    }
}
