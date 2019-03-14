// <copyright file="InvalidPipelineException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Pipeline
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The exception that is thrown when a pipeline object is invalid.
    /// </summary>
    /// <seealso cref="Exception" />
    public class InvalidPipelineException : Exception
    {
        /// <summary>
        /// Gets or sets the errors rendering the pipeline invalid.
        /// </summary>
        /// <value>
        /// The errors rendering the pipeline invalid.
        /// </value>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidPipelineException"/> class.
        /// </summary>
        /// <param name="errors">The errors rendering the pipeline invalid.</param>
        public InvalidPipelineException(List<string> errors)
            : base("The supplied pipeline information is invalid.")
        {
            Errors = errors;
        }
    }
}
