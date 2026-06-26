using System.Reflection;
using System.Text;
using FluentValidation;
using Microsoft.Extensions.Hosting;
using RentalSystem.Application.Common.Behaviors;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddOpenRequestPreProcessor(typeof(LoggingBehavior<>));
            // Where you register your pipeline
            config.AddOpenBehavior(typeof(UnhandledExceptionBehavior<,>));
            config.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });

        // https://docs.fluentvalidation.net/en/latest/configuring.html#overriding-the-property-name:~:text=Property%20name%20resolution%20is%20also%20pluggable.%20By%20default%2C%20the%20name%20of%20the%20property%20extracted%20from%20the%20MemberExpression%20passed%20to%20RuleFor.%20If%20you%20want%20to%20change%20this%20logic%2C%20you%20can%20set%20the%20DisplayNameResolver%20property%20on%20the%20ValidatorOptions%20class%3A
        // This option changes how property name display
        ValidatorOptions.Global.DisplayNameResolver = (type, member, expression) =>
        {
            if (member != null)
            {
                return SplitFieldName(member.Name);
            }
            return null;
        };
    }

    /// <summary>
    /// Get everything within a string after dot
    /// <strong>v.DepositFee</strong> -> <strong>DepositFee</strong>
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private static string? GetMember(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return null;

        var afterDotIndex = text.LastIndexOf('.');

        return text[(afterDotIndex + 1)..]; // everything after dot
    }

    private static string SplitFieldName(string? fieldName)
    {
        // string with "+" arithmetic operator creates new string every time because it is immutable
        // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/#using-stringbuilder-for-fast-string-creation
        // with StringBuilder you can mutate string in a faster way

        var member = GetMember(fieldName);

        if (string.IsNullOrWhiteSpace(member))
            return "unknown field name";

        var split = new StringBuilder();

        for (int i = 0; i < member.Length; i++)
        {
            var c = member[i];

            if (i > 0 && char.IsUpper(c))
                split.Append(' ');

            split.Append(char.ToLowerInvariant(c));
        }

        return split.ToString();
    }
}