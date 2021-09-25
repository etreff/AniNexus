using System.Net.Http.Json;
using AniNexus.Models.User;

namespace AniNexus.Web.Client.Services;

/// <summary>
/// A service that authenticates a user.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Attempts to authenticate a user.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    Task<AuthenticationResult> Authenticate(string username, string password)
        => Authenticate(new LoginRequestDTO { Username = username, Password = password });

    /// <summary>
    /// Attempts to authenticate a user.
    /// </summary>
    /// <param name="loginRequest">The request that contains the username and password.</param>
    Task<AuthenticationResult> Authenticate(LoginRequestDTO loginRequest);
}

/// <summary>
/// The result of an authentication request.
/// </summary>
public sealed class AuthenticationResult : LoginResponseDTO
{
    public string Message { get; }

    private readonly ILocalStorageService LocalStorage;

    internal AuthenticationResult(string error, ILocalStorageService localStorageService)
    {
        Code = ELoginResult.GenericFailure;
        Error = error;
        User = null;
        Message = error;
        LocalStorage = localStorageService;
    }

    internal AuthenticationResult(LoginResponseDTO other, ILocalStorageService localStorage)
    {
        Code = other.Code;
        Error = other.Error;
        User = other.User;
        Message = other.Error ?? (Code == ELoginResult.Success ? string.Empty : "An error has occurred.");
        LocalStorage = localStorage;
    }

    public async ValueTask Commit()
    {
        if (!string.IsNullOrWhiteSpace(User?.Token))
        {
            await LocalStorage.SetAccessTokenAsync(User.Token);
        }
    }
}

internal class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientService HttpService;
    private readonly ILocalStorageService LocalStorage;

    public AuthenticationService(IHttpClientService httpClientService, ILocalStorageService localStorageService)
    {
        HttpService = httpClientService;
        LocalStorage = localStorageService;
    }

    public async Task<AuthenticationResult> Authenticate(LoginRequestDTO loginRequest)
    {
        var http = await HttpService.GetHttpClientAsync();
        using var response = await http.PostAsJsonAsync("/api/account/authenticate/login", loginRequest);

        var responseDTO = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
        if (responseDTO is null)
        {
            return new AuthenticationResult("An empty response was received by the server. Please try again later.", LocalStorage);
        }

        return new AuthenticationResult(responseDTO, LocalStorage);
    }
}
