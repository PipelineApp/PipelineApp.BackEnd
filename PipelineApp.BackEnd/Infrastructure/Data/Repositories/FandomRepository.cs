namespace PipelineApp.BackEnd.Infrastructure.Data.Repositories
{
    using Entities;
    using Interfaces.Repositories;
    using Neo4jClient;

    public class FandomRepository : BaseRepository<FandomEntity>, IFandomRepository
    {
        public FandomRepository(GraphClient graphClient) : base(graphClient) { }
    }
}
