// <copyright file="PostMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using Data.Entities;
    using Interfaces.Mappers;
    using Models.DomainModels;
    using Models.RequestModels.Post;
    using Models.ViewModels;

    /// <inheritdoc />
    public class PostMapper : IPostMapper
    {
        /// <inheritdoc />
        public PostDto ToDto(Post post)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Post ToDomainModel(CreateRootPostRequestModel requestModel)
        {
            var post = new Post
            {
                Content = requestModel.Content,
                Title = requestModel.Title
            };
            return post;
        }

        /// <inheritdoc />
        public Post ToDomainModel(PostEntity entity)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public PostEntity ToEntity(Post post)
        {
            throw new System.NotImplementedException();
        }
    }
}
