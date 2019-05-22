// <copyright file="PostService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using Data.Entities;
    using Data.Relationships;
    using Data.Requests;
    using Interfaces.Mappers;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;

    /// <inheritdoc />
    public class PostService : IPostService
    {
        /// <inheritdoc />
        public Post CreateRootPost(Post post, Guid personaId, Guid fandomId, IPostRepository repository, IPostMapper mapper)
        {
            if (personaId == null)
            {
                throw new ArgumentException("Persona ID cannot be null.");
            }

            var entity = mapper.ToEntity(post);
            var request = new CreateNodeRequest<PostEntity>(entity)
                .WithInboundRelationshipFrom<IsAuthorOf>(personaId)
                .WithOutboundRelationshipTo<BelongsToFandom>(fandomId);
            var result = repository.CreateWithRelationships(request);
            return mapper.ToDomainModel(result);
        }
    }
}
