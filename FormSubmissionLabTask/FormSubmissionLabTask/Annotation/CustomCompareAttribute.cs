using System;
using System.ComponentModel.DataAnnotations;

public class CustomCompareAttribute : ValidationAttribute
{
    private readonly string _otherPropertyName;

    public CustomCompareAttribute(string otherPropertyName)
    {
        _otherPropertyName = otherPropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(_otherPropertyName);
        if (property == null)
        {
            return new ValidationResult($"Property {_otherPropertyName} not found.");
        }

        var otherValue = property.GetValue(validationContext.ObjectInstance);

        string[] emailDivided = value.ToString().Split('@');

        string emailId = emailDivided[0];

        if (!Equals(emailId, otherValue.ToString()))
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must match {_otherPropertyName}.");
        }

        return ValidationResult.Success;
    }
}
