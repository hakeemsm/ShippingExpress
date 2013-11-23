using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ShippingExpress.API.Model.RequestCommands
{
    public class MinimumAttribute : ValidationAttribute
    {
        private readonly int _minimum;

        public MinimumAttribute(int minimum)
        {
            _minimum = minimum;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, base.ErrorMessage, name, _minimum);
        }

        public override bool IsValid(object value)
        {
            int intVal;
            return value != null && int.TryParse(value.ToString(), out intVal) && intVal >= _minimum;
        }
    }
}