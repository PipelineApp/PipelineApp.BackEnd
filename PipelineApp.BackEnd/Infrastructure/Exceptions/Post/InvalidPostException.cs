// <copyright file="InvalidPostException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Post
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The exception that is thrown when a post object is invalid.
    /// </summary>
    /// <seealso cref="Exception" />
    public class InvalidPostException : Exception
    {
        /// <summary>
        /// Gets or sets the errors rendering the post invalid.
        /// </summary>
        /// <value>
        /// The errors rendering the post invalid.
        /// </value>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPostException"/> class.
        /// </summary>
        /// <param name="errors">The errors rendering the post invalid.</param>
        public InvalidPostException(List<string> errors)
            : base("The supplied post information is invalid.")
        {
            Errors = errors;
        }
    }
}
