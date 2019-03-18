// <copyright file="PipelineNotFoundException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Pipeline
{
    using System;

    /// <summary>
    /// The exception that is thrown when there was an error retrieving a pipeline.
    /// </summary>
    /// <seealso cref="Exception" />
    public class PipelineNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineNotFoundException"/> class.
        /// </summary>
        public PipelineNotFoundException()
            : base("The requested pipeline does not exist for the current user.")
        {
        }
    }
}
