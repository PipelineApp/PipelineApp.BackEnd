// <copyright file="UserNotFoundException.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Account
{
    using System;

    /// <summary>
    /// The exception that is thrown when there was an error retrieving a user.
    /// </summary>
    /// <seealso cref="Exception" />
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class.
        /// </summary>
        public UserNotFoundException()
            : base("The requested user does not exist.")
        {
        }
    }
}
