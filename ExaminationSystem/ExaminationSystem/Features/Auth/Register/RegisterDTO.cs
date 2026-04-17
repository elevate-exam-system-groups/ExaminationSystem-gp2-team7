using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Auth.Register
{
    public record RegisterDTO([EmailAddress] String email ,string password , string full_name);
    
}
