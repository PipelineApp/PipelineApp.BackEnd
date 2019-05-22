// <copyright file="IPersonaMapper.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Interfaces.Mappers
{
    using Infrastructure.Data.Entities;
    using Models.DomainModels;
    using Models.ViewModels;

    /// <summary>
    /// Mapping class for mapping between view model, domain model, and entity representations of personas.
    /// </summary>
    public interface IPersonaMapper
    {
        /// <summary>
        /// Maps the given domain model to a DTO.
        /// </summary>
        /// <param name="persona">The persona domain model.</param>
        /// <returns>A DTO object representation of the persona.</returns>
        PersonaDto ToDto(Persona persona);

        /// <summary>
        /// Maps the given entity to a domain model.
        /// </summary>
        /// <param name="entity">The persona data entity.</param>
        /// <returns>A domain layer representation of the persona.</returns>
        Persona ToDomainModel(PersonaEntity entity);

        /// <summary>
        /// Maps the given DTO to a domain model.
        /// </summary>
        /// <param name="personaDto">The persona DTO.</param>
        /// <returns>A domain layer representation of the persona.</returns>
        Persona ToDomainModel(PersonaDto personaDto);

        /// <summary>
        /// Maps the given domain model to a data entity.
        /// </summary>
        /// <param name="persona">The persona domain model.</param>
        /// <returns>A data layer representation of the persona.</returns>
        PersonaEntity ToEntity(Persona persona);
    }
}
