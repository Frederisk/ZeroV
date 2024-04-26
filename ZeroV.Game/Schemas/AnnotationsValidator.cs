using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ZeroV.Game.Schemas;

public class AnnotationsValidator {

    public static Boolean TryValidateObjectRecursive(Object? instance, IDictionary<Object, Object?>? validationContexts, ICollection<ValidationResult>? results) {
        ArgumentNullException.ThrowIfNull(instance);

        if (instance is IEnumerable) {
            throw new ArgumentException($"The {nameof(instance)} must be a single object, not a collection. You should validate each item in the collection individually.", nameof(instance));
        }

        return tryValidateObjectRecursive(instance, validationContexts, results, new HashSet<Object>());
    }

    private static Boolean tryValidateObjectRecursive(Object instance, IDictionary<Object, Object?>? validationContexts, ICollection<ValidationResult>? results, ISet<Object> validatedObjects) {
        // short-circuit to avoid infinite loops on cyclical object graphs
        if (validatedObjects.Contains(instance)) {
            return true;
        }
        validatedObjects.Add(instance);

        var isValidate = Validator.TryValidateObject(instance, new ValidationContext(instance, null, validationContexts), results, true);

        foreach (PropertyInfo property in instance.GetType().GetProperties()) {
            // Ignore:
            if (!property.CanRead // Can not be read
                || property.PropertyType == typeof(String) // String
                || property.PropertyType.IsValueType // Value type
                || property.GetIndexParameters().Length is not 0 // Has index parameters
                || property.GetCustomAttribute(typeof(SkipRecursiveValidationAttribute), false) is not null) {
                continue;
            }

            var value = property.GetValue(instance, null);
            if (value is null) {
                continue;
            }

            if (value is IEnumerable enumValue) {
                foreach (var item in enumValue) {
                    if (item is not null) {
                        List<ValidationResult> nestedResults = [];
                        if (!tryValidateObjectRecursive(item, validationContexts, nestedResults, validatedObjects)) {
                            isValidate = false;
                            foreach (ValidationResult nestedItem in nestedResults) {
                                results?.Add(new ValidationResult(nestedItem.ErrorMessage, nestedItem.MemberNames.Select(name => $"{property.Name}.{name}")));
                            }
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
}

[AttributeUsage(AttributeTargets.Property)]
public class SkipRecursiveValidationAttribute : Attribute { }
