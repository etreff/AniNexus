using HotChocolate;
using HotChocolate.Language;
using HotChocolate.Types;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace AniNexus.GraphQL.Types.Primitives;

public class DateOnlyType : ScalarType<DateOnly, StringValueNode>
{
    private const string DateFormat = "yyyy-MM-dd";

    public DateOnlyType()
        : this("DateOnly")
    {

    }

    public DateOnlyType(NameString name, string? description = null, BindingBehavior bind = BindingBehavior.Explicit)
        : base(name, bind)
    {
        Description = description;
    }

    public override IValueNode ParseResult(object? resultValue)
    {
        if (resultValue is null)
        {
            return NullValueNode.Default;
        }

        if (resultValue is string text)
        {
            return new StringValueNode(text);
        }

        if (resultValue is DateOnly date)
        {
            return ParseValue(date);
        }

        throw new SerializationException($"Cannot parse scalar {Name} into {resultValue!.GetType()}", this);
    }

    public override bool TrySerialize(object? runtimeValue, out object? resultValue)
    {
        if (runtimeValue is null)
        {
            resultValue = null;
            return true;
        }

        if (runtimeValue is DateOnly date)
        {
            resultValue = Serialize(date);
            return true;
        }

        resultValue = null;
        return false;
    }

    public override bool TryDeserialize(object? resultValue, out object? runtimeValue)
    {
        if (resultValue is null)
        {
            runtimeValue = null;
            return true;
        }

        if (resultValue is string text && TryDeserializeFromString(text, out DateOnly? value))
        {
            runtimeValue = value;
            return true;
        }

        if (resultValue is DateOnly date)
        {
            runtimeValue = date;
            return true;
        }

        runtimeValue = null;
        return false;
    }

    protected override DateOnly ParseLiteral(StringValueNode valueSyntax)
    {
        if (TryDeserializeFromString(valueSyntax.Value, out DateOnly? value))
        {
            return value.Value;
        }

        throw new SerializationException($"Cannot parse literal {Name} into {valueSyntax.GetType()}", this);
    }

    protected override StringValueNode ParseValue(DateOnly runtimeValue)
    {
        return new StringValueNode(Serialize(runtimeValue));
    }

    private static string Serialize(DateOnly value)
    {
        return value.ToString(DateFormat, CultureInfo.InvariantCulture);
    }

    private static bool TryDeserializeFromString(string? serialized, [NotNullWhen(true)] out DateOnly? result)
    {
        if (DateOnly.TryParse(serialized, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateOnly date))
        {
            result = date;
            return true;
        }

        result = null;
        return false;
    }
}
