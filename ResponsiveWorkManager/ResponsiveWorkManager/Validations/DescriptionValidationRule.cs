﻿using System.Windows.Controls;

namespace ResponsiveWorkManager.Validations
{
	public class DescriptionValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			string description = value as string;
			if (string.IsNullOrEmpty(description))
			{
				return new ValidationResult(false, "Description cannot be empty");
			}
			return ValidationResult.ValidResult;
		}
	}
}
