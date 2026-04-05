using System;

namespace GentrysQuest.Game.Online.API
{
    public class SessionExpiredException : InvalidOperationException
    {
        public SessionExpiredException(string message)
            : base(message)
        {
        }
    }
}
