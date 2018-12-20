// <copyright file="AppSettings.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.Configuration
{
    /// <summary>
    /// Wrapper class for application settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the application settings related to auth tokens.
        /// </summary>
        public AuthAppSettings Auth { get; set; }

        /// <summary>
        /// Gets or sets the application settings related to cross-origin requests.
        /// </summary>
        public CorsAppSettings Cors { get; set; }

        /// <summary>
        /// Gets or sets the application settings related to graph database queries.
        /// </summary>
        public GraphDbAppSettings GraphDb { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the site is in maintenance mode.
        /// </summary>
        public bool IsMaintenanceMode { get; set; }
    }
}
