using Microsoft.JSInterop;

namespace AniNexus.Web.Client.Services
{
    /// <summary>
    /// A logger that writes to the browser console.
    /// </summary>
    public class JSConsoleLogger
    {
        private readonly IJSRuntime JSRuntime;

        public JSConsoleLogger(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        public ValueTask Debug(string message)
            => JSRuntime.InvokeVoidAsync("console.debug", $"DEBUG: {message}");

        public ValueTask Info(string message)
            => JSRuntime.InvokeVoidAsync("console.log", $"INFO: {message}");

        public ValueTask Warning(string message)
            => JSRuntime.InvokeVoidAsync("console.warn", $"WARN: {message}");

        public ValueTask Error(string message)
            => JSRuntime.InvokeVoidAsync("console.error", $"ERROR: {message}");
    }
}
