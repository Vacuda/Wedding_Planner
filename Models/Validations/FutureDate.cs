using System;
using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext){
        DateTime unboxed_value=(DateTime)value;
        
        if (unboxed_value < DateTime.Now){
            return new ValidationResult("The wedding must be in the future!");
        }
        return ValidationResult.Success;
    }

}
}