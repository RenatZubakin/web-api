using System.ComponentModel.DataAnnotations;

namespace WebApi.MinimalApi.Models;

public class UserToCreateDTO
{
    [Required(AllowEmptyStrings = false)]
    public string Login { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}