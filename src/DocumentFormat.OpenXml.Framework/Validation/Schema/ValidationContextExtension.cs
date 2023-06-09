﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Globalization;

namespace DocumentFormat.OpenXml.Validation.Schema
{
    internal static class ValidationContextExtension
    {
        // helper methods to compose a  ValidationErrorInfo for schema validation error.
        internal static ValidationErrorInfo ComposeSchemaValidationError(
            this ValidationContext validationContext,
            OpenXmlElement? element,
            OpenXmlElement? child,
            string messageId,
            params object?[] args)
        {
            return ComposeValidationError(validationContext, ValidationErrorType.Schema, element, child, messageId, args);
        }

        // helper methods to compose a  ValidationErrorInfo for MC validation error.
        internal static ValidationErrorInfo ComposeMcValidationError(
            this ValidationContext validationContext,
            OpenXmlElement? element,
            string messageId,
            params object?[] args)
        {
            return ComposeValidationError(validationContext, ValidationErrorType.MarkupCompatibility, element, null, messageId, args);
        }

        internal static ValidationErrorInfo ComposeValidationError(
            this ValidationContext validationContext,
            ValidationErrorType errorType,
            OpenXmlElement? element,
            OpenXmlElement? child,
            string messageId,
            params object?[] args)
        {
            var message = ValidationResources.ResourceManager.GetString(messageId, CultureInfo.CurrentCulture);

            var description = message switch
            {
                null => SR.Format(ExceptionMessages.UnknownError, messageId),
                _ => SR.Format(message, args),
            };

            return new ValidationErrorInfo
            {
                ErrorType = errorType,
                Part = validationContext.Stack.Current?.Part,
                Node = element,
                Id = messageId,
                RelatedNode = child,
                Description = description,
            };
        }
    }
}
