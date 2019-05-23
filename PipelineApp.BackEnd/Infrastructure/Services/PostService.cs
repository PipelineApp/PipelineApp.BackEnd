// <copyright file="PostService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Services
{
    using System;
    using System.Collections.Generic;
    using Data.Entities;
    using Data.Relationships;
    using Data.Requests;
    using Interfaces.Mappers;
    using Interfaces.Repositories;
    using Interfaces.Services;
    using Models.DomainModels;
    using Models.DomainModels.Posts;

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
            var postRequest = new CreateNodeRequest<PostEntity>(entity.Post)
                .WithInboundRelationshipFrom<IsAuthorOf>(personaId);
            var createdPost = repository.CreateWithRelationships(postRequest);
            var versionRequest = new CreateNodeRequest<PostVersionEntity>(entity.Versions[0])
                .WithOutboundRelationshipTo<BelongsToFandom>(fandomId)
                .WithInboundRelationshipFrom<HasVersion>(createdPost.Id);
            var createdVersion = repository.CreateWithRelationships(versionRequest);
            return mapper.ToDomainModel(createdPost, createdVersion);
        }
    }
}
