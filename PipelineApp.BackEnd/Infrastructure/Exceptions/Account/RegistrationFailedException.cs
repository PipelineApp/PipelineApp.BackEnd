// <copyright file="RegistrationFailedException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Account
{
    using System;

    /// <summary>
    /// The exception that is thrown when there was an error registering a new user.
    /// </summary>
    /// <seealso cref="Exception" />
    public class RegistrationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFailedException"/> class.
        /// </summary>
        /// <param name="error">The errors resulting from the account registration failure.</param>
        public RegistrationFailedException(string error)
            : base(error)
        {
        }
    }
}
