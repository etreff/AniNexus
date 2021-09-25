using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniNexus.Domain.Migrations
{
    public partial class FixIndexKeyDefaults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "TwoFactorEnabled",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether MFA is enabled for this user.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether MFA is enabled for this user.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanned",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the user is banned.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether the user is banned.");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailValidated",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the user has validated their email address.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether the user has validated their email address.");

            migrationBuilder.AlterColumn<int>(
                name: "AccessFailedCount",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "The number of times the user entered incorrect credentials since their last login.",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "The number of times the user entered incorrect credentials since their last login.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "ThirdParty",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "PersonName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Person",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSpoiler",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this tag is a spoiler for an event in the media.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this tag is a spoiler for an event in the media.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsNSFW",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this tag marks an entity as NSFW.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this tag marks an entity as NSFW.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsGore",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this tag marks an entity as containing gore.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this tag marks an entity as containing gore.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MediaSeriesName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this name is the primary name of the series.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this name is the primary name of the series.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MediaSeries",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MediaGenre",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsNSFW",
                table: "MediaGenre",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this tag marks an entity as NSFW.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this tag marks an entity as NSFW.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsGore",
                table: "MediaGenre",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this tag marks an entity as containing gore.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this tag marks an entity as containing gore.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MangaVolumeName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaVolume",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "MangaUserRecVote",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this user agrees with the recommendation.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this user agrees with the recommendation.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaUserRec",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<byte>(
                name: "Order",
                table: "MangaSysRec",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "The order in which the recommendation will be listed. Lower order will be listed first.",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)10,
                oldComment: "The order in which the recommendation will be listed. Lower order will be listed first.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "MangaReviewVote",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this user agrees with the review.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this user agrees with the review.");

            migrationBuilder.AlterColumn<bool>(
                name: "Recommend",
                table: "MangaReview",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the user recommends the manga.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether the user recommends the manga.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaReview",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MangaReleaseName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaRelease",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MangaRelease",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this is the primary release information for the manga.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this is the primary release information for the manga.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Manga",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "GameUserRecVote",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this user agrees with the recommendation.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this user agrees with the recommendation.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "GameUserRec",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<byte>(
                name: "Order",
                table: "GameSysRec",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "The order in which the recommendation will be listed. Lower order will be listed first.",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)10,
                oldComment: "The order in which the recommendation will be listed. Lower order will be listed first.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "GameReviewVote",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this user agrees with the review.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this user agrees with the review.");

            migrationBuilder.AlterColumn<bool>(
                name: "Recommend",
                table: "GameReview",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the user recommends the game.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether the user recommends the game.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "GameReview",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "GameName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Game",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "CompanyName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this name is the primary name.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this name is the primary name.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Company",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Primary",
                table: "CharacterVAMap",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this person was the primary voice actor for this character.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this person was the primary voice actor for this character.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "CharacterName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "Primary",
                table: "CharacterActorMap",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this person was the primary actor for this character.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this person was the primary actor for this character.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Character",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "AnimeUserRecVote",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this user agrees with the recommendation.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this user agrees with the recommendation.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "AnimeUserRec",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<byte>(
                name: "Order",
                table: "AnimeSysRec",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                comment: "The order in which the recommendation will be listed. Lower order will be listed first.",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)10,
                oldComment: "The order in which the recommendation will be listed. Lower order will be listed first.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "AnimeReviewVote",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this user agrees with the review.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this user agrees with the review.");

            migrationBuilder.AlterColumn<bool>(
                name: "Recommend",
                table: "AnimeReview",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the user recommends the anime.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether the user recommends the anime.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "AnimeReview",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "AnimeReleaseName",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "AnimeRelease",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "AnimeRelease",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this is the primary release information for the anime.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this is the primary release information for the anime.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSpoiler",
                table: "AnimeEpisode",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEpisodeNumberPoint5",
                table: "AnimeEpisode",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether the episode is a \".5\" episode.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether the episode is a \".5\" episode.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Anime",
                type: "bit",
                nullable: false,
                defaultValue: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComment: "Whether this entity is soft deleted.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "TwoFactorEnabled",
                table: "User",
                type: "bit",
                nullable: false,
                comment: "Whether MFA is enabled for this user.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether MFA is enabled for this user.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "User",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanned",
                table: "User",
                type: "bit",
                nullable: false,
                comment: "Whether the user is banned.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether the user is banned.");

            migrationBuilder.AlterColumn<bool>(
                name: "EmailValidated",
                table: "User",
                type: "bit",
                nullable: false,
                comment: "Whether the user has validated their email address.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether the user has validated their email address.");

            migrationBuilder.AlterColumn<int>(
                name: "AccessFailedCount",
                table: "User",
                type: "int",
                nullable: false,
                comment: "The number of times the user entered incorrect credentials since their last login.",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0,
                oldComment: "The number of times the user entered incorrect credentials since their last login.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "ThirdParty",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "PersonName",
                type: "bit",
                nullable: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Person",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSpoiler",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                comment: "Whether this tag is a spoiler for an event in the media.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this tag is a spoiler for an event in the media.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsNSFW",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                comment: "Whether this tag marks an entity as NSFW.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this tag marks an entity as NSFW.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsGore",
                table: "MediaTag",
                type: "bit",
                nullable: false,
                comment: "Whether this tag marks an entity as containing gore.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this tag marks an entity as containing gore.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MediaSeriesName",
                type: "bit",
                nullable: false,
                comment: "Whether this name is the primary name of the series.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this name is the primary name of the series.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MediaSeries",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MediaGenre",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsNSFW",
                table: "MediaGenre",
                type: "bit",
                nullable: false,
                comment: "Whether this tag marks an entity as NSFW.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this tag marks an entity as NSFW.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsGore",
                table: "MediaGenre",
                type: "bit",
                nullable: false,
                comment: "Whether this tag marks an entity as containing gore.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this tag marks an entity as containing gore.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MangaVolumeName",
                type: "bit",
                nullable: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaVolume",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "MangaUserRecVote",
                type: "bit",
                nullable: false,
                comment: "Whether this user agrees with the recommendation.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this user agrees with the recommendation.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaUserRec",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<byte>(
                name: "Order",
                table: "MangaSysRec",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)10,
                comment: "The order in which the recommendation will be listed. Lower order will be listed first.",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0,
                oldComment: "The order in which the recommendation will be listed. Lower order will be listed first.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "MangaReviewVote",
                type: "bit",
                nullable: false,
                comment: "Whether this user agrees with the review.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this user agrees with the review.");

            migrationBuilder.AlterColumn<bool>(
                name: "Recommend",
                table: "MangaReview",
                type: "bit",
                nullable: false,
                comment: "Whether the user recommends the manga.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether the user recommends the manga.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaReview",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MangaReleaseName",
                type: "bit",
                nullable: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "MangaRelease",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "MangaRelease",
                type: "bit",
                nullable: false,
                comment: "Whether this is the primary release information for the manga.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this is the primary release information for the manga.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Manga",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "GameUserRecVote",
                type: "bit",
                nullable: false,
                comment: "Whether this user agrees with the recommendation.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this user agrees with the recommendation.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "GameUserRec",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<byte>(
                name: "Order",
                table: "GameSysRec",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)10,
                comment: "The order in which the recommendation will be listed. Lower order will be listed first.",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0,
                oldComment: "The order in which the recommendation will be listed. Lower order will be listed first.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "GameReviewVote",
                type: "bit",
                nullable: false,
                comment: "Whether this user agrees with the review.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this user agrees with the review.");

            migrationBuilder.AlterColumn<bool>(
                name: "Recommend",
                table: "GameReview",
                type: "bit",
                nullable: false,
                comment: "Whether the user recommends the game.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether the user recommends the game.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "GameReview",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "GameName",
                type: "bit",
                nullable: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Game",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "CompanyName",
                type: "bit",
                nullable: false,
                comment: "Whether this name is the primary name.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this name is the primary name.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Company",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Primary",
                table: "CharacterVAMap",
                type: "bit",
                nullable: false,
                comment: "Whether this person was the primary voice actor for this character.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this person was the primary voice actor for this character.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "CharacterName",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Primary",
                table: "CharacterActorMap",
                type: "bit",
                nullable: false,
                comment: "Whether this person was the primary actor for this character.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this person was the primary actor for this character.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Character",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "AnimeUserRecVote",
                type: "bit",
                nullable: false,
                comment: "Whether this user agrees with the recommendation.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this user agrees with the recommendation.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "AnimeUserRec",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<byte>(
                name: "Order",
                table: "AnimeSysRec",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)10,
                comment: "The order in which the recommendation will be listed. Lower order will be listed first.",
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)0,
                oldComment: "The order in which the recommendation will be listed. Lower order will be listed first.");

            migrationBuilder.AlterColumn<bool>(
                name: "Agrees",
                table: "AnimeReviewVote",
                type: "bit",
                nullable: false,
                comment: "Whether this user agrees with the review.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this user agrees with the review.");

            migrationBuilder.AlterColumn<bool>(
                name: "Recommend",
                table: "AnimeReview",
                type: "bit",
                nullable: false,
                comment: "Whether the user recommends the anime.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether the user recommends the anime.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "AnimeReview",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "AnimeReleaseName",
                type: "bit",
                nullable: false,
                comment: "Whether this name is the primary name of the release.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this name is the primary name of the release.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "AnimeRelease",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsPrimary",
                table: "AnimeRelease",
                type: "bit",
                nullable: false,
                comment: "Whether this is the primary release information for the anime.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this is the primary release information for the anime.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSpoiler",
                table: "AnimeEpisode",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsEpisodeNumberPoint5",
                table: "AnimeEpisode",
                type: "bit",
                nullable: false,
                comment: "Whether the episode is a \".5\" episode.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether the episode is a \".5\" episode.");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSoftDeleted",
                table: "Anime",
                type: "bit",
                nullable: false,
                comment: "Whether this entity is soft deleted.",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false,
                oldComment: "Whether this entity is soft deleted.");
        }
    }
}
