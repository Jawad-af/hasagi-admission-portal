using Ultimate.Exceptions.Models;

namespace Ultimate.Exceptions.ExceptionTypes
{
    public class UltimateValidationException : Exception
    {
        public List<ErrorDetail> ErrorDetails { get; private set; }

        public UltimateValidationException(List<ErrorDetail> errorDetails)
        {
            ErrorDetails = errorDetails;
        }
    }
}
