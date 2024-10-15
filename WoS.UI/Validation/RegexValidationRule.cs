using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WoS.UI.Validation
{
    internal class RegexValidationRule : ValidationRule
    {
        private string _pattern;
        private Regex _regex;

        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                _regex = new Regex(_pattern, RegexOptions.IgnoreCase);
            }
        }

        public string Message { get; set; }

        public RegexValidationRule() { }

        public override ValidationResult Validate(object value, CultureInfo ultureInfo)
        {
            return value == null || !_regex.Match(value.ToString()).Success ?
                new ValidationResult(false, Message) :
                new ValidationResult(true, null);
        }
    }

    internal class RegexValidationRuleCaseInSensitive : ValidationRule
    {
        private string _pattern;
        private Regex _regex;

        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                _regex = new Regex(_pattern, RegexOptions.IgnoreCase);
            }
        }

        public string Message { get; set; }

        public RegexValidationRuleCaseInSensitive() { }

        public bool Negating { get; set; } = false;

        public override ValidationResult Validate(object value, CultureInfo ultureInfo)
        {
            if (!Negating)
            {
                return value == null || !_regex.Match(value.ToString()).Success ?
                    new ValidationResult(false, Message) :
                    new ValidationResult(true, null);
            }

            return value == null || Regex.IsMatch(value.ToString(), Pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace) ?
                new ValidationResult(false, Message) :
                new ValidationResult(true, null);
        }
    }

    internal class RegexWhiteSpaceString : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var strValue = value as string;
            if (strValue.Length == 0 || (strValue.Length > 0 && strValue.Trim().Length == 0))
                return new ValidationResult(false, Message);

            return new ValidationResult(true, null);
        }

        public string Message { get; set; }

        public RegexWhiteSpaceString() { }
    }

}
