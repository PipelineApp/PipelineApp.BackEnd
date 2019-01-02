// <copyright file="InvalidRefreshTokenException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Account
{
    using System;

    /// <summary>
    /// The exception that is thrown when there was an error processing a user's refresh token.
    /// </summary>
    /// <seealso cref="Exception" />
    public class InvalidRefreshTokenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidRefreshTokenException"/> class.
        /// </summary>
        public InvalidRefreshTokenException()
            : base("The supplied refresh token is invalid.")
        {
        }
    }
}
