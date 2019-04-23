namespace PipelineApp.BackEnd.Interfaces.Repositories
{
    using Infrastructure.Data.Entities;

    /// <summary>
    /// Extension of base repository containing methods related to
    /// post data.
    /// </summary>
    public interface IPostRepository : IRepository<PostEntity>
    {
    }
}
