namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Entities;
    using Data.Relationships;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    public class PostService : IPostService
    {
        public Post CreateBasePost(Post post, Guid personaId, Guid fandomId, IPostRepository repository, IMapper mapper)
        {
            if (personaId == null)
            {
                throw new ArgumentException("Persona ID cannot be null.");
            }
            var entity = mapper.Map<PostEntity>(post);
            var inboundRelationships = new List<BaseRelationship>()
            {
                new IsAuthorOf() {SourceId = personaId}
            };
            var outboundRelationships = new List<BaseRelationship>
            {
                new BelongsTo {TargetId = fandomId}
            };
            var result = repository.CreateWithRelationships(entity, inboundRelationships, outboundRelationships);
            return mapper.Map<Post>(result);
        }
    }
}
