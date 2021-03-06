using AniNexus.Data.Entities;

namespace AniNexus.Data;

public partial class ApplicationDbContext
{
    /// <summary>
    /// A collection of mappings to albums and the regions they are legally available in.
    /// </summary>
    public DbSet<AlbumRegionMapEntity> AlbumRegions => Set<AlbumRegionMapEntity>();

    /// <summary>
    /// A collection of audio albums.
    /// </summary>
    public DbSet<AlbumEntity> Albums => Set<AlbumEntity>();

    /// <summary>
    /// A collection of mappings to albums and their songs.
    /// </summary>
    public DbSet<AlbumSongMapEntity> AlbumSongMap => Set<AlbumSongMapEntity>();

    /// <summary>
    /// A collection of anime entries.
    /// </summary>
    public DbSet<AnimeEntity> Anime => Set<AnimeEntity>();

    /// <summary>
    /// A collection of mappings to characters and the anime they appear in.
    /// </summary>
    public DbSet<AnimeCharacterMapEntity> AnimeCharacterMap => Set<AnimeCharacterMapEntity>();

    /// <summary>
    /// A collection of anime episodes.
    /// </summary>
    public DbSet<AnimeEpisodeEntity> AnimeEpisodes => Set<AnimeEpisodeEntity>();

    /// <summary>
    /// A collection of users who have favorited an anime.
    /// </summary>
    public DbSet<AnimeFavoriteMapEntity> AnimeFavoriteMap => Set<AnimeFavoriteMapEntity>();

    /// <summary>
    /// A collection of genre mappings to anime.
    /// </summary>
    public DbSet<AnimeGenreMapEntity> AnimeGenreMap => Set<AnimeGenreMapEntity>();

    /// <summary>
    /// A collection of anime list entries.
    /// </summary>
    public DbSet<AnimeListEntryEntity> AnimeListEntry => Set<AnimeListEntryEntity>();

    /// <summary>
    /// A collection of related anime mappings.
    /// </summary>
    public DbSet<AnimeRelatedMapEntity> AnimeRelatedMap => Set<AnimeRelatedMapEntity>();

    /// <summary>
    /// A collection of anime that is airing.
    /// </summary>
    public DbSet<AnimeReleaseAiringEntity> AnimeAiring => Set<AnimeReleaseAiringEntity>();

    /// <summary>
    /// A collection of anime release mappings to the people who worked on them.
    /// </summary>
    public DbSet<AnimeReleasePersonMapEntity> AnimeReleasePersonMap => Set<AnimeReleasePersonMapEntity>();

    /// <summary>
    /// A collection of anime releases.
    /// </summary>
    public DbSet<AnimeReleaseEntity> AnimeReleases => Set<AnimeReleaseEntity>();

    /// <summary>
    /// A collection of tag mappings to anime.
    /// </summary>
    public DbSet<AnimeTagMapEntity> AnimeTagMap => Set<AnimeTagMapEntity>();

    /// <summary>
    /// A collection of tag mappings to anime that are pending enough votes to be applied.
    /// </summary>
    public DbSet<AnimeTagMapPendingEntity> AnimeTagMapPending => Set<AnimeTagMapPendingEntity>();

    /// <summary>
    /// A collection of anime mappings to third party trackers.
    /// </summary>
    public DbSet<AnimeThirdPartyMapEntity> AnimeThirdPartyMap => Set<AnimeThirdPartyMapEntity>();

    /// <summary>
    /// A collection of anime recommendations made by users.
    /// </summary>
    public DbSet<AnimeUserRecommendationEntity> AnimeUserRecommendations => Set<AnimeUserRecommendationEntity>();

    /// <summary>
    /// A collection of votes associated with an anime recommendation.
    /// </summary>
    /// <seealso cref="AnimeUserRecommendations"/>
    public DbSet<AnimeUserRecommendationVoteEntity> AnimeUserRecommendationVotes => Set<AnimeUserRecommendationVoteEntity>();

    /// <summary>
    /// A collection of anime user reviews.
    /// </summary>
    public DbSet<AnimeUserReviewEntity> AnimeUserReviews => Set<AnimeUserReviewEntity>();

    /// <summary>
    /// A collection of anime user reviews that have been reported.
    /// </summary>
    public DbSet<AnimeUserReviewDeletedEntity> AnimeUserReviewsDeleted => Set<AnimeUserReviewDeletedEntity>();

    /// <summary>
    /// A collection of anime user reviews that have been reported.
    /// </summary>
    public DbSet<AnimeUserReviewReportedEntity> AnimeUserReviewsReported => Set<AnimeUserReviewReportedEntity>();

    /// <summary>
    /// A collection of votes for anime reviews.
    /// </summary>
    public DbSet<AnimeUserReviewVoteEntity> AnimeUserReviewVotes => Set<AnimeUserReviewVoteEntity>();

    /// <summary>
    /// A collection of anime recommendations made by AniNexus.
    /// </summary>
    public DbSet<AnimeSystemRecommendationEntity> AnimeSystemRecommendations => Set<AnimeSystemRecommendationEntity>();

    /// <summary>
    /// A collection of application resource strings.
    /// </summary>
    public DbSet<AppResourceEntity> AppResources => Set<AppResourceEntity>();

    /// <summary>
    /// A collection of characters.
    /// </summary>
    public DbSet<CharacterEntity> Characters => Set<CharacterEntity>();

    /// <summary>
    /// A collection of user claims.
    /// </summary>
    public DbSet<ClaimEntity> Claims => Set<ClaimEntity>();

    /// <summary>
    /// A collection of companies.
    /// </summary>
    public DbSet<CompanyEntity> Companies => Set<CompanyEntity>();

    /// <summary>
    /// A collection of mappings to anime that companies are associated with.
    /// </summary>
    public DbSet<CompanyAnimeMapEntity> CompanyAnimeMap => Set<CompanyAnimeMapEntity>();

    /// <summary>
    /// A collection of franchises/IPs.
    /// </summary>
    public DbSet<FranchiseEntity> Franchises => Set<FranchiseEntity>();

    /// <summary>
    /// A collection of known languages.
    /// </summary>
    public DbSet<LanguageEntity> Languages => Set<LanguageEntity>();

    /// <summary>
    /// A collection of music genres.
    /// </summary>
    public DbSet<MusicGenreEntity> MusicGenres => Set<MusicGenreEntity>();

    /// <summary>
    /// A collection of music genres translations.
    /// </summary>
    public DbSet<MusicGenreTranslationEntity> MusicGenreTranslations => Set<MusicGenreTranslationEntity>();

    /// <summary>
    /// A collection of music sub-genres.
    /// </summary>
    public DbSet<MusicSubGenreEntity> MusicSubGenres => Set<MusicSubGenreEntity>();

    /// <summary>
    /// A collection of music sub-genres translations.
    /// </summary>
    public DbSet<MusicSubGenreTranslationEntity> MusicSubGenreTranslations => Set<MusicSubGenreTranslationEntity>();

    /// <summary>
    /// A collection of banned content types per region.
    /// </summary>
    public DbSet<OffensiveBannedContentTypeEntity> OffensiveBannedContentTypes => Set<OffensiveBannedContentTypeEntity>();

    /// <summary>
    /// A collection of content types.
    /// </summary>
    public DbSet<OffensiveContentTypeEntity> OffensiveContentTypes => Set<OffensiveContentTypeEntity>();

    /// <summary>
    /// A collection of content type translations.
    /// </summary>
    public DbSet<OffensiveContentTypeTranslationEntity> OffensiveContentTypesTranslations => Set<OffensiveContentTypeTranslationEntity>();

    /// <summary>
    /// A collection of NSFW content types per region.
    /// </summary>
    public DbSet<OffensiveNsfwContentTypeEntity> OffensiveNsfwContentTypes => Set<OffensiveNsfwContentTypeEntity>();

    /// <summary>
    /// A collection of real people.
    /// </summary>
    public DbSet<PersonEntity> People => Set<PersonEntity>();

    /// <summary>
    /// A collection of mappings to people and albums they worked on.
    /// </summary>
    public DbSet<PersonAlbumMapEntity> PersonAlbumMap => Set<PersonAlbumMapEntity>();

    /// <summary>
    /// A collection of mappings to people and the characters they voiced.
    /// </summary>
    public DbSet<PersonVoiceActorMapEntity> PersonVoiceActorMap => Set<PersonVoiceActorMapEntity>();

    /// <summary>
    /// A collection of mappings to people and the characters they portrayed live.
    /// </summary>
    public DbSet<PersonLiveActorMapEntity> PersonLiveActorMap => Set<PersonLiveActorMapEntity>();

    /// <summary>
    /// A collection of known regions.
    /// </summary>
    public DbSet<RegionEntity> Regions => Set<RegionEntity>();

    /// <summary>
    /// A collection of social media entities.
    /// </summary>
    public DbSet<SocialMediaEntity> SocialMedia => Set<SocialMediaEntity>();

    /// <summary>
    /// A collection of mappings to a band/group and the albums they created.
    /// </summary>
    public DbSet<SongArtistAlbumMapEntity> SongArtistAlbumMap => Set<SongArtistAlbumMapEntity>();

    /// <summary>
    /// A collection of mappings to a band/group and the members.
    /// </summary>
    public DbSet<SongArtistPersonMapEntity> SongArtistPersonMap => Set<SongArtistPersonMapEntity>();

    /// <summary>
    /// A collection of mappings to a band/group and the songs they created.
    /// </summary>
    public DbSet<SongArtistSongMapEntity> SongArtistSongMap => Set<SongArtistSongMapEntity>();

    /// <summary>
    /// A collection of song artists/groups/bands.
    /// </summary>
    public DbSet<SongArtistEntity> SongArtists => Set<SongArtistEntity>();

    /// <summary>
    /// A collection of songs and their composers.
    /// </summary>
    public DbSet<SongPersonComposerMapEntity> SongComposers => Set<SongPersonComposerMapEntity>();

    /// <summary>
    /// A collection of songs.
    /// </summary>
    public DbSet<SongEntity> Songs => Set<SongEntity>();

    /// <summary>
    /// A collection of themes.
    /// </summary>
    public DbSet<ThemeEntity> Themes => Set<ThemeEntity>();

    /// <summary>
    /// A collection of theme translations.
    /// </summary>
    public DbSet<ThemeTranslationEntity> ThemeTranslations => Set<ThemeTranslationEntity>();

    /// <summary>
    /// A collection of third party trackers.
    /// </summary>
    public DbSet<ThirdPartyTrackerEntity> ThirdPartyTrackers => Set<ThirdPartyTrackerEntity>();

    /// <summary>
    /// A collection of twitter hashtags.
    /// </summary>
    //public DbSet<TwitterHashTagEntity> TwitterHashtags => Set<TwitterHashTagEntity>();

    /// <summary>
    /// A collection of user ban histories.
    /// </summary>
    public DbSet<UserBanHistoryEntity> UserBanHistory => Set<UserBanHistoryEntity>();

    /// <summary>
    /// A collection of user claims.
    /// </summary>
    public DbSet<UserClaimEntity> UserClaims => Set<UserClaimEntity>();

    /// <summary>
    /// A collection of user email codes.
    /// </summary>
    public DbSet<UserEmailCodeEntity> UserEmailCodes => Set<UserEmailCodeEntity>();

    /// <summary>
    /// A collection of users.
    /// </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();

    /// <summary>
    /// A collection of top-level user-generated tags (English).
    /// </summary>
    public DbSet<UserTagEntity> UserTags => Set<UserTagEntity>();

    /// <summary>
    /// A collection of user-generated tags that are pending approval.
    /// </summary>
    public DbSet<UserTagPendingEntity> UserTagsPending => Set<UserTagPendingEntity>();

    /// <summary>
    /// A collection of user-generated tags that were rejected.
    /// </summary>
    public DbSet<UserTagRejectedEntity> UserTagsRejected => Set<UserTagRejectedEntity>();

    /// <summary>
    /// A collection of translations for user-generated tags that are pending approval.
    /// </summary>
    public DbSet<UserTagPendingTranslationEntity> UserTagPendingTranslations => Set<UserTagPendingTranslationEntity>();

    /// <summary>
    /// A collection of translations for user-generated tags that were rejected.
    /// </summary>
    public DbSet<UserTagRejectedTranslationEntity> UserTagRejectedTranslations => Set<UserTagRejectedTranslationEntity>();

    /// <summary>
    /// A collection of translations for user-generated tags.
    /// </summary>
    public DbSet<UserTagTranslationEntity> UserTagTranslations => Set<UserTagTranslationEntity>();

    /// <summary>
    /// A collection of video genres.
    /// </summary>
    public DbSet<VideoGenreEntity> VideoGenres => Set<VideoGenreEntity>();

    /// <summary>
    /// A collection of video genre translations.
    /// </summary>
    public DbSet<VideoGenreTranslationEntity> VideoGenreTranslations => Set<VideoGenreTranslationEntity>();
}
