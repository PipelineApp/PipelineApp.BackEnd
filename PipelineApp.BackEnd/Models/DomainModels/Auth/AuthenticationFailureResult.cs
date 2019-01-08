// <copyright file="AuthenticationFailureResult.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.DomainModels.Auth
{
    using Newtonsoft.Json;

    /// <summary>
    /// Domain model class for information relating to a failed login.
    /// </summary>
    public class AuthenticationFailureResult
    {
        /// <summary>
        /// Gets or sets the error ID.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the error description.
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
