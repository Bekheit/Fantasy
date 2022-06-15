using API.Helpers;

namespace API.Extensions
{
    public static class OperationalResultExtensions
    {
        public static OperationalResult Unauthorized(this OperationalResult operationalResult, string message)
        {
            operationalResult.IsSuccess = false;
            operationalResult.StatusCode = 401;
            operationalResult.ErrorMessage = message;

            return operationalResult;
        }

        public static OperationalResult BadRequest(this OperationalResult operationalResult, string message)
        {
            operationalResult.IsSuccess = false;
            operationalResult.StatusCode = 400;
            operationalResult.ErrorMessage = message;

            return operationalResult;
        }

        
    }
}
