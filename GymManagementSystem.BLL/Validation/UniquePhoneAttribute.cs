using GymManagementSystem.DAL.Data.Contexts;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.Validation;

public class UniquePhoneAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var phone = value?.ToString();

        if (string.IsNullOrWhiteSpace(phone))
            return ValidationResult.Success;

        var context = (GymDbContext?)validationContext.GetService(typeof(GymDbContext));

        if (context == null)
            throw new InvalidOperationException("DbContext not found.");

        bool exists = context.Members.Any(m => m.Phone == phone);

        if (exists)
            return new ValidationResult("Phone number already exists!");

        return ValidationResult.Success;
    }
}
