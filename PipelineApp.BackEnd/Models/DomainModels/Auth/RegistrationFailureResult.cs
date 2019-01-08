// <copyright file="RegistrationFailureResult.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.DomainModels.Auth
{
    /// <summary>
    /// Domain model class for information relating to a failed registration.
    /// </summary>
    public class RegistrationFailureResult
    {
        /// <summary>
        /// Gets or sets the name of the registration error.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the code indicating what type of error occurred.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the error status code.
        /// </summary>
        public int StatusCode { get; set; }
    }
}
