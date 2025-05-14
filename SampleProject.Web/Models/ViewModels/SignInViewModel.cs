using System.ComponentModel.DataAnnotations;

namespace SampleProject.Web.Models.ViewModels
{
    public record SignInViewModel([Required] string Email, [Required] string Password)
    {
    }
}
