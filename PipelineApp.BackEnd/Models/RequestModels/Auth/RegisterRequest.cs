// <copyright file="RegisterRequest.cs" company="Blackjack Software">
// Copyright (c) Blackjack Software. All rights reserved.
// Licensed under the GPL v3 license. See LICENSE file in the project root for full license information.
// </copyright>

namespace PipelineApp.BackEnd.Models.RequestModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Exceptions.Account;

    /// <summary>
    /// Request model containing data about a user's request to register a new account.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmed password.
        /// </summary>
        /// <value>
        /// The confirmed password.
        /// </value>
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Throws an exception if the registration request is not valid.
        /// </summary>
        /// <exception cref="InvalidRegistrationException">Thrown if the registration request is not valid.</exception>
        public virtual void AssertIsValid()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Email))
            {
                errors.Add("You must provide a valid email address.");
            }

            if (string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                errors.Add("You must provide a password.");
            }

            if (!string.Equals(Password, ConfirmPassword, StringComparison.CurrentCulture))
            {
                errors.Add("Your passwords must match.");
            }

            if (DateOfBirth == null || DateOfBirth.Value.ToUniversalTime() > DateTime.UtcNow)
            {
                errors.Add("You must enter a valid birthdate.");
            }

            if (errors.Any())
            {
                throw new InvalidRegistrationException(errors);
            }
        }
    }
}
