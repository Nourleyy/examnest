using Microsoft.AspNetCore.Identity;

namespace ExamNest.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }


}
