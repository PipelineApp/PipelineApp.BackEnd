// <copyright file="CreatePipelineRequestModel.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.RequestModels.Pipeline
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Exceptions.Pipeline;

    /// <summary>
    /// Request model containing data about a user's request to create a new pipeline.
    /// </summary>
    public class CreatePipelineRequestModel
    {
        /// <summary>
        /// Gets or sets the pipeline's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the pipeline's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Throws an exception if the pipeline is not valid.
        /// </summary>
        /// <exception cref="InvalidPipelineException">Thrown if the pipeline is not valid.</exception>
        public void AssertIsValid()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(Name))
            {
                errors.Add("You must provide a name for this pipeline.");
            }

            if (errors.Any())
            {
                throw new InvalidPipelineException(errors);
            }
        }
    }
}
