// <copyright file="FandomRepository.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Neo4j.Driver.V1;

    /// <summary>
    /// Repository class for data handling relating to fandoms.
    /// </summary>
    public class FandomRepository : BaseRepository<FandomEntity>, IRepository<FandomEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FandomRepository"/> class.
        /// </summary>
        /// <param name="driver">The graph db driver.</param>
        public FandomRepository(IDriver driver)
            : base(driver)
        {
        }

        /// <inheritdoc />
        public async Task<FandomEntity> Create(FandomEntity data)
        {
            var id = Guid.NewGuid();
            var query = $"CREATE (v:{VertexTypes.FANDOM} {{ id: '{id}', name: '{data.Name}' }}) RETURN v";
            return await LoadQuerySingle(query);
        }

        /// <inheritdoc />
        public int Count(string userId)
        {
            return CountAll(VertexTypes.FANDOM);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<FandomEntity>> GetAll(string userId)
        {
            var query = $"MATCH (v:{VertexTypes.FANDOM}) RETURN v";
            return await LoadQuery(query);
        }

        /// <inheritdoc />
        public Task<FandomEntity> GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
