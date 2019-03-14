namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using System.Threading.Tasks;
    using Models.DomainModels;
    using Repositories;

    public interface IPipelineService
    {
        Task<Pipeline> CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository);
    }
}
