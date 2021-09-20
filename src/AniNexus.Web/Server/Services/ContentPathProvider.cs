using System.IO;
using AniNexus.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Services;

public interface IContentPathProvider
{
    DirectoryInfo Root { get; }

    ValueTask<Uri> GetCdnRootAsync(CancellationToken cancellationToken = default);
    ValueTask<ContentPath> GetCdnResourceAsync(string resourceKey, CancellationToken cancellationToken = default);
}

public readonly struct ContentPath
{
    public readonly string CdnRoot { get; }
    public readonly string Resource { get; }

    internal ContentPath(string cdnRoot, string resourceFormat)
    {
        CdnRoot = cdnRoot;
        Resource = resourceFormat;
    }

    public override readonly string ToString()
    {
        return CdnRoot + Resource;
    }

    public readonly string ToString(object? arg)
    {
        // Another allocation, but whatever.
        return string.Format(CdnRoot + Resource, arg);
    }
}

internal class ContentPathProvider : IContentPathProvider
{
    public DirectoryInfo Root { get; }

    private readonly IDbContextFactory<ApplicationDbContext> DbContextFactory;

    public ContentPathProvider(IWebHostEnvironment webEnvironment, IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        DbContextFactory = dbContextFactory;

        Root = new DirectoryInfo(webEnvironment.WebRootPath);
    }

    public async ValueTask<Uri> GetCdnRootAsync(CancellationToken cancellationToken = default)
    {
        using var context = DbContextFactory.CreateDbContext();

        string? cdnRoot = await context.GetApplicationResourceAsync(ApplicationResource.ContentHostKey, cancellationToken);
        if (string.IsNullOrWhiteSpace(cdnRoot))
        {
            throw new InvalidOperationException("Unable to acquire cover art path resource.");
        }

        return new Uri(cdnRoot);
    }

    public async ValueTask<ContentPath> GetCdnResourceAsync(string resourceKey, CancellationToken cancellationToken = default)
    {
        var cdnRoot = await GetCdnRootAsync(cancellationToken);

        using var context = DbContextFactory.CreateDbContext();

        string? resource = await context.GetApplicationResourceAsync(resourceKey, cancellationToken);
        if (string.IsNullOrWhiteSpace(resource))
        {
            throw new InvalidOperationException($"Unable to acquire resource '{resourceKey}'.");
        }

        return new ContentPath(cdnRoot.ToString(), resource);
    }
}
