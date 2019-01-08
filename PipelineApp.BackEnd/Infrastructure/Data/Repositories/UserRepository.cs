namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Interfaces;
    using Neo4j.Driver.V1;

    public class UserRepository : BaseRepository, IRepository<UserEntity>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="driver"></param>
        public UserRepository(IDriver driver)
            : base(driver)
        {
        }

        public async Task<UserEntity> Create(UserEntity data)
        {
            var query = $@"CREATE (v:{VertexTypes.USER} 
                        {{ 
                            id: '{data.Id}', 
                            username: '{data.Username}',
                            dob: '{data.DateOfBirth}'
                        }}) RETURN v";
            return await Session.WriteTransactionAsync(async tx =>
            {
                var cursor = await tx.RunAsync(query);
                await cursor.FetchAsync();
                var fandom = new UserEntity();
                fandom.LoadRecord(cursor.Current);
                return fandom;
            });
        }

        public int Count()
        {
            return Count(VertexTypes.USER);
        }

        public IEnumerable<UserEntity> GetAll()
        {
            var query = $"MATCH (v:{VertexTypes.USER}) RETURN v";
            var data = Session.Run(query);
            return data.Select(v =>
            {
                var entity = new UserEntity();
                entity.LoadRecord(v);
                return entity;
            });
        }
    }
}