// <copyright file="AuthAppSettings.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.Configuration
{
    /// <summary>
    /// Wrapper class for application settings related to auth tokens.
    /// </summary>
    public class AuthAppSettings
    {
        /// <summary>
        /// Gets or sets the auth token key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the auth token issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the auth token audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the access token duration in minutes.
        /// </summary>
        public int AccessExpireMinutes { get; set; }

        /// <summary>
        /// Gets or sets the refresh token duration in minutes.
        /// </summary>
        public int RefreshExpireMinutes { get; set; }
    }
}
