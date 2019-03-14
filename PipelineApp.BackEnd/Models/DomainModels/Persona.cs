// <copyright file="Persona.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.DomainModels
{
    using System;
    using Infrastructure.Data.Entities;
    using ViewModels;

    /// <summary>
    /// Domain model representing a user's persona.
    /// </summary>
    public class Persona
    {
        /// <summary>
        /// Gets or sets the unique identifier of this persona.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets this persona's unique slug.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets this persona's name.
        /// </summary>
        public string PersonaName { get; set; }

        /// <summary>
        /// Gets or sets this persona's description.
        /// </summary>
        public string Description { get; set; }

        public Persona(PersonaEntity entity)
        {
            Id = entity.Id;
            Slug = entity.Slug;
            PersonaName = entity.PersonaName;
            Description = entity.Description;
        }

        public PersonaDto ToDto()
        {
            return new PersonaDto
            {
                Id = Id,
                Description = Description,
                PersonaName = PersonaName,
                Slug = Slug
            };
        }
    }
}
