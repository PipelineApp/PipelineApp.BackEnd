// <copyright file="PersonaMapper.cs" company="Blackjack Software">
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
    /// Mapping class for mapping between view model, domain model, and entity representations of personas.
    /// </summary>
    /// <seealso cref="Profile" />
    [ExcludeFromCodeCoverage]
    public class PersonaMapper : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaMapper"/> class.
        /// </summary>
        public PersonaMapper()
        {
            CreateMap<Persona, PersonaEntity>()
                .ReverseMap();
            CreateMap<Persona, PersonaDto>()
                .ReverseMap();
        }
    }
}
