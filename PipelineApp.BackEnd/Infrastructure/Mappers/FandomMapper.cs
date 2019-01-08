// <copyright file="FandomMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Data.Entities;
    using Models.DomainModels;
    using Models.ViewModels;

    /// <summary>
    /// Mapping class for mapping between view model, domain model, and entity representations of fandoms.
    /// </summary>
    /// <seealso cref="Profile" />
    [ExcludeFromCodeCoverage]
    public class FandomMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FandomMapper"/> class.
        /// </summary>
        public FandomMapper()
        {
            CreateMap<Fandom, FandomEntity>()
                .ReverseMap();
            CreateMap<Fandom, FandomDto>()
                .ReverseMap();
        }
    }
}
