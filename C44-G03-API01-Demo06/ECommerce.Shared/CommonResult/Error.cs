using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description , ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        // Methods
        public static Error Failure(string code = "General Failure", string description = "General Failure Occurred !")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code = "Validation Error", string description = "Validation Error Occurred !")
        {
            return new Error(code, description, ErrorType.Validation);
        }
        public static Error NotFound(string code = "Not Found", string description = "Error 404 Not Found !")
        {
            return new Error(code, description, ErrorType.NotFound);
        }
        public static Error Unauthorized(string code = "Unauthorized Error", string description = "Unauthorized Occurred !")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }
        public static Error Forbidden(string code = "Forbidden Error", string description = "Forbidden Error Occurred")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string code = "Invalid Credintials Error", string description = "Invalid Credintials Error Occurred !")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }

        // No Errors =>

        // One Error Occurred =>

        // More than one Error Occurred =>
    }
}
