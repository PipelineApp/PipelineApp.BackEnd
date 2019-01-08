﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using Entities;
    using Interfaces;
    using Neo4j.Driver.V1;

    public class FandomRepository : BaseRepository, IRepository<Fandom>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="driver"></param>
        public FandomRepository(IDriver driver)
            : base(driver)
        {
        }

        public async Task<Fandom> Create(Fandom data)
        {
            var id = Guid.NewGuid();
            var query = $"CREATE (v:{VertexTypes.FANDOM} {{ id: '{id}', name: '{data.Name}' }}) RETURN v";
            return await Session.WriteTransactionAsync(async tx =>
            {
                var cursor = await tx.RunAsync(query);
                await cursor.FetchAsync();
                var fandom = new Fandom();
                fandom.LoadRecord(cursor.Current);
                return fandom;
            });
        }

        public int Count()
        {
            return Count(VertexTypes.FANDOM);
        }

        public IEnumerable<Fandom> GetAll()
        {
            var query = $"MATCH (v:{VertexTypes.FANDOM}) RETURN v";
            var data = Session.Run(query);
            return data.Select(v =>
            {
                var entity = new Fandom();
                entity.LoadRecord(v);
                return entity;
            });
        }
    }
}