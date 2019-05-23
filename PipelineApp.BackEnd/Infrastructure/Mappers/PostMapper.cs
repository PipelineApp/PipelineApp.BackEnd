// <copyright file="PostMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Entities;
    using Data.Entities.EntityCollections;
    using Interfaces.Mappers;
    using Models.DomainModels;
    using Models.DomainModels.Posts;
    using Models.RequestModels.Post;
    using Models.ViewModels;

    /// <inheritdoc />
    public class PostMapper : IPostMapper
    {
        /// <inheritdoc />
        public PostDto ToDto(Post post)
        {
            var mostRecentVersion = post.Versions.OrderBy(v => v.UtcDateTime).FirstOrDefault();
            return new PostDto
            {
                Id = post.Id,
                Title = mostRecentVersion?.Title,
                Content = mostRecentVersion?.Content,
                ParentPost = ToDto(post.ParentPost),
                RootPost = ToDto(post.RootPost)
            };
        }

        /// <inheritdoc />
        public Post ToDomainModel(CreateRootPostRequestModel requestModel)
        {
            var post = new Post
            {
                Versions = new List<PostVersion>
                {
                    new PostVersion {
                        Content = requestModel.Content,
                        Title = requestModel.Title,
                        FandomId = requestModel.FandomId
                    }
                }
            };
            return post;
        }

        public Post ToDomainModel(PostEntity postEntity, List<VersionEntity<PostEntity>> versionEntities)
        {
            var post = new Post
            {
                Id = postEntity.Id,
                Versions = versionEntities.Select(e =>
                {
                    var version = e as PostVersionEntity ?? new PostVersionEntity();
                    return new PostVersion
                    {
                        Content = version.Content,
                        Title = version.Title,
                        UtcDateTime = version.UtcDateTime,
                        FandomId = version.FandomId
                    };
                }).ToList()
            };
            return post;
        }

        public Post ToDomainModel(PostEntity postEntity, VersionEntity<PostEntity> versionEntity)
        {
            return ToDomainModel(postEntity, new List<VersionEntity<PostEntity>> { versionEntity });
        }

        /// <inheritdoc />
        public PostEntityDataCollection ToEntity(Post post)
        {
            return new PostEntityDataCollection
            {
                Post = new PostEntity
                {
                    Id = post.Id.GetValueOrDefault()
                },
                Versions = post.Versions.Select(m => new PostVersionEntity
                {
                    Id = m.Id.GetValueOrDefault(),
                    Content = m.Content,
                    Title = m.Title,
                    UtcDateTime = m.UtcDateTime
                }).ToList()
            };
        }
    }
}
