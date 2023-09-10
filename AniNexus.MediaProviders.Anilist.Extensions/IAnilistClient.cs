using System.Runtime.CompilerServices;
using AniNexus.MediaProviders.Anilist.Client;
using Microsoft.Extensions.Logging;
using StrawberryShake;

namespace AniNexus.MediaProviders.Anilist.Api;

/// <summary>
/// Extensions for working with the generated Anilist GraphQL client.
/// </summary>
public static class AnilistClientExtensions
{
    /// <summary>
    /// Checks whether the API call failed due to an authentication error and logs out a message if that is the reason it failed.
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <param name="logger">The logger to log the message to.</param>
    /// <param name="callerName">The action that failed.</param>
    /// <returns>Whether the operation failed due to an authentication error.</returns>
    public static bool IsAuthenticationErrorResult(this IOperationResult result, ILogger logger, [CallerMemberName] string? callerName = null)
    {
        var errors = result.Errors;
        if (errors.Count == 0)
        {
            return false;
        }

        foreach (var error in errors)
        {
            if (error.Message.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogError("Unable to perform action {ActionName} - the client is unauthorized.", callerName);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks whether the operation result has any errors.
    /// </summary>
    /// <param name="result">The operation result.</param>
    /// <returns>Whether the operation result returned with any errors.</returns>
    public static bool HasErrors(this IOperationResult result)
    {
        return result.Errors.Count > 0;
    }

    /// <summary>
    /// Builds a context dictionary using the errors on this <see cref="IOperationResult"/> object.
    /// </summary>
    /// <param name="result">The operation result object.</param>
    /// <returns>A context dictionary to pass to <see cref="ILogger.BeginScope{TState}(TState)"/>.</returns>
    public static Dictionary<string, object?> BuildContextDictionary(this IOperationResult result)
    {
        var errors = result.Errors;
        if (errors.Count > 0)
        {
            var dict = new Dictionary<string, object?>(errors.Count);
            for (int i = 0; i < errors.Count; ++i)
            {
                var error = errors[i];

                dict.Add($"Error{i + 1}_Message", error.Message);
                if (!string.IsNullOrWhiteSpace(error.Code))
                {
                    dict.Add($"Error{i + 1}_Code", error.Code);
                }
                if (error.Exception is not null)
                {
                    dict.Add($"Error{i + 1}_Exception_Message", error.Exception.Message);
                    if (error.Exception.StackTrace is not null)
                    {
                        dict.Add($"Error{i + 1}_Exception_Stack", error.Exception.StackTrace);
                    }
                }
            }

            return dict;
        }

        return new Dictionary<string, object?>(0);
    }

    /// <summary>
    /// An object that contains the necessary information to create an API call to a paged API.
    /// </summary>
    /// <param name="Client">The GraphQL client.</param>
    /// <param name="Page">The page to fetch.</param>
    /// <param name="CancellationToken">The cancellation token.</param>
    /// <param name="PerPage">The number of records to return in this page.</param>
    public record struct PageEnumerationContext(IAnilistClient Client, int Page, CancellationToken CancellationToken, int PerPage = 500);

    /// <summary>
    /// Enumerates over the response of an operation that returns a paged result.
    /// </summary>
    /// <typeparam name="TResult">The operation response type.</typeparam>
    /// <typeparam name="T">The response data type.</typeparam>
    /// <param name="client">The client that sends requests to Anilist.</param>
    /// <param name="fetchOperation">An action that returns the results of an API call.</param>
    /// <param name="pageAccessor">An action that accesses the page data from the result.</param>
    /// <param name="dataAccessor">An action that accesses the response data from the result.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="GraphQLClientException"></exception>
    public static async IAsyncEnumerable<T> EnumeratePagedResult<TResult, T>(
        this IAnilistClient client,
        Func<PageEnumerationContext, Task<IOperationResult<TResult>>> fetchOperation,
        Func<TResult, IPageInfo?> pageAccessor,
        Func<TResult, IReadOnlyList<T?>?> dataAccessor,
        ILogger logger,
        [EnumeratorCancellation] CancellationToken cancellationToken)
        where TResult : class
    {
        int pageNumber = 1;
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var context = new PageEnumerationContext(client, pageNumber, cancellationToken);
            var response = await fetchOperation(context);
            if (response.HasErrors())
            {
                logger.LogError("Unable to fetch paged operation for page {Page} ({PerPage} records per page).", pageNumber, context.PerPage);
                throw new GraphQLClientException(response.Errors);
            }

            if (response.Data is null)
            {
                break;
            }

            var elements = dataAccessor(response.Data);
            if (elements is not null)
            {
                logger.LogInformation("Enumerating page {Page} of {RecordCount} records ({PerPage} records per page)",
                    pageNumber, elements.Count, context.PerPage);

                foreach (var element in elements)
                {
                    if (element is not null)
                    {
                        yield return element;
                    }
                }
            }

            var page = pageAccessor(response.Data);
            if (page?.HasNextPage != true)
            {
                break;
            }

            ++pageNumber;
        }
    }
}
