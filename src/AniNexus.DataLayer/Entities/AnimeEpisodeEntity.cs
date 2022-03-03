namespace AniNexus.Data.Entities;

/// <summary>
/// Models an episode of an anime.
/// </summary>
/// <remarks>
/// An episode may also refer to a feature length film.
/// </remarks>
public sealed class AnimeEpisodeEntity : InstallmentEntity<AnimeEpisodeEntity, AnimeReleaseEntity>
{
    /// <summary>
    /// Whether the episode is a ".5" episode.
    /// </summary>
    public bool IsEpisodeNumberPoint5 { get; set; }

    /// <summary>
    /// Whether the episode is a recap episode.
    /// </summary>
    public bool IsRecap { get; set; }

    /// <summary>
    /// The length of the episode.
    /// </summary>
    public TimeSpan? Duration { get; set; }

    /// <summary>
    /// The Id of the opening song.
    /// </summary>
    public Guid? OpeningSongId { get; set; }

    /// <summary>
    /// The Id of the ending song.
    /// </summary>
    public Guid? EndingSongId { get; set; }

    /// <summary>
    /// The OP.
    /// </summary>
    public SongEntity? OpeningSong { get; set; }

    /// <summary>
    /// The ED.
    /// </summary>
    public SongEntity? EndingSong { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeEpisodeEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => new object[] { m.Number, m.ReleaseId, m.IsEpisodeNumberPoint5 }).IsUnique();
        builder.HasIndex(m => m.OpeningSongId).HasNotNullFilter();
        builder.HasIndex(m => m.EndingSongId).HasNotNullFilter();
        // 2. Navigation properties
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.OpeningSong).WithMany(m => m.EpisodesOP).HasForeignKey(m => m.OpeningSongId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.EndingSong).WithMany(m => m.EpisodesED).HasForeignKey(m => m.EndingSongId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        // 3. Propery specification
        builder.Property(m => m.IsEpisodeNumberPoint5).HasComment("Whether the episode is a .5 release.");
        builder.Property(m => m.IsRecap).HasComment("Whether the episode is a recap episode.");
        builder.Property(m => m.Duration).HasComment("The length of the episode.");
        builder.Property(m => m.OpeningSongId).HasColumnName("OPId");
        builder.Property(m => m.EndingSongId).HasColumnName("EDId");
        // 4. Other
    }
}
