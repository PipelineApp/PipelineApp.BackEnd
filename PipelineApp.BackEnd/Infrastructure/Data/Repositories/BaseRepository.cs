namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Interfaces.Data;
    using Interfaces.Repositories;
    using Neo4jClient;

    public class BaseRepository<TModel> : IRepository<TModel> where TModel : IEntity
    {
        private IGraphClient _graphClient;

        public BaseRepository(GraphClient client)
        {
            _graphClient = client;
            _graphClient.Connect();
        }

        public async Task<IList<TModel>> GetAllAsync()
        {
            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.ToList();
        }

        public async Task<TModel> GetByIdAsync(Guid id)
        {
            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Where<TModel>(e => e.Id == id)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.SingleOrDefault();
        }

        public async Task<TModel> SaveAsync(TModel model)
        {
            var results = await GraphClient.Cypher.Create($"(e:{typeof(TModel).Name} {{model}})")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.Single();
        }

        public async Task<TModel> UpdateAsync(TModel model)
        {
            Guid id = model.Id;

            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                                     .Where<IEntity>(e => e.Id == id)
                                     .Set("e = {model}")
                                     .WithParam("model", model)
                                     .Return(e => e.As<TModel>())
                                     .ResultsAsync;
            return results.Single();
        }

        public async Task DeleteAsync(TModel model)
        {
            Guid id = model.Id;

            await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                              .Where<IEntity>(e => e.Id == id)
                              .DetachDelete("e")
                              .ExecuteWithoutResultsAsync();
        }

        public async Task<long> Count()
        {
            var results = await GraphClient.Cypher.Match($"(e:{typeof(TModel).Name})")
                .Return(e => e.Count())
                .ResultsAsync;
            return results.SingleOrDefault();
        }

        protected IGraphClient GraphClient
        {
            get
            {
                if (_graphClient == null)
                {
                    throw new ApplicationException("Initialize the graph client!");
                }

                return _graphClient;
            }
        }
    }
}
