namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Entities;
    using Data.Relationships;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    public class PostService : IPostService
    {
        public async Task<Post> CreateBasePost(Post post, Guid personaId, IPostRepository repository, IMapper mapper)
        {
            if (personaId == null)
            {
                throw new ArgumentException("Persona ID cannot be null.");
            }
            var entity = mapper.Map<PostEntity>(post);
            var result = await repository.CreateWithInboundRelationshipAsync<IsAuthorOf, PersonaEntity>(entity, personaId);
            return mapper.Map<Post>(result);
        }
    }
}
