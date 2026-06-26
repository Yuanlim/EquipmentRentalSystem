// https://docs.fluentvalidation.net/en/latest/custom-validators.html?highlight=custom%20extension#predicate-validator

using FluentValidation;

namespace RentalSystem.Application.Common.Validation;

/// <summary>
/// Add new custom rule to FluentValidator with message for auto organized.
/// </summary>
public static class FluentValidationExtensions
{
    // Second generic as target type intellisense method

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is int</br>
    /// <br>This method checks wether a value is greater than threshold.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="threshold"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, int> GreaterThanWithMessage<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        int threshold,
        Type entityType
    )
    {
        return ruleBuilder
            .GreaterThan(threshold)
            .WithMessage(ValidationMessageBuilder.IsGreaterThan(entityType));
    }

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is decimal</br>
    /// <br>This method checks wether a value is greater than threshold.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="threshold"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, decimal> GreaterThanWithMessage<T>(
        this IRuleBuilder<T, decimal> ruleBuilder,
        int threshold,
        Type entityType
    )
    {
        return ruleBuilder
            .GreaterThan(threshold)
            .WithMessage(ValidationMessageBuilder.IsGreaterThan(entityType));
    }

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is int</br>
    /// <br>This method checks wether a value is lower than threshold.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="threshold"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, int> LowerThanWithMessage<T>(
        this IRuleBuilder<T, int> ruleBuilder,
        int threshold,
        Type entityType
    )
    {
        return ruleBuilder
            .LessThan(threshold)
            .WithMessage(ValidationMessageBuilder.IsLowerThan(entityType));
    }

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is decimal</br>
    /// <br>This method checks wether a value is lower than threshold.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="threshold"></param>
    /// /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, decimal> LowerThanWithMessage<T>(
        this IRuleBuilder<T, decimal> ruleBuilder,
        int threshold,
        Type entityType
    )
    {
        return ruleBuilder
            .LessThan(threshold)
            .WithMessage(ValidationMessageBuilder.IsLowerThan(entityType));
    }

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is int</br>
    /// <br>This method checks wether a value is greater than threshold.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="length"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> MaxLengthWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int length,
        Type entityType
    )
    {
        return ruleBuilder
            .MaximumLength(length)
            .WithMessage(ValidationMessageBuilder.IsExceededLength(entityType));
    }

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is decimal</br>
    /// <br>This method checks wether a value is greater than threshold.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="length"></param>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> MinLengthWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int length,
        Type entityType
    )
    {
        return ruleBuilder
            .MinimumLength(length)
            .WithMessage(ValidationMessageBuilder.IsNotExceededLength(entityType));
    }

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is Generic</br>
    /// <br>This method checks wether a value is not null.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, TElement> IsNotNullOrWhitespaceWithMessage<T, TElement>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Type entityType
    )
    {
        return ruleBuilder
            .Must(v => v is not null)
            .WithMessage(ValidationMessageBuilder.IsNotNullOrEmpty<TElement>(entityType));
    }

    /// <summary>
    /// <br>T as request type</br>
    /// <br>Checked data is string?</br>
    /// <br>This method checks wether a value is not null.</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <param name="entityType"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> IsNotNullOrWhitespaceWithMessage<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        Type entityType
    )
    {
        return ruleBuilder
            .Must(v => !string.IsNullOrWhiteSpace(v))
            .WithMessage(ValidationMessageBuilder.IsNotNullOrEmpty<string?>(entityType));
    }
}