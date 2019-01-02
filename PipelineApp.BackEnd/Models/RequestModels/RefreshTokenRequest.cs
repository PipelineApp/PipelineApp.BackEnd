// <copyright file="RefreshTokenRequest.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.RequestModels
{
    /// <summary>
    /// Request model containing data about a user's request to refresh their JWT to maintain their session.
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        public string RefreshToken { get; set; }
    }
}
