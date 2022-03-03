using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus;

/// <summary>
/// <see cref="Expression"/> extensions.
/// </summary>
public static class ExpressionExtensions
{
    private static readonly Regex _propertyNameRegex = new(@"^[a-zA-Z_][a-zA-Z0-9_]*(?:\.[a-zA-Z_][a-zA-Z0-9_]*)*$");

    /// <summary>
    /// Attempts to get the member name from an expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="memberName">The name of the member on success.</param>
    /// <returns><see langword="true"/> if the member name was successfully extracted, <see langword="false"/> otherwise.</returns>
    public static bool TryGetMemberName(this Expression expression, [NotNullWhen(true)] out string? memberName)
    {
        Guard.IsTrue(expression.NodeType == ExpressionType.MemberAccess, "Expression must be a member access expression.");

        return TryFixMemberAccessName(expression.ToString(), out memberName);
    }

    private static bool TryFixMemberAccessName(ReadOnlySpan<char> body, [NotNullWhen(true)] out string? memberName)
    {
        int qualifierIndex = body.IndexOf('.');
        if (qualifierIndex >= 0)
        {
            body = body.Slice(qualifierIndex + 1);
        }

        qualifierIndex = body.IndexOf('(');
        if (qualifierIndex >= 0)
        {
            body = body.Slice(qualifierIndex + 1);
        }

        qualifierIndex = body.IndexOf(')');
        if (qualifierIndex >= 0)
        {
            body = body.Slice(0, qualifierIndex);
        }

        string name = new(body);
        bool result = _propertyNameRegex.IsMatch(name);
        memberName = result ? name : null;

        return result;
    }

    /// <summary>
    /// Gets the body of an expression as text.
    /// </summary>
    /// <param name="expression">The expression to get the body of.</param>
    /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null"/></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetBody<T>(this Expression<Func<T>> expression)
    {
        Guard.IsNotNull(expression, nameof(expression));

        return GetBody(expression.Body);
    }

    /// <summary>
    /// Gets the body of an expression as text.
    /// </summary>
    /// <param name="expression">The expression to get the body of.</param>
    public static string GetBody(this Expression expression)
    {
        Guard.IsNotNull(expression, nameof(expression));

        string body;
        switch (expression)
        {
            case MemberExpression memberBody:
                body = memberBody.Member.Name;
                break;
            case ConstantExpression constantBody:
                body = constantBody.Value!.ToString()!;
                break;
            case UnaryExpression unaryBody:
                return GetBody(unaryBody.Operand);
            case MethodCallExpression methodBody:
                string c = methodBody.Method.IsStatic && methodBody.Method.DeclaringType is not null ? $"{methodBody.Method.DeclaringType.Name}." : string.Empty;
                body = $"{c}{methodBody.Method.Name}({methodBody.Arguments.Aggregate(string.Empty, static (prev, next) => $"{prev}, {GetBody(next)}")[(methodBody.Arguments.Count > 0 ? 2 : 0)..]})";
                break;
            case BinaryExpression simpleBinaryExpression:
                string op = Expression.MakeBinary(simpleBinaryExpression.NodeType, Expression.Constant(0), Expression.Constant(0)).ToString().Replace("0", "").Replace("(", "").Replace(")", "");
                body = $"{GetBody(simpleBinaryExpression.Left)} {op} {GetBody(simpleBinaryExpression.Right)}";
                break;
            default:
                body = expression.ToString();
                break;
        }

        return body;
    }
}
