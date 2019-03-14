namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Threading.Tasks;
    using Data.Entities;
    using Data.Relationships;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;
    using Models.ViewModels;

    public class PipelineService : IPipelineService
    {
        public async Task<Pipeline> CreatePipeline(Pipeline pipeline, Guid? userId, IPipelineRepository repository)
        {
            if (userId == null)
            {
                throw new ArgumentException("User ID cannot be null.");
            }
            var entity = pipeline.ToEntity();
            var result = await repository.CreateWithInboundRelationshipAsync<Manages, UserEntity>(entity, userId.Value);
            return new Pipeline(entity);
        }
    }
}
