namespace PipelineApp.BackEnd.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Data;
    using Neo4jClient;

    public interface IRepository<TModel> where TModel : IEntity
    {
        Task<IList<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(Guid id);

        Task<TModel> SaveAsync(TModel model);

        Task<TModel> UpdateAsync(TModel model);

        Task DeleteAsync(TModel model);

        Task<long> Count();
    }
}
