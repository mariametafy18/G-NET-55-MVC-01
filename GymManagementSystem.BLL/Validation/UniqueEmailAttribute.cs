using GymManagementSystem.DAL.Data.Contexts;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.Validation;

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var email = value?.ToString();
        if (string.IsNullOrWhiteSpace(email))
            return ValidationResult.Success;

        var context = (GymDbContext?)validationContext.GetService(typeof(GymDbContext));
        if (context == null)
            throw new InvalidOperationException("DbContext not found.");

        bool exists = context.Members.Any(m => m.Email == email);
        if (exists)
            return new ValidationResult("Email already exists!");

        return ValidationResult.Success;
    }
}
