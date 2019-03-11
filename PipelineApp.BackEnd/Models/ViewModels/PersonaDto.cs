// <copyright file="PersonaDto.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Infrastructure.Exceptions.Persona;
    using Microsoft.EntityFrameworkCore.Internal;

    /// <summary>
    /// View model representation of a user's persona.
    /// </summary>
    public class PersonaDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the persona.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the persona's unique slug.
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the pserona's name.
        /// </summary>
        public string PersonaName { get; set; }

        /// <summary>
        /// Gets or sets the persona's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Throws an exception if the persona is not valid.
        /// </summary>
        /// <exception cref="InvalidPersonaException">Thrown if the persona is not valid.</exception>
        public virtual void AssertIsValid()
        {
            var errors = new List<string>();

            var slugRegex = new Regex(@"^[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*$");
            if (string.IsNullOrWhiteSpace(Slug) || !slugRegex.IsMatch(Slug))
            {
                errors.Add("You must provide a valid slug.");
            }

            if (string.IsNullOrWhiteSpace(PersonaName))
            {
                errors.Add("You must provide a persona name.");
            }

            if (errors.Any())
            {
                throw new InvalidPersonaException(errors);
            }
        }
    }
}
