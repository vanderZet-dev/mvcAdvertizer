using System;

namespace MvcAdvertizer.Core.Exceptions
{
    public class FileStorageSavePathInvalidException : Exception
    {
        private readonly string customMessage;

        public FileStorageSavePathInvalidException() {
        }

        public FileStorageSavePathInvalidException(string message) : base(message) {

            if (message == null)
            {
                message = "NULL";
            }

            customMessage = message;
        }

        public override string Message
        {
            get
            {
                if (base.Message != "")
                {
                    return base.Message;
                }
                return $"Base path: \"{customMessage}\" is invalid. Сheck file storage settings.";
            }
        }
    }
}
