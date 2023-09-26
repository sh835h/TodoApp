using System.ComponentModel.DataAnnotations;

public class AuthenticationModel
{
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [StringLength(100, MinimumLength = 6)]
    public string ConfirmPassword { get; set; }

    [Required]
    [MaxLength(50)] 
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)] 
    public string LastName { get; set; }

    [Required]
    [MaxLength(100)] 
    public bool IsActive { get; set; }


    [Required]
    [MaxLength(100)]
    public string Roles { get; set; }





}

public class User
{
    
    public string Email { get; set; }

    public string Password { get; set; }


   
}



