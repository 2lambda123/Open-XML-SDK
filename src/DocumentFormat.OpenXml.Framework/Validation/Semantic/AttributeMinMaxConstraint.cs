﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DocumentFormat.OpenXml.Validation.Semantic
{
    /// <summary>
    /// One attribute value must no bigger than another's.
    /// Attribute value should be number.
    /// </summary>
    internal class AttributeMinMaxConstraint : SemanticConstraint
    {
        private readonly string _minAttributeLocalName;
        private readonly string _minAttributeNamesapce;
        private readonly string _maxAttributeLocalName;
        private readonly string _maxAttributeNamesapce;

        public AttributeMinMaxConstraint(string minAttributeNamespace, string minAttributeLocalName, string maxAttributeNamespace, string maxAttributeLocalName)
            : base(SemanticValidationLevel.Element) // TODO: add error message for this class
        {
            _minAttributeNamesapce = minAttributeNamespace;
            _minAttributeLocalName = minAttributeLocalName;
            _maxAttributeNamesapce = maxAttributeNamespace;
            _maxAttributeLocalName = maxAttributeLocalName;
        }

        public override ValidationErrorInfo? ValidateCore(ValidationContext context)
        {
            var element = context.Stack.Current?.Element;

            if (element is null)
            {
                return null;
            }

            var minAttributeValue = element.GetAttributeValueEx(_minAttributeLocalName, _minAttributeNamesapce);

            // If value cannot be converted into double, that means attribute type is not correct.
            // That's the job of schema validation, semantic validation will do nothing to avoid throw duplicated error.
            if (!double.TryParse(minAttributeValue, out double minValue))
            {
                return null;
            }

            var maxAttributeValue = element.GetAttributeValueEx(_maxAttributeLocalName, _maxAttributeNamesapce);

            // If value cannot be converted into double, that means attribute type is not correct.
            // That's the job of schema validation, semantic validation will do nothing to avoid throw duplicated error.
            if (!double.TryParse(maxAttributeValue, out double maxValue))
            {
                return null;
            }

            if (minValue <= maxValue)
            {
                return null;
            }

            string errorId = string.Empty; // TODO: add error id
            string errorMessage = string.Empty; // TODO: add error message

            return new ValidationErrorInfo
            {
                Id = errorId,
                ErrorType = ValidationErrorType.Semantic,
                Node = element,
                Description = errorMessage,
            };
        }
    }
}
