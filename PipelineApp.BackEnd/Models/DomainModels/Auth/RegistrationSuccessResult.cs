// <copyright file="RegistrationSuccessResult.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.DomainModels.Auth
{
    using Newtonsoft.Json;

    /// <summary>
    /// Domain model containing the result of a communication with the auth server
    /// registering a new user.
    /// </summary>
    public class RegistrationSuccessResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the created user.
        /// </summary>
        [JsonProperty("_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the created user's email.
        /// </summary>
        public string Email { get; set; }
    }
}
