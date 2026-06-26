using System.Runtime.CompilerServices;
using System.Text;

namespace RentalSystem.Application.Common.Validation;

public static class ValidationMessageBuilder
{

    // Purpose: Organized error
    public static string IsNotNullOrEmpty<TData>(Type entityType)
    {
        if (typeof(TData) == typeof(string))
            return $"{entityType.Name} {{PropertyName}} shouldn't be whitespace, empty or null.";

        return $"{entityType.Name} {{PropertyName}} shouldn't be null.";
    }

    public static string IsExceededLength(Type entityType)
    => $"{entityType.Name} {{PropertyName}} length shouldn't be larger than {{ComparisonValue}}.";

    public static string IsNotExceededLength(Type entityType)
    => $"{entityType.Name} {{PropertyName}} length shouldn't be lower than {{ComparisonValue}}.";

    public static string IsGreaterThan(Type entityType)
    => $"{entityType.Name} {{PropertyName}} value should be greater than {{ComparisonValue}}.";

    public static string IsLowerThan(Type entityType)
    => $"{entityType.Name} {{PropertyName}} value should be lower than {{ComparisonValue}}.";
}