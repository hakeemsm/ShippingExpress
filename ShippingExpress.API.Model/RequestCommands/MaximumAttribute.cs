using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ShippingExpress.API.Model.RequestCommands
{
    public class MaximumAttribute : ValidationAttribute
    {
        private readonly int _max;

        public MaximumAttribute(int max):base("The {0} field value must be maximum {1}")
        {
            _max = max;
        }

        public override string FormatErrorMessage(string name)
        {

            return string.Format(
                CultureInfo.CurrentCulture,
                base.ErrorMessageString,
                name,
                _max);
        }

        public override bool IsValid(object value)
        {
            int intValue;
            return value != null && int.TryParse(value.ToString(), out intValue) && intValue <= _max;
        }
    }
}