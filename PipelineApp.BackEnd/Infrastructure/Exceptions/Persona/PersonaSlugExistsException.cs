// <copyright file="PersonaSlugExistsException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Persona
{
    using System;

    /// <summary>
    /// The exception that is thrown when there was an error creating or updating a persona
    /// because its slug is already used.
    /// </summary>
    /// <seealso cref="Exception" />
    public class PersonaSlugExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaSlugExistsException"/> class.
        /// </summary>
        public PersonaSlugExistsException()
            : base("The supplied persona slug is already in use.")
        {
        }
    }
}
