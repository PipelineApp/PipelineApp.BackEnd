// <copyright file="PersonaRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces.Repositories;
    using Neo4jClient;
    using Relationships;

    /// <inheritdoc cref="IPersonaRepository" />
    public class PersonaRepository : BaseRepository<PersonaEntity>, IPersonaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaRepository"/> class.
        /// </summary>
        /// <param name="graphClient">The graph client.</param>
        public PersonaRepository(IGraphClient graphClient)
            : base(graphClient)
        {
        }

        public async Task<IEnumerable<PersonaEntity>> GetByUserIdAsync(string userId)
        {
            var result = await GraphClient.Cypher
                .Match($"(user:{typeof(UserEntity).Name})-[r:{typeof(HasPersona).Name}]->(persona:{typeof(PersonaEntity).Name})")
                .Where((UserEntity user) => user.Id.ToString() == userId)
                .Return(persona => persona.As<PersonaEntity>())
                .ResultsAsync;
            return result.ToList();
        }
    }
}
