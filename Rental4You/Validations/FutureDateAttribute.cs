using System.ComponentModel.DataAnnotations;

namespace Rental4You.Validations
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var date = (DateTime)value;
                if (date < DateTime.Now)
                {
                    return new ValidationResult("The date must be a future date");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class DeliveryDateAttribute : ValidationAttribute 
    {
        public DeliveryDateAttribute(string dateToCompareToFieldName)
        {
            DateToCompareToFieldName = dateToCompareToFieldName;
        }

        private string DateToCompareToFieldName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime laterDate = (DateTime)value;

                DateTime earlierDate = (DateTime)validationContext.ObjectType.GetProperty(DateToCompareToFieldName).GetValue(validationContext.ObjectInstance, null);

                if (laterDate > earlierDate)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Delivery date must be posterior to Pickup date");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class BeforeCurrentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime birthDate = (DateTime)value;

            if (birthDate > DateTime.Now)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
