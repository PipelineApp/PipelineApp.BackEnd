// <copyright file="PersonaMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Mappers
{
    using Data.Entities;
    using Interfaces.Mappers;
    using Models.DomainModels;
    using Models.ViewModels;

    /// <inheritdoc />
    public class PersonaMapper : IPersonaMapper
    {
        /// <inheritdoc />
        public PersonaDto ToDto(Persona persona)
        {
            return new PersonaDto
            {
                Id = persona.Id,
                Description = persona.Description,
                PersonaName = persona.PersonaName,
                Slug = persona.Slug
            };
        }

        /// <inheritdoc />
        public Persona ToDomainModel(PersonaEntity entity)
        {
            return new Persona
            {
                Id = entity.Id,
                Description = entity.Description,
                PersonaName = entity.PersonaName,
                Slug = entity.Slug
            };
        }

        /// <inheritdoc />
        public Persona ToDomainModel(PersonaDto personaDto)
        {
            return new Persona
            {
                Id = personaDto.Id,
                Description = personaDto.Description,
                PersonaName = personaDto.PersonaName,
                Slug = personaDto.Slug
            };
        }

        /// <inheritdoc />
        public PersonaEntity ToEntity(Persona persona)
        {
            var entity = new PersonaEntity();
            entity.Id = persona.Id ?? entity.Id;
            entity.Description = persona.Description;
            entity.PersonaName = persona.PersonaName;
            entity.Slug = persona.Slug;
            return entity;
        }
    }
}
