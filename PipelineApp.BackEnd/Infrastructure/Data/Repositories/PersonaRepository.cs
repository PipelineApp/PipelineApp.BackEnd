// <copyright file="PersonaRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces.Repositories;
    using Neo4j.Driver.V1;
    using Providers;

    /// <summary>
    /// Repository class for data handling relating to personas.
    /// </summary>
    public class PersonaRepository : BaseRepository<PersonaEntity>, IPersonaRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaRepository"/> class.
        /// </summary>
        /// <param name="driver">The graph db driver.</param>
        public PersonaRepository(IDriver driver)
            : base(driver)
        {
        }

        /// <inheritdoc />
        public async Task<PersonaEntity> Create(PersonaEntity data)
        {
            var id = Guid.NewGuid();
            var query = $@"
                    MATCH(u:{VertexTypes.USER})
                    WHERE u.id = '{data.UserId}'
                    CREATE (v:{VertexTypes.PERSONA} 
                        {{ 
                            id: '{id}', 
                            slug: '{data.Slug}',
                            personaname: '{data.PersonaName}',
                            description: '{data.Description}'
                        }})<-[:{EdgeTypes.MANAGES}]-(u)
                    RETURN v";
            return await LoadQuerySingle(query);
        }

        /// <inheritdoc />
        public int Count(string userId)
        {
            var query = $@"MATCH (user:{VertexTypes.USER} {{id:'{userId}'}})-[:Manages]-(v) RETURN count(*)";
            var data = Session.Run(query);
            return data.Peek().GetOrDefault("count(*)", 0);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PersonaEntity>> GetAll(string userId)
        {
            var query = $"MATCH (user:{VertexTypes.USER} {{id:'{userId}'}} )-[:{EdgeTypes.MANAGES}]-(v) RETURN v";
            return await LoadQuery(query);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<PersonaEntity>> GetBySlug(string personaSlug)
        {
            var query = $@"MATCH (v:{VertexTypes.PERSONA})
                        WHERE v.slug = '{personaSlug}' RETURN v";
            return await LoadQuery(query);
        }
    }
}
