using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.Common
{
    public class Error
    {
        public string Code { get; } = default!;
        public string Description { get; } = default!;
        public ErrorType Type { get; } = default!;

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        // Factory method to create an error
        public static Error Failure(string code ="General.Failure", string description ="A General Failure Has Occurred.")
        {
            return new Error(code, description, ErrorType.Failure);
        }


        public static Error Validation(string code = "General.Validation", string description = "A Validation Error Has Occurred.")
        {
            return new Error(code, description, ErrorType.Validation);
        }

        public static Error NotFound(string code = "General.NotFound", string description = "The Requested Resource Was Not Found.")
        {
            return new Error(code, description, ErrorType.NotFound);
        }

        public static Error Unauthorized(string code = "General.Unauthorized", string description = "Unauthorized Access.")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }
    
        public static Error Forbidden(string code = "General.Forbidden", string description = "Forbidden Access.")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }


        public static Error InValidCredentials(string code = "General.InValidCredentials", string description = "Invalid Credentials Has Occurred.")
        {
            return new Error(code, description, ErrorType.InValidCredentials);
        }


    }
}
