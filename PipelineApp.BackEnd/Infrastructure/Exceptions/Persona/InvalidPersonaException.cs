// <copyright file="InvalidPersonaException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Persona
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The exception that is thrown when a persona object is invalid.
    /// </summary>
    /// <seealso cref="Exception" />
    public class InvalidPersonaException : Exception
    {
        /// <summary>
        /// Gets or sets the errors rendering the persona invalid.
        /// </summary>
        /// <value>
        /// The errors rendering the persona invalid.
        /// </value>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPersonaException"/> class.
        /// </summary>
        /// <param name="errors">The errors rendering the persona invalid.</param>
        public InvalidPersonaException(List<string> errors)
            : base("The supplied persona information is invalid.")
        {
            Errors = errors;
        }
    }
}
