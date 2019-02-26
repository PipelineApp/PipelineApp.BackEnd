// <copyright file="RefreshTokenRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System.Threading.Tasks;
    using Entities;
    using Interfaces.Repositories;
    using Neo4jClient;
    using Relationships;

    /// <inheritdoc />
    public class RefreshTokenRepository : BaseRepository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
        /// </summary>
        /// <param name="graphClient">The graph client.</param>
        public RefreshTokenRepository(GraphClient graphClient)
            : base(graphClient)
        {
        }

        /// <inheritdoc />
        public async Task SaveRefreshTokenForUser(RefreshTokenEntity refreshToken, UserEntity userEntity)
        {
            await GraphClient.Cypher
                .Match($"(user:{typeof(UserEntity).Name})")
                .Where((UserEntity user) => user.Id == userEntity.Id)
                .Create($"(token:{typeof(RefreshTokenEntity).Name} {{newToken}})<-[:{typeof(IsValidatedBy).Name}]-(user)")
                .WithParam("newToken", refreshToken)
                .ExecuteWithoutResultsAsync();
        }
    }
}
