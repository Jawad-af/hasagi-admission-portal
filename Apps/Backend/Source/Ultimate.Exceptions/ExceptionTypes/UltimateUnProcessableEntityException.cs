using Ultimate.Exceptions.Models;

namespace Ultimate.Exceptions.ExceptionTypes
{
    public class UltimateUnProcessableEntityException : Exception
    {
        public List<ErrorDetail> ErrorDetails { get; private set; }

        public UltimateUnProcessableEntityException(List<ErrorDetail> errorDetails)
        {
            ErrorDetails = errorDetails;
        }
    }
}
