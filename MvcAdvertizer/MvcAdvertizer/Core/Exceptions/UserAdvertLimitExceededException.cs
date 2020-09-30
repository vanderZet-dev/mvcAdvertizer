using System;

namespace MvcAdvertizer.Core.Exceptions
{
    public class UserAdvertLimitExceededException : Exception
    {
        public UserAdvertLimitExceededException() {
        }

        public UserAdvertLimitExceededException(string message) : base(message){
        }

        public override string Message
        {
            get
            {
                if (base.Message != "")
                {
                    return base.Message;
                }
                return "Exceeded advert limit for user";
            }
        }
    }
}
