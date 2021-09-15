using AniNexus.Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an episode of an anime.
/// </summary>
/// <remarks>
/// An episode may also refer to a feature length film.
/// </remarks>
public class AnimeEpisodeModel : IHasAudit, IHasRowVersion, IEntityTypeConfiguration<AnimeEpisodeModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the episode.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The release the episode is associated with.
    /// </summary>
    /// <seealso cref="AnimeReleaseModel"/>
    public int ReleaseId { get; set; }

    /// <summary>
    /// The episode number.
    /// </summary>
    public int EpisodeNumber { get; set; }

    /// <summary>
    /// Whether the episode is a ".5" episode.
    /// </summary>
    public bool IsEpisodeNumberPoint5 { get; set; }

    /// <summary>
    /// The name of the episode in its native language.
    /// </summary>
    public string? NativeEpisodeName { get; set; }

    /// <summary>
    /// The romanization of <see cref="NativeEpisodeName"/> if the native name is not already in English.
    /// </summary>
    public string? RomajiEpisodeName { get; set; }

    /// <summary>
    /// <see cref="NativeEpisodeName"/> in English if the native name is not already in English.
    /// </summary>
    public string? EnglishEpisodeName { get; set; }

    /// <summary>
    /// Whether the name is a spoiler.
    /// </summary>
    public bool IsSpoiler { get; set; }

    /// <summary>
    /// The length of the episode.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// The date the episode aired.
    /// </summary>
    public DateOnly? ReleaseDate { get; set; }

#if SONGMODEL
    /// <summary>
    /// The Id of the OP.
    /// </summary>
    ///<seealso cref="SongModel">
    public int? OpeningSongId { get; set; }

    /// <summary>
    /// The Id of the ED.
    /// </summary>
    ///<seealso cref="SongModel">
    public int? EndingSongId { get; set; }
#endif

    /// <summary>
    /// A synopsis of the episode.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// A URL to a place where the user can legally watch the episode.
    /// </summary>
    public string? WatchUrl { get; set; }

    #region Interface Properties
    /// <summary>
    /// The UTC date and time this entry was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entry was last updated.
    /// </summary>
    public DateTime DateUpdated { get; set; }

    /// <summary>
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; } = default!;
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The release this episode belongs to.
    /// </summary>
    public AnimeReleaseModel Release { get; set; } = default!;

#if SONGMODEL
    /// <summary>
    /// The opening song.
    /// </summary>
    public SongModel? OpeningSong { get; set; }

    /// <summary>
    /// The ending song.
    /// </summary>
    public SongModel? EndingSong { get; set; }

    /// <summary>
    /// Songs that were played during the episode.
    /// </summary>
    public IList<SongModel> Songs { get; set; }
#endif
    #endregion

    public void Configure(EntityTypeBuilder<AnimeEpisodeModel> builder)
    {
        builder.ToTable("AnimeEpisode");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.ReleaseId);
        builder.HasIndex(m => m.ReleaseDate).HasFilter("[ReleaseDate] IS NOT NULL");
        builder.HasIndex(m => new { m.ReleaseId, m.EpisodeNumber, m.IsEpisodeNumberPoint5 }).IsUnique();

        builder.HasOne(m => m.Release).WithMany(m => m.Episodes).HasForeignKey(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Restrict);
#if SONGMODEL
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        //builder.HasOne(m => m.OpeningSong).WithMany().HasForeignKey(m => m.OpeningSongId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        //builder.HasOne(m => m.EndingSong).WithMany().HasForeignKey(m => m.EndingSongId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
#endif

        builder.Property(m => m.EpisodeNumber).HasComment("The episode number in the release.");
        builder.Property(m => m.IsEpisodeNumberPoint5).HasComment("Whether the episode is a \".5\" episode.");
        builder.Property(m => m.NativeEpisodeName).HasComment("The native name of the episode.").IsSparse();
        builder.Property(m => m.RomajiEpisodeName).HasComment("The romanization of the native name if the name is not already in English.").IsSparse();
        builder.Property(m => m.EnglishEpisodeName).HasComment("The name of the episode in English if the name is not already in English.").IsSparse();
        builder.Property(m => m.Duration).HasComment("The length of the episode.");
        builder.Property(m => m.ReleaseDate).HasComment("The date this episode aired.");
        builder.Property(m => m.Synopsis).HasComment("The episode synopsis.");
        builder.Property(m => m.WatchUrl).HasComment("A URL to a place where the user can legally watch the episode.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EpisodeNumber < 0)
        {
            yield return new ValidationResult("The episode number must be greater than or equal to 0.", new[] { nameof(EpisodeNumber) });
        }

        if (!UriValidator.Validate(WatchUrl, nameof(WatchUrl), out var urlValidationResult))
        {
            yield return urlValidationResult;
        }
    }
}
