using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Stijn
/// https://stackoverflow.com/questions/5920403/asp-net-mvc-3-data-annotations-greaterthan-lowerthan-for-datetime-and-int
/// </summary>
namespace Bistronger.Data.Validations
{
    public class BusinessHourGreaterThanAttribute : ValidationAttribute
    {
        string otherPropertyName;

        public BusinessHourGreaterThanAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                // Using reflection we can get a reference to the other date property, in this example the project start date
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                // Let's check that otherProperty is of type DateTime as we expect it to be
                if (otherPropertyInfo.PropertyType.Equals(new DateTime().GetType()))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                    // if the end date is lower than the start date, than the validationResult will be set to false and return
                    // a properly formatted error message
                    if (TimeSpan.Compare(referenceProperty.TimeOfDay, toValidate.TimeOfDay) == 0 && referenceProperty.TimeOfDay.TotalMinutes == 0)
                    {
                        return validationResult;
                    }

                    if (TimeSpan.Compare(referenceProperty.TimeOfDay, toValidate.TimeOfDay) < 0)
                    {
                        if (Math.Abs((toValidate.TimeOfDay - referenceProperty.TimeOfDay).TotalMinutes) < 60)
                            validationResult = new ValidationResult(ErrorMessageString);
                    }
                    else
                    {
                        validationResult = new ValidationResult(ErrorMessageString);
                    }

                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                // Do stuff, i.e. log the exception
                // Let it go through the upper levels, something bad happened
                throw;
            }

            return validationResult;
        }
    }
}
