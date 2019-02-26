// <copyright file="RefreshTokenEntity.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Data.Entities
{
    using System;

    /// <summary>
    /// Data-layer representation of refresh token information used to maintain
    /// a user's session.
    /// </summary>
    public class RefreshTokenEntity : BaseEntity
    {
        /// <summary>
        /// Gets or sets the datetime at which this refresh token was issued.
        /// </summary>
        /// <value>
        /// The datetime at which this refresh token was issued.
        /// </value>
        public DateTime IssuedUtc { get; set; }

        /// <summary>
        /// Gets or sets the datetime at which this refresh token will expire.
        /// </summary>
        /// <value>
        /// The datetime at which this refresh token will expire.
        /// </value>
        public DateTime ExpiresUtc { get; set; }

        /// <summary>
        /// Gets or sets the refresh token value.
        /// </summary>
        /// <value>
        /// The refresh token value.
        /// </value>
        public string Token { get; set; }
    }
}
