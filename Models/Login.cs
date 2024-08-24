using System.ComponentModel.DataAnnotations;

namespace MvcLoginTemplate.Models;

public class Login
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}