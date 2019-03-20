// <copyright file="PipelineRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Entities.EntityCollections;
    using Interfaces.Repositories;
    using Neo4jClient;
    using Relationships;

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

        /// <inheritdoc />
        public async Task<IEnumerable<PipelineEntityDataCollection>> GetByUserIdAsync(Guid? userId)
        {
            var result = await GraphClient.Cypher
                .Match($"(user:{typeof(UserEntity).Name})-[r:{typeof(Manages).Name}]->(pipeline:{typeof(PipelineEntity).Name})")
                .Where((UserEntity user) => user.Id == userId)
                .With("user, pipeline")
                .OptionalMatch($"(pipeline)-[t:{typeof(Tracks).Name}]->(fandom:{typeof(FandomEntity).Name})")
                .OptionalMatch($"(pipeline)-[t2:{typeof(Tracks).Name}]->(persona:{typeof(PersonaEntity).Name})")
                .Return((pipeline, fandom, persona) => new
                {
                    Pipeline = pipeline.As<PipelineEntity>(),
                    Fandoms = fandom.CollectAsDistinct<FandomEntity>(),
                    Personas = persona.CollectAsDistinct<PersonaEntity>()
                })
                .ResultsAsync;
            var collection = result.Select(p => new PipelineEntityDataCollection
            {
                Pipeline = p.Pipeline,
                Fandoms = p.Fandoms.ToList(),
                Personas = p.Personas.ToList()
            }).ToList();
            return collection;
        }
    }
}
