namespace PipelineApp.BackEnd.Infrastructure.Exceptions.Account
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The exception that is thrown when there was an error updating a user's account information.
    /// </summary>
    /// <seealso cref="Exception" />
    public class InvalidAccountInfoUpdateException : Exception
    {
        /// <summary>
        /// Gets or sets the errors resulting from the account update failure.
        /// </summary>
        /// <value>
        /// The errors resulting from the account update failure.
        /// </value>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAccountInfoUpdateException"/> class.
        /// </summary>
        /// <param name="errors">The errors resulting from the account update failure.</param>
        public InvalidAccountInfoUpdateException(List<string> errors)
            : base("There was an error updating the users's account information.")
        {
            Errors = errors;
        }
    }
}
