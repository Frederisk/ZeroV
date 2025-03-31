/* // Unused code
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ZeroV.Game.Utils;

public class AnnotationsValidator {

    /// <summary>
    /// Determines whether the specified object is valid using the validation context recursively, validation results collection, and a value that specifies whether to validate all properties.
    /// </summary>
    /// <param name="instance">The object to validate.</param>
    /// <param name="validationContexts">The context that describes the object to validate.</param>
    /// <param name="results">A collection to hold each failed validation.</param>
    /// <returns>
    /// <see langword="true"/> if the object validates; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="instance"/> is <see cref="IEnumerable"/> or doesn't match the <see cref="ValidationContext.ObjectInstance"/> on <paramref name="validationContexts"/>.</exception>
    public static Boolean TryValidateObjectRecursive(Object instance, IDictionary<Object, Object?>? validationContexts, ICollection<ValidationResult>? results) {
        ArgumentNullException.ThrowIfNull(instance);

        if (instance is IEnumerable) {
            throw new ArgumentException($"The {nameof(instance)} must be a single object, not a collection. You should validate each item in the collection individually.", nameof(instance));
        }

        return tryValidateObjectRecursive(instance, validationContexts, results, new HashSet<Object>());
    }

    private static Boolean tryValidateObjectRecursive(Object instance, IDictionary<Object, Object?>? validationContexts, ICollection<ValidationResult>? results, ISet<Object> validatedObjects) {
        // A short-circuit to avoid infinite loops on cyclical object graphs
        if (!validatedObjects.Add(instance)) {
            return true;
        }

        Boolean isValidate = Validator.TryValidateObject(instance, new ValidationContext(instance, null, validationContexts), results, true);

        // Validate each property recursively
        foreach (PropertyInfo property in instance.GetType().GetProperties()) {
            // Ignore:
            if (isIllegalProperty(property)) {
                continue;
            }

            Object? value = property.GetValue(instance, null);
            if (value is null) {
                continue;
            }

            if (value is IEnumerable enumValue) {
                foreach (Object? item in enumValue) {
                    if (item is null) { continue; }
                    List<ValidationResult> nestedResults = [];
                    if (!tryValidateObjectRecursive(item, validationContexts, nestedResults, validatedObjects)) {
                        isValidate = false;
                        foreach (ValidationResult nestedItem in nestedResults) {
                            results?.Add(new ValidationResult(nestedItem.ErrorMessage, nestedItem.MemberNames.Select(name => $"{property.Name}.{name}")));
                        }
                    }
                }
            } else {
                List<ValidationResult> nestedResults = [];
                if (!tryValidateObjectRecursive(value, validationContexts, nestedResults, validatedObjects)) {
                    isValidate = false;
                    foreach (ValidationResult nestedItem in nestedResults) {
                        results?.Add(new ValidationResult(nestedItem.ErrorMessage, nestedItem.MemberNames.Select(name => $"{property.Name}.{name}")));
                    }
                }
            }
        }
        return isValidate;
    }

    private static Boolean isIllegalProperty(PropertyInfo property) =>
        !property.CanRead // Can not be read
        || property.PropertyType == typeof(String) // String
        || property.PropertyType.IsValueType // Value type
        || property.GetIndexParameters().Length > 0 // Has index parameters
        || property.GetCustomAttributes(typeof(SkipRecursiveValidationAttribute), false).Length > 0;
}

/// <summary>
/// Specifies that the property should not be validated recursively by <see cref="AnnotationsValidator"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SkipRecursiveValidationAttribute : Attribute { }
*/
