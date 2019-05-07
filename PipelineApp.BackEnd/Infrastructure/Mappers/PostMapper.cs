// <copyright file="PostMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Data.Entities;
    using Models.DomainModels;
    using Models.RequestModels.Post;
    using Models.ViewModels;

    /// <summary>
    /// Mapping class for mapping between view model, domain model, and entity representations of posts.
    /// </summary>
    /// <seealso cref="Profile" />
    [ExcludeFromCodeCoverage]
    public class PostMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostMapper"/> class.
        /// </summary>
        public PostMapper()
        {
            CreateMap<Post, PostEntity>()
                .ReverseMap();
            CreateMap<Post, PostDto>()
                .ReverseMap();
            CreateMap<CreateRootPostRequestModel, Post>();
        }
    }
}
