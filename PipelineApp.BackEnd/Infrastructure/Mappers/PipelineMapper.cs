// <copyright file="PipelineMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using System.Diagnostics.CodeAnalysis;
    using AutoMapper;
    using Data.Entities;
    using Data.Entities.EntityCollections;
    using Models.DomainModels;
    using Models.RequestModels.Pipeline;
    using Models.ViewModels;

    /// <summary>
    /// Mapping class for mapping between view model, domain model, and entity representations of pipelines.
    /// </summary>
    /// <seealso cref="Profile" />
    [ExcludeFromCodeCoverage]
    public class PipelineMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineMapper"/> class.
        /// </summary>
        public PipelineMapper()
        {
            CreateMap<Pipeline, PipelineEntity>()
                .ReverseMap();
            CreateMap<Pipeline, PipelineDto>()
                .ReverseMap();
            CreateMap<Pipeline, PipelineEntityDataCollection>()
                .ForMember(d => d.Pipeline, opt => opt.MapFrom((source, dest) =>
                {
                    var entity = new PipelineEntity
                    {
                        Name = source.Name,
                        Description = source.Description
                    };
                    if (source.Id != null)
                    {
                        entity.Id = source.Id.Value;
                    }

                    return entity;
                }))
                .ReverseMap()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Pipeline.Id))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Pipeline.Name))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Pipeline.Description));
            CreateMap<UpsertPipelineRequestModel, Pipeline>();
        }
    }
}
