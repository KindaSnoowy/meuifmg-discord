using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeuIFMG_DiscordBot.models
{
    public class LoginError : Exception
    {
        public LoginError()
        {
        }

        public LoginError(string message)
            : base(message)
        {
        }
    }

    public class CreatingUserError : Exception
    {
        public CreatingUserError()
        {
        }

        public CreatingUserError(string message)
            : base(message)
        {
        }
    }

    public class FindingUserError : Exception
    {
        public FindingUserError()
        {
        }

        public FindingUserError(string message)
            : base(message)
        {
        }
    }
}