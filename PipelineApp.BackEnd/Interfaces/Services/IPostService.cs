// <copyright file="IPostService.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Services
{
    using System;
    using AutoMapper;
    using Models.DomainModels;
    using Repositories;

    /// <summary>
    /// Service for data manipulation relating to posts.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Initializes and saves a new root post object.
        /// </summary>
        /// <param name="post">Data regarding the post to be created.</param>
        /// <param name="personaId">The unique ID of the persona authoring the post.</param>
        /// <param name="fandomId">The unique ID of the fandom with which the post is associated.</param>
        /// <param name="repository">The post repository.</param>
        /// <param name="mapper">The mapper.</param>
        /// <returns>
        /// The created post.
        /// </returns>
        Post CreateRootPost(Post post, Guid personaId, Guid fandomId, IPostRepository repository, IMapper mapper);
    }
}
