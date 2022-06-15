using Microsoft.AspNetCore.Mvc;

namespace API.Helpers
{
    public class OperationalResult
    {
        public OperationalResult()
        {
        }

        public OperationalResult(ActionContext context)
        {
            IsSuccess = false;
            StatusCode = 400;
            ErrorMessage = ConcatenateErrors(context);
        }

        private string ConcatenateErrors(ActionContext context)
        {
            string message = "";
            foreach (var keyModelStatePair in context.ModelState)
            {
                var key = keyModelStatePair.Key;
                var errors = keyModelStatePair.Value.Errors;
                if (errors is null || errors.Count == 0)
                    continue;

                for (var i = 0; i < errors.Count; i++)
                    message += errors[i].ErrorMessage;
            }
            message = message.Replace(".", ", ");
            message = message.Remove(message.Length - 2);
            return message;
        }

        public bool IsSuccess { get; set; } = true;
        public int StatusCode { get; set; } = 200;
        public object? Content { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
