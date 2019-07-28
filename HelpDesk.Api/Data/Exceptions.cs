using System;

namespace HelpDesk.Api.Data
{
    public class AccountException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountException"/> class.
        /// </summary>
        public AccountException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountException"/> class with
        /// the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AccountException(string message)
            : base(message)
        {
        }
    }

    public class AccountAuthorizationException : AccountException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAuthorizationException"/> class.
        /// </summary>
        public AccountAuthorizationException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAuthorizationException"/> class with
        /// the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AccountAuthorizationException(string message)
            : base(message)
        {
        }
    }

    public class AccountExistsException : AccountException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountExistsException"/> class.
        /// </summary>
        public AccountExistsException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountExistsException"/> class with
        /// the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AccountExistsException(string message)
            : base(message)
        {
        }
    }

    public class AccountNotFoundException : AccountException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountNotFoundException"/> class.
        /// </summary>
        public AccountNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountNotFoundException"/> class with
        /// the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public AccountNotFoundException(string message)
            : base(message)
        {
        }
    }

    public class SessionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionException"/> class.
        /// </summary>
        public SessionException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionException"/> class with
        /// the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public SessionException(string message)
            : base(message)
        {
        }
    }

    public class SessionNotFoundException : SessionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNotFoundException"/> class.
        /// </summary>
        public SessionNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionNotFoundException"/> class with
        /// the provided message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public SessionNotFoundException(string message)
            : base(message)
        {
        }
    }
}
