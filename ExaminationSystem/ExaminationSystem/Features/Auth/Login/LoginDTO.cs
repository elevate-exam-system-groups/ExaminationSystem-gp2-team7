using System.ComponentModel.DataAnnotations;

namespace ExaminationSystem.Features.Auth.Login
{
    public record LoginDTO([EmailAddress] String email ,string password);
    
}
