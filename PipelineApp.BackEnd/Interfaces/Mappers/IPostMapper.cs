// <copyright file="IPostMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Mappers
{
    using System.Collections.Generic;
    using Infrastructure.Data.Entities;
    using Infrastructure.Data.Entities.EntityCollections;
    using Models.DomainModels;
    using Models.DomainModels.Posts;
    using Models.RequestModels.Post;
    using Models.ViewModels;

    /// <summary>
    /// Mapping class for mapping between view model, domain model, and entity representations of posts.
    /// </summary>
    public interface IPostMapper
    {
        /// <summary>
        /// Maps the given domain model to a DTO.
        /// </summary>
        /// <param name="post">The post domain model.</param>
        /// <returns>A DTO representation of the post.</returns>
        PostDto ToDto(Post post);

        /// <summary>
        /// Maps the given request model to a domain layer object.
        /// </summary>
        /// <param name="requestModel">The post request model.</param>
        /// <returns>A domain layer representation of the post.</returns>
        Post ToDomainModel(CreateRootPostRequestModel requestModel);

        /// <summary>
        /// Maps the given data entities to a domain model.
        /// </summary>
        /// <param name="postEntity">The base post information.</param>
        /// <param name="versionEntities">The post's version information.</param>
        /// <returns>A domain layer representation of the post.</returns>
        Post ToDomainModel(PostEntity postEntity, List<VersionEntity<PostEntity>> versionEntities);

        Post ToDomainModel(PostEntity postEntity, VersionEntity<PostEntity> versionEntity);

        /// <summary>
        /// Maps the given domain model to a data entity.
        /// </summary>
        /// <param name="post">The post domain model.</param>
        /// <returns>A data layer representation of the post.</returns>
        PostEntityDataCollection ToEntity(Post post);
    }
}
