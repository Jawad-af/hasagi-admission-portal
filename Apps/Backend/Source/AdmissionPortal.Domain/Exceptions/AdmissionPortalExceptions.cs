using Ultimate.Exceptions.ExceptionTypes;
using Ultimate.Exceptions.Models;

namespace AdmissionPortal.Domain.Exceptions
{
    public static class AdmissionPortalExceptions
    {
        public static void ThrowUnAuthorizedException(string message)
        {
            throw new UltimateUnAuthorizedException(message);
        }

        public static void ThrowUnProcessableEntityException(List<ErrorDetail> errorDetails)
        {
            throw new UltimateUnProcessableEntityException(errorDetails);
        }

        public static void ThrowNotFoundException(string entity)
        {
            throw new UltimateNotFoundException(entity);
        }
    }
}
