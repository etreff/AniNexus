using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniNexus.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeAgeRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the AnimeAgeRating enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeAgeRating", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "AnimeCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the AnimeCategory enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeCategory", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "AnimeListStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the AnimeListStatus enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeListStatus", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "AnimeSeason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the AnimeSeason enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeSeason", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "AppResource",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The key of the dictionary."),
                    Value = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The value of the dictionary.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppResource", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Character",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOB = table.Column<DateTime>(type: "date", nullable: true, comment: "The character's date of birth"),
                    Height = table.Column<int>(type: "int", nullable: true, comment: "The height of the character, in centimeters."),
                    Age = table.Column<int>(type: "int", nullable: true, comment: "The age of the character, in months."),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "A description of the character.", collation: "Japanese_CI_AS_KS_WS"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Character", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CharacterRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the CharacterRole enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRole", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOB = table.Column<DateTime>(type: "date", nullable: true, comment: "The date this company was established or founded."),
                    CreationLocation = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The location the company was established or founded."),
                    Description = table.Column<string>(type: "nvarchar(1250)", maxLength: 1250, nullable: true, comment: "A description of the entity.", collation: "Japanese_CI_AS_KS_WS"),
                    IsCircle = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether the entity represents a circle."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the CompanyRole enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRole", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "GameCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the GameCategory enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategory", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "GameListStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the GameListStatus enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameListStatus", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Locale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false, comment: "The name of the locale."),
                    LanguageCode = table.Column<string>(type: "char(17)", unicode: false, fixedLength: true, maxLength: 17, nullable: false, comment: "The i18n language code of the locale."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MangaAgeRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the MangaAgeRating enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaAgeRating", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MangaCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the MangaCategory enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaCategory", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MangaListStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the MangaListStatus enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaListStatus", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MediaGenre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, comment: "The name of the genre, for example \"Action\" or \"Fantasy\"."),
                    IsNSFW = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this tag marks an entity as NSFW."),
                    IsGore = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this tag marks an entity as containing gore."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaGenre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaRelationType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the MediaRelationType enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaRelationType", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "MediaSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Synopsis = table.Column<string>(type: "nvarchar(1250)", maxLength: 1250, nullable: true, comment: "A synopsis of the media series.", collation: "Japanese_CI_AS_KS_WS"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the MediaStatus enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaStatus", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOB = table.Column<DateTime>(type: "date", nullable: true, comment: "The person's date of birth."),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "A description of the person", collation: "Japanese_CI_AS_KS_WS"),
                    BirthPlace = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true, comment: "The place this person was born."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The name of the PersonRole enum value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRole", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ThirdParty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false, comment: "The name of the third party tracker.", collation: "Japanese_CI_AS_KS_WS"),
                    ShortName = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: true, comment: "The short name or alias of the third party tracker.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true, comment: "The romanized name of the third party tracker."),
                    Url = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true, comment: "The URL of the third party tracker's website homepage."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdParty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitterHashTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hashtag = table.Column<string>(type: "varchar(240)", unicode: false, maxLength: 240, nullable: false, comment: "The Twitter hashtag.", collation: "Japanese_CI_AS_KS_WS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterHashTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    Username = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false, comment: "The user's username."),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false, comment: "The user's email address."),
                    EmailValidated = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the user has validated their email address."),
                    PasswordHash = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The user's hashed password."),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false, comment: "Whether MFA is enabled for this user."),
                    TwoFactorKey = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The MFA secret key for this user."),
                    LockoutEnd = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The UTC time until which the user is locked out of their account."),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false, comment: "The number of times the user entered incorrect credentials since their last login."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The claim type."),
                    ClaimValue = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The claim value.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Anime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The URL to the anime's official website."),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "A synopsis or description of the anime.", collation: "Japanese_CI_AS_KS_WS"),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The user rating of the anime (Completed Only), from 0 to 100. Calculated by the system periodically."),
                    ActiveRating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The user rating of the anime (Watching Only), from 0 to 100. Calculated by the system periodically."),
                    Votes = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of votes that contributed to the rating. Calculated by the system periodically."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anime_AnimeCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AnimeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Anime_AnimeSeason_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "AnimeSeason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CharacterName",
                columns: table => new
                {
                    MediaCharacterModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The native name of the character.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    RomajiFirstName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the first name."),
                    RomajiMiddleName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the middle name."),
                    RomajiLastName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the last name."),
                    IsSpoiler = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Whether this name is a spoiler."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterName", x => new { x.MediaCharacterModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_CharacterName_Character_MediaCharacterModelId",
                        column: x => x.MediaCharacterModelId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyName",
                columns: table => new
                {
                    MediaCompanyModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this name is the primary name.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyName", x => new { x.MediaCompanyModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_CompanyName_Company_MediaCompanyModelId",
                        column: x => x.MediaCompanyModelId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRelatedMap",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    RelatedCompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRelatedMap", x => new { x.CompanyId, x.RelatedCompanyId });
                    table.ForeignKey(
                        name: "FK_CompanyRelatedMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyRelatedMap_Company_RelatedCompanyId",
                        column: x => x.RelatedCompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Manga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The URL to the manga's official website."),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "A synopsis or description of the manga.", collation: "Japanese_CI_AS_KS_WS"),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The user rating of the manga (Completed Only), from 0 to 100. Calculated by the system periodically."),
                    ActiveRating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The user rating of the manga (Readonly Only), from 0 to 100. Calculated by the system periodically."),
                    Votes = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of votes that contributed to the rating. Calculated by the system periodically."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manga", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manga_MangaCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MangaCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediaSeriesName",
                columns: table => new
                {
                    MediaSeriesModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    EnglishName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The name in English."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this name is the primary name of the series.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSeriesName", x => new { x.MediaSeriesModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_MediaSeriesName_MediaSeries_MediaSeriesModelId",
                        column: x => x.MediaSeriesModelId,
                        principalTable: "MediaSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The URL to the media's official website."),
                    ReleaseDate = table.Column<string>(type: "char(10)", fixedLength: true, nullable: true, comment: "The date this game was released."),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "A synopsis or description of the media.", collation: "Japanese_CI_AS_KS_WS"),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The user rating of the media, from 0 to 100. Calculated by the system periodically."),
                    Votes = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of votes that contributed to the rating. Calculated by the system periodically."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_GameCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "GameCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_MediaStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "MediaStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonName",
                columns: table => new
                {
                    MediaPersonModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    RomajiFirstName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the first name."),
                    RomajiMiddleName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the middle name."),
                    RomajiLastName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the last name."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this name is the primary name of the release.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonName", x => new { x.MediaPersonModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_PersonName_Person_MediaPersonModelId",
                        column: x => x.MediaPersonModelId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    Table = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The table that was affected."),
                    Action = table.Column<string>(type: "varchar(900)", unicode: false, nullable: false, comment: "The action that was performed on the table."),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: true),
                    AffectedKeys = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The values of the primary keys that were affected."),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date that this entry was added.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Audit_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MediaTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false, comment: "The name of the tag."),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "A short description of the tag.", collation: "Japanese_CI_AS_KS_WS"),
                    IsSpoiler = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this tag is a spoiler for an event in the media."),
                    IsNSFW = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this tag marks an entity as NSFW."),
                    IsGore = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this tag marks an entity as containing gore."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaTag_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserEmailCode",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The code to validate the email address."),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The UTC time until which the code is valid.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmailCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEmailCode_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaimMap",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaimMap", x => new { x.UserId, x.ClaimId });
                    table.ForeignKey(
                        name: "FK_UserClaimMap_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClaimMap_UserClaim_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "UserClaim",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeCharacterMap",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeCharacterMap", x => new { x.AnimeId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_AnimeCharacterMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeCharacterMap_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeCharacterMap_CharacterRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CharacterRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeFavoriteMap",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeFavoriteMap", x => new { x.AnimeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AnimeFavoriteMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeGenreMap",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeGenreMap", x => new { x.AnimeId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_AnimeGenreMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeGenreMap_MediaGenre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "MediaGenre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeListEntry",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    EpisodeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of episodes the user has seen."),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The rating this user gives the anime, from 0 to 100."),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "Comments the user has about the anime.", collation: "Japanese_CI_AS_KS_WS"),
                    RewatchCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of times the user has rewatched the anime."),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true, comment: "The date the user first started watching the anime."),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true, comment: "The date the user watched the final episode of the anime.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeListEntry", x => new { x.UserId, x.AnimeId });
                    table.ForeignKey(
                        name: "FK_AnimeListEntry_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeListEntry_AnimeListStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AnimeListStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeListEntry_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimePersonRoleMap",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimePersonRoleMap", x => new { x.PersonId, x.AnimeId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AnimePersonRoleMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimePersonRoleMap_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimePersonRoleMap_PersonRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "PersonRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimeRelatedMap",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    RelatedAnimeId = table.Column<int>(type: "int", nullable: false),
                    RelationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeRelatedMap", x => new { x.AnimeId, x.RelatedAnimeId });
                    table.ForeignKey(
                        name: "FK_AnimeRelatedMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimeRelatedMap_Anime_RelatedAnimeId",
                        column: x => x.RelatedAnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimeRelatedMap_MediaRelationType_RelatedAnimeId",
                        column: x => x.RelatedAnimeId,
                        principalTable: "MediaRelationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeRelease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    LocaleId = table.Column<int>(type: "int", nullable: false),
                    AgeRatingId = table.Column<int>(type: "int", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this is the primary release information for the anime."),
                    StartDate = table.Column<string>(type: "char(10)", fixedLength: true, nullable: true, comment: "The air date of the first episode in this locale."),
                    EndDate = table.Column<string>(type: "char(10)", fixedLength: true, nullable: true, comment: "The air date of the last episode in this locale."),
                    EpisodeCount = table.Column<short>(type: "smallint", nullable: true, comment: "The expected number of entries in this release."),
                    LatestEpisodeCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0, comment: "The actual number of entries in this release."),
                    AirsOnDay = table.Column<int>(type: "int", nullable: true, comment: "The day of the week this entry airs on. Only relevant for anime with a regular release."),
                    AirTime = table.Column<TimeSpan>(type: "time", nullable: true, comment: "The UTC time this anime normally airs at."),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "A synopsis or description of the anime.", collation: "Japanese_CI_AS_KS_WS"),
                    WebsiteUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The URL to a place where the release can be purchased."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeRelease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeRelease_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeRelease_AnimeAgeRating_AgeRatingId",
                        column: x => x.AgeRatingId,
                        principalTable: "AnimeAgeRating",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AnimeRelease_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimeRelease_MediaStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "MediaStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimeReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the user recommends the anime."),
                    Review = table.Column<string>(type: "varchar(2500)", unicode: false, maxLength: 2500, nullable: false, comment: "The review content.", collation: "Japanese_CI_AS_KS_WS"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeReview_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeReview_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeSysRec",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    AnimeRecId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)10, comment: "The order in which the recommendation will be listed. Lower order will be listed first.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeSysRec", x => new { x.AnimeId, x.AnimeRecId });
                    table.ForeignKey(
                        name: "FK_AnimeSysRec_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimeSysRec_Anime_AnimeRecId",
                        column: x => x.AnimeRecId,
                        principalTable: "Anime",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnimeThirdPartyMap",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    ThirdPartyId = table.Column<int>(type: "int", nullable: false),
                    ExternalMediaId = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The Id that the third party tracker has assigned to the anime entry.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeThirdPartyMap", x => new { x.AnimeId, x.ThirdPartyId });
                    table.ForeignKey(
                        name: "FK_AnimeThirdPartyMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeThirdPartyMap_ThirdParty_ThirdPartyId",
                        column: x => x.ThirdPartyId,
                        principalTable: "ThirdParty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeTwitterHashTagMap",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    TwitterHashTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeTwitterHashTagMap", x => new { x.AnimeId, x.TwitterHashTagId });
                    table.ForeignKey(
                        name: "FK_AnimeTwitterHashTagMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeTwitterHashTagMap_TwitterHashTag_TwitterHashTagId",
                        column: x => x.TwitterHashTagId,
                        principalTable: "TwitterHashTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeUserRec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    AnimeRecommendationId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false, comment: "The reason why this user recommends the anime.", collation: "Japanese_CI_AS_KS_WS"),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeUserRec", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeUserRec_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimeUserRec_Anime_AnimeRecommendationId",
                        column: x => x.AnimeRecommendationId,
                        principalTable: "Anime",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimeUserRec_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaSeriesAnimeMap",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    AnimeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSeriesAnimeMap", x => new { x.SeriesId, x.AnimeId });
                    table.ForeignKey(
                        name: "FK_MediaSeriesAnimeMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaSeriesAnimeMap_MediaSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "MediaSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaCharacterMap",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaCharacterMap", x => new { x.MangaId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_MangaCharacterMap_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaCharacterMap_CharacterRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CharacterRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaCharacterMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaFavoriteMap",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaFavoriteMap", x => new { x.MangaId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MangaFavoriteMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaGenreMap",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaGenreMap", x => new { x.MangaId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MangaGenreMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaGenreMap_MediaGenre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "MediaGenre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaListEntry",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    VolumeCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of volumes the user has read."),
                    ChapterCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of chapters the user has read."),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The rating this user gives the manga, from 0 to 100."),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "Comments the user has about the manga.", collation: "Japanese_CI_AS_KS_WS"),
                    RereadCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of times the user has reread the manga."),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true, comment: "The date the user first started reading the manga."),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true, comment: "The date the user finished reading the manga.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaListEntry", x => new { x.UserId, x.MangaId });
                    table.ForeignKey(
                        name: "FK_MangaListEntry_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaListEntry_MangaListStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "MangaListStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaListEntry_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaPersonRoleMap",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    MangaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaPersonRoleMap", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_MangaPersonRoleMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaPersonRoleMap_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaPersonRoleMap_PersonRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "PersonRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MangaRelatedMap",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    RelatedMangaId = table.Column<int>(type: "int", nullable: false),
                    RelationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaRelatedMap", x => new { x.MangaId, x.RelatedMangaId });
                    table.ForeignKey(
                        name: "FK_MangaRelatedMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MangaRelatedMap_Manga_RelatedMangaId",
                        column: x => x.RelatedMangaId,
                        principalTable: "Manga",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MangaRelatedMap_MediaRelationType_RelatedMangaId",
                        column: x => x.RelatedMangaId,
                        principalTable: "MediaRelationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaRelease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    LocaleId = table.Column<int>(type: "int", nullable: false),
                    AgeRatingId = table.Column<int>(type: "int", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this is the primary release information for the manga."),
                    StartDate = table.Column<string>(type: "char(10)", fixedLength: true, nullable: true, comment: "The air date of the first chapter in this locale."),
                    EndDate = table.Column<string>(type: "char(10)", fixedLength: true, nullable: true, comment: "The air date of the last chapter in this locale."),
                    VolumeCount = table.Column<short>(type: "smallint", nullable: true, comment: "The expected number of volumes in this release."),
                    ChapterCount = table.Column<short>(type: "smallint", nullable: true, comment: "The expected number of chapters in this release."),
                    LatestVolumeCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0, comment: "The actual number of volumes in this release."),
                    LatestChapterCount = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0, comment: "The actual number of chapters in this release."),
                    WebsiteUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The URL to a place where the release can be purchased."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaRelease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaRelease_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MangaRelease_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaRelease_MangaAgeRating_AgeRatingId",
                        column: x => x.AgeRatingId,
                        principalTable: "MangaAgeRating",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MangaRelease_MediaStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "MediaStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MangaReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the user recommends the manga."),
                    Review = table.Column<string>(type: "varchar(2500)", unicode: false, maxLength: 2500, nullable: false, comment: "The review content.", collation: "Japanese_CI_AS_KS_WS"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaReview_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaReview_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaSysRec",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    MangaRecId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)10, comment: "The order in which the recommendation will be listed. Lower order will be listed first.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaSysRec", x => new { x.MangaId, x.MangaRecId });
                    table.ForeignKey(
                        name: "FK_MangaSysRec_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MangaSysRec_Manga_MangaRecId",
                        column: x => x.MangaRecId,
                        principalTable: "Manga",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MangaThirdPartyMap",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    ThirdPartyId = table.Column<int>(type: "int", nullable: false),
                    ExternalMediaId = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The Id that the third party tracker has assigned to the manga entry.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaThirdPartyMap", x => new { x.MangaId, x.ThirdPartyId });
                    table.ForeignKey(
                        name: "FK_MangaThirdPartyMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaThirdPartyMap_ThirdParty_ThirdPartyId",
                        column: x => x.ThirdPartyId,
                        principalTable: "ThirdParty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaTwitterHashTagMap",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    TwitterHashTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaTwitterHashTagMap", x => new { x.MangaId, x.TwitterHashTagId });
                    table.ForeignKey(
                        name: "FK_MangaTwitterHashTagMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaTwitterHashTagMap_TwitterHashTag_TwitterHashTagId",
                        column: x => x.TwitterHashTagId,
                        principalTable: "TwitterHashTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaUserRec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    MangaRecommendationId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false, comment: "The reason why this user recommends the manga.", collation: "Japanese_CI_AS_KS_WS"),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaUserRec", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaUserRec_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MangaUserRec_Manga_MangaRecommendationId",
                        column: x => x.MangaRecommendationId,
                        principalTable: "Manga",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MangaUserRec_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaSeriesMangaMap",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    MangaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSeriesMangaMap", x => new { x.SeriesId, x.MangaId });
                    table.ForeignKey(
                        name: "FK_MediaSeriesMangaMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaSeriesMangaMap_MediaSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "MediaSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyGameMap",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyGameMap", x => new { x.CompanyId, x.GameId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_CompanyGameMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyGameMap_CompanyRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CompanyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyGameMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameCharacterMap",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCharacterMap", x => new { x.GameId, x.CharacterId });
                    table.ForeignKey(
                        name: "FK_GameCharacterMap_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameCharacterMap_CharacterRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CharacterRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameCharacterMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameFavoriteMap",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFavoriteMap", x => new { x.GameId, x.UserId });
                    table.ForeignKey(
                        name: "FK_GameFavoriteMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameGenreMap",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenreMap", x => new { x.GameId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_GameGenreMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenreMap_MediaGenre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "MediaGenre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameListEntry",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true, comment: "The rating this user gives the game, from 0 to 100."),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true, comment: "Comments the user has about the game.", collation: "Japanese_CI_AS_KS_WS"),
                    ReplayCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "The number of times the user has replayed the game."),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true, comment: "The date the user first started playing the game."),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true, comment: "The date the user finished playing the game.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameListEntry", x => new { x.UserId, x.GameId });
                    table.ForeignKey(
                        name: "FK_GameListEntry_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameListEntry_GameListStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "GameListStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameListEntry_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameName",
                columns: table => new
                {
                    GameModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocaleId = table.Column<int>(type: "int", nullable: false),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    EnglishName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The name in English."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this name is the primary name of the release.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameName", x => new { x.GameModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_GameName_Game_GameModelId",
                        column: x => x.GameModelId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameName_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamePersonRoleMap",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePersonRoleMap", x => new { x.PersonId, x.GameId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_GamePersonRoleMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePersonRoleMap_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePersonRoleMap_PersonRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "PersonRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameRelatedMap",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    RelatedGameId = table.Column<int>(type: "int", nullable: false),
                    RelationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRelatedMap", x => new { x.GameId, x.RelatedGameId });
                    table.ForeignKey(
                        name: "FK_GameRelatedMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameRelatedMap_Game_RelatedGameId",
                        column: x => x.RelatedGameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameRelatedMap_MediaRelationType_RelatedGameId",
                        column: x => x.RelatedGameId,
                        principalTable: "MediaRelationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Recommend = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the user recommends the game."),
                    Review = table.Column<string>(type: "varchar(2500)", unicode: false, maxLength: 2500, nullable: false, comment: "The review content.", collation: "Japanese_CI_AS_KS_WS"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameReview_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameReview_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameSysRec",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    GameRecId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)10, comment: "The order in which the recommendation will be listed. Lower order will be listed first.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSysRec", x => new { x.GameId, x.GameRecId });
                    table.ForeignKey(
                        name: "FK_GameSysRec_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameSysRec_Game_GameRecId",
                        column: x => x.GameRecId,
                        principalTable: "Game",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameThirdPartyMap",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    ThirdPartyId = table.Column<int>(type: "int", nullable: false),
                    ExternalMediaId = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The Id that the third party tracker has assigned to the game entry.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameThirdPartyMap", x => new { x.GameId, x.ThirdPartyId });
                    table.ForeignKey(
                        name: "FK_GameThirdPartyMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameThirdPartyMap_ThirdParty_ThirdPartyId",
                        column: x => x.ThirdPartyId,
                        principalTable: "ThirdParty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTrailer",
                columns: table => new
                {
                    GameModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The URL of the trailer or promotional video.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTrailer", x => new { x.GameModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_GameTrailer_Game_GameModelId",
                        column: x => x.GameModelId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTwitterHashTagMap",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TwitterHashTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTwitterHashTagMap", x => new { x.GameId, x.TwitterHashTagId });
                    table.ForeignKey(
                        name: "FK_GameTwitterHashTagMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTwitterHashTagMap_TwitterHashTag_TwitterHashTagId",
                        column: x => x.TwitterHashTagId,
                        principalTable: "TwitterHashTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameUserRec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    GameRecommendationId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: false, comment: "The reason why this user recommends the game.", collation: "Japanese_CI_AS_KS_WS"),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUserRec", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameUserRec_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameUserRec_Game_GameRecommendationId",
                        column: x => x.GameRecommendationId,
                        principalTable: "Game",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GameUserRec_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaSeriesGameMap",
                columns: table => new
                {
                    SeriesId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaSeriesGameMap", x => new { x.SeriesId, x.GameId });
                    table.ForeignKey(
                        name: "FK_MediaSeriesGameMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaSeriesGameMap_MediaSeries_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "MediaSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeTagMap",
                columns: table => new
                {
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeTagMap", x => new { x.AnimeId, x.TagId });
                    table.ForeignKey(
                        name: "FK_AnimeTagMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeTagMap_MediaTag_TagId",
                        column: x => x.TagId,
                        principalTable: "MediaTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameTagMap",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTagMap", x => new { x.GameId, x.TagId });
                    table.ForeignKey(
                        name: "FK_GameTagMap_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTagMap_MediaTag_TagId",
                        column: x => x.TagId,
                        principalTable: "MediaTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaTagMap",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaTagMap", x => new { x.MangaId, x.TagId });
                    table.ForeignKey(
                        name: "FK_MangaTagMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaTagMap_MediaTag_TagId",
                        column: x => x.TagId,
                        principalTable: "MediaTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeEpisode",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseId = table.Column<int>(type: "int", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "int", nullable: false, comment: "The episode number in the release."),
                    IsEpisodeNumberPoint5 = table.Column<bool>(type: "bit", nullable: false, comment: "Whether the episode is a \".5\" episode."),
                    NativeEpisodeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name of the episode.", collation: "Japanese_CI_AS_KS_WS")
                        .Annotation("SqlServer:Sparse", true),
                    RomajiEpisodeName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name if the name is not already in English.")
                        .Annotation("SqlServer:Sparse", true),
                    EnglishEpisodeName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The name of the episode in English if the name is not already in English.")
                        .Annotation("SqlServer:Sparse", true),
                    IsSpoiler = table.Column<bool>(type: "bit", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true, comment: "The length of the episode."),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: true, comment: "The date this episode aired."),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The episode synopsis.", collation: "Japanese_CI_AS_KS_WS"),
                    WatchUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "A URL to a place where the user can legally watch the episode."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeEpisode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeEpisode_AnimeRelease_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "AnimeRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimeReleaseAiring",
                columns: table => new
                {
                    ReleaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeReleaseAiring", x => x.ReleaseId);
                    table.ForeignKey(
                        name: "FK_AnimeReleaseAiring_AnimeRelease_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "AnimeRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeReleaseName",
                columns: table => new
                {
                    AnimeReleaseModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    EnglishName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The name in English."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this name is the primary name of the release.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeReleaseName", x => new { x.AnimeReleaseModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_AnimeReleaseName_AnimeRelease_AnimeReleaseModelId",
                        column: x => x.AnimeReleaseModelId,
                        principalTable: "AnimeRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimeReleaseTrailer",
                columns: table => new
                {
                    AnimeReleaseModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false, comment: "The URL of the trailer or promotional video.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeReleaseTrailer", x => new { x.AnimeReleaseModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_AnimeReleaseTrailer_AnimeRelease_AnimeReleaseModelId",
                        column: x => x.AnimeReleaseModelId,
                        principalTable: "AnimeRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterActorMap",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    AnimeReleaseId = table.Column<int>(type: "int", nullable: false),
                    LocaleId = table.Column<int>(type: "int", nullable: false),
                    Primary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this person was the primary actor for this character.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterActorMap", x => new { x.CharacterId, x.PersonId, x.AnimeReleaseId, x.LocaleId });
                    table.ForeignKey(
                        name: "FK_CharacterActorMap_AnimeRelease_AnimeReleaseId",
                        column: x => x.AnimeReleaseId,
                        principalTable: "AnimeRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterActorMap_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterActorMap_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterActorMap_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterVAMap",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    AnimeReleaseId = table.Column<int>(type: "int", nullable: false),
                    LocaleId = table.Column<int>(type: "int", nullable: false),
                    Primary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this person was the primary voice actor for this character.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterVAMap", x => new { x.CharacterId, x.PersonId, x.AnimeReleaseId, x.LocaleId });
                    table.ForeignKey(
                        name: "FK_CharacterVAMap_AnimeRelease_AnimeReleaseId",
                        column: x => x.AnimeReleaseId,
                        principalTable: "AnimeRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterVAMap_Character_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Character",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterVAMap_Locale_LocaleId",
                        column: x => x.LocaleId,
                        principalTable: "Locale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterVAMap_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAnimeMap",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ReleaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAnimeMap", x => new { x.CompanyId, x.AnimeId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_CompanyAnimeMap_Anime_AnimeId",
                        column: x => x.AnimeId,
                        principalTable: "Anime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAnimeMap_AnimeRelease_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "AnimeRelease",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyAnimeMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAnimeMap_CompanyRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CompanyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnimeReviewVote",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    Agrees = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this user agrees with the review.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeReviewVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimeReviewVote_AnimeReview_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "AnimeReview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeReviewVote_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnimeUserRecVote",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    RecommendationId = table.Column<int>(type: "int", nullable: false),
                    Agrees = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this user agrees with the recommendation.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeUserRecVote", x => new { x.UserId, x.RecommendationId });
                    table.ForeignKey(
                        name: "FK_AnimeUserRecVote_AnimeUserRec_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "AnimeUserRec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimeUserRecVote_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CompanyMangaMap",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ReleaseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyMangaMap", x => new { x.CompanyId, x.MangaId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_CompanyMangaMap_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyMangaMap_CompanyRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "CompanyRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyMangaMap_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyMangaMap_MangaRelease_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "MangaRelease",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MangaReleaseName",
                columns: table => new
                {
                    MangaReleaseModelId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    EnglishName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The name in English."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this name is the primary name of the release.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaReleaseName", x => new { x.MangaReleaseModelId, x.Id });
                    table.ForeignKey(
                        name: "FK_MangaReleaseName_MangaRelease_MangaReleaseModelId",
                        column: x => x.MangaReleaseModelId,
                        principalTable: "MangaRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaVolume",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseId = table.Column<int>(type: "int", nullable: false),
                    VolumeNumber = table.Column<int>(type: "int", nullable: false, comment: "The volume number in the release."),
                    ReleaseDate = table.Column<string>(type: "char(10)", fixedLength: true, nullable: true, comment: "The date this volume released."),
                    PageCount = table.Column<int>(type: "int", nullable: true, comment: "The number of pages the volume has."),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The volume synopsis.", collation: "Japanese_CI_AS_KS_WS"),
                    ReadUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "A URL to a place where the user can legally read the volume."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IsSoftDeleted = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this entity is soft deleted.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaVolume", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaVolume_MangaRelease_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "MangaRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MangaReviewVote",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    Agrees = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this user agrees with the review.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaReviewVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaReviewVote_MangaReview_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "MangaReview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaReviewVote_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MangaUserRecVote",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    RecommendationId = table.Column<int>(type: "int", nullable: false),
                    Agrees = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this user agrees with the recommendation.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaUserRecVote", x => new { x.UserId, x.RecommendationId });
                    table.ForeignKey(
                        name: "FK_MangaUserRecVote_MangaUserRec_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "MangaUserRec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaUserRecVote_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameReviewVote",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    ReviewId = table.Column<int>(type: "int", nullable: false),
                    Agrees = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this user agrees with the review.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameReviewVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameReviewVote_GameReview_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "GameReview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameReviewVote_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GameUserRecVote",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "char(36)", fixedLength: true, nullable: false),
                    RecommendationId = table.Column<int>(type: "int", nullable: false),
                    Agrees = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this user agrees with the recommendation.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUserRecVote", x => new { x.UserId, x.RecommendationId });
                    table.ForeignKey(
                        name: "FK_GameUserRecVote_GameUserRec_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "GameUserRec",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameUserRecVote_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MangaChapter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReleaseId = table.Column<int>(type: "int", nullable: false),
                    VolumeId = table.Column<long>(type: "bigint", nullable: true),
                    ChapterNumber = table.Column<int>(type: "int", nullable: false, comment: "The chapter number in the release."),
                    ReleaseDate = table.Column<string>(type: "char(10)", fixedLength: true, nullable: true, comment: "The date this chapter released."),
                    PageCount = table.Column<int>(type: "int", nullable: true, comment: "The number of pages the chapter has."),
                    Synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The chapter synopsis.", collation: "Japanese_CI_AS_KS_WS"),
                    ReadUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "A URL to a place where the user can legally read the chapter."),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was created."),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false, computedColumnSql: "getutcdate()", comment: "The date the entry was last updated."),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaChapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MangaChapter_MangaRelease_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "MangaRelease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MangaChapter_MangaVolume_VolumeId",
                        column: x => x.VolumeId,
                        principalTable: "MangaVolume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MangaVolumeName",
                columns: table => new
                {
                    MangaVolumeModelId = table.Column<long>(type: "bigint", nullable: false),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    EnglishName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The name in English."),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false, comment: "Whether this name is the primary name of the release.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaVolumeName", x => x.MangaVolumeModelId);
                    table.ForeignKey(
                        name: "FK_MangaVolumeName_MangaVolume_MangaVolumeModelId",
                        column: x => x.MangaVolumeModelId,
                        principalTable: "MangaVolume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MangaChapterName",
                columns: table => new
                {
                    MangaChapterModelId = table.Column<long>(type: "bigint", nullable: false),
                    NativeName = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "The native name.", collation: "Japanese_CI_AS_KS_WS"),
                    RomajiName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The romanization of the native name."),
                    EnglishName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true, comment: "The name in English.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaChapterName", x => x.MangaChapterModelId);
                    table.ForeignKey(
                        name: "FK_MangaChapterName_MangaChapter_MangaChapterModelId",
                        column: x => x.MangaChapterModelId,
                        principalTable: "MangaChapter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AnimeAgeRating",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Everyone" },
                    { 2, "Youth" },
                    { 3, "Teen" },
                    { 4, "OlderTeen" },
                    { 5, "Mature" },
                    { 6, "Adult" }
                });

            migrationBuilder.InsertData(
                table: "AnimeCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 7, "Music" },
                    { 6, "LiveAction" },
                    { 5, "Special" },
                    { 99, "Other" },
                    { 3, "ONA" },
                    { 2, "OVA" },
                    { 1, "TV" },
                    { 4, "Movie" }
                });

            migrationBuilder.InsertData(
                table: "AnimeListStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 7, "WillNeverWatch" },
                    { 6, "Dropped" },
                    { 4, "Rewatching" },
                    { 5, "Paused" },
                    { 2, "Watching" },
                    { 1, "PlanToWatch" },
                    { 3, "Complete" }
                });

            migrationBuilder.InsertData(
                table: "AnimeSeason",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Winter" },
                    { 2, "Spring" },
                    { 3, "Summer" },
                    { 4, "Fall" }
                });

            migrationBuilder.InsertData(
                table: "AppResource",
                columns: new[] { "Name", "Value" },
                values: new object[,]
                {
                    { "PersonPicturePath", "assets/coverart/person/{0}.jpg" },
                    { "CharacterPicturePath", "assets/coverart/character/{0}.jpg" },
                    { "SoundTrackAlbumArtPath", "assets/coverart/ost/{0}.jpg" },
                    { "AnimeCoverArtPath", "assets/coverart/anime/{0}.jpg" },
                    { "ContentHostKey", "https://localhost:5001" },
                    { "MediaSeriesCoverArtPath", "assets/coverart/series/{0}.jpg" }
                });

            migrationBuilder.InsertData(
                table: "CharacterRole",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Main" },
                    { 2, "Supporting" }
                });

            migrationBuilder.InsertData(
                table: "CompanyRole",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Producer" },
                    { 2, "Studio" },
                    { 3, "Publisher" }
                });

            migrationBuilder.InsertData(
                table: "GameCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Digital" },
                    { 2, "VisualNovel" },
                    { 3, "Analog" },
                    { 99, "Other" }
                });

            migrationBuilder.InsertData(
                table: "GameListStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 3, "Complete" },
                    { 7, "WillNeverPlay" }
                });

            migrationBuilder.InsertData(
                table: "GameListStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Dropped" },
                    { 5, "Paused" },
                    { 1, "PlanToPlay" },
                    { 2, "Playing" },
                    { 4, "Replaying" }
                });

            migrationBuilder.InsertData(
                table: "Locale",
                columns: new[] { "Id", "LanguageCode", "Name" },
                values: new object[,]
                {
                    { 1, "ja_JP", "Japanese" },
                    { 2, "en_US", "English" }
                });

            migrationBuilder.InsertData(
                table: "MangaAgeRating",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "OlderTeen" },
                    { 1, "Everyone" },
                    { 2, "Youth" },
                    { 3, "Teen" },
                    { 5, "Mature" }
                });

            migrationBuilder.InsertData(
                table: "MangaCategory",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "OneShot" },
                    { 7, "PictureBook" },
                    { 5, "Doujinshi" },
                    { 99, "Other" },
                    { 3, "LightNovel" },
                    { 2, "Comic" },
                    { 1, "Manga" },
                    { 4, "WebNovel" }
                });

            migrationBuilder.InsertData(
                table: "MangaListStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 7, "WillNeverRead" },
                    { 6, "Dropped" },
                    { 4, "Rereading" },
                    { 5, "Paused" },
                    { 2, "Reading" },
                    { 1, "PlanToRead" },
                    { 3, "Complete" }
                });

            migrationBuilder.InsertData(
                table: "MediaGenre",
                columns: new[] { "Id", "IsGore", "IsNSFW", "IsSoftDeleted", "Name" },
                values: new object[,]
                {
                    { 10, false, false, false, "Shoujo" },
                    { 14, false, false, false, "Music" },
                    { 13, false, false, false, "Fantasy" },
                    { 12, false, false, false, "Mecha" },
                    { 15, false, false, false, "Magic Girl" },
                    { 11, false, false, false, "Sports" },
                    { 9, false, false, false, "Shounen" },
                    { 4, false, false, false, "Daily Life" },
                    { 7, false, false, false, "Romance" },
                    { 6, false, false, false, "Manga" },
                    { 5, false, false, false, "High School" },
                    { 3, false, false, false, "Comedy" },
                    { 2, false, false, false, "Adventure" },
                    { 1, false, false, false, "4-Koma" },
                    { 8, false, false, false, "School" }
                });

            migrationBuilder.InsertData(
                table: "MediaRelationType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 99, "Other" },
                    { 9, "DLC" },
                    { 8, "MusicVideo" },
                    { 6, "AlternativeSetting" },
                    { 7, "AlternativeVersion" },
                    { 4, "ParentStory" },
                    { 3, "SideStory" },
                    { 2, "Sequel" },
                    { 1, "Prequel" },
                    { 5, "Recap" }
                });

            migrationBuilder.InsertData(
                table: "MediaStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 7, "Unknown" },
                    { 6, "OnHaitus" },
                    { 4, "NotReleased" },
                    { 5, "Cancelled" },
                    { 2, "Releasing" },
                    { 1, "Complete" },
                    { 3, "InProduction" }
                });

            migrationBuilder.InsertData(
                table: "PersonRole",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 10, "CharacterDesign" },
                    { 14, "ThemeSongPerformance" },
                    { 13, "Planning" },
                    { 12, "KeyAnimation" },
                    { 11, "OriginalCharacterDesign" },
                    { 15, "Programmer" },
                    { 9, "ScriptWriter" },
                    { 5, "AnimationDirector" },
                    { 7, "EpisodeDirector" },
                    { 6, "SoundDirector" },
                    { 4, "Producer" },
                    { 3, "Director" },
                    { 2, "Editor" },
                    { 1, "OriginalCreator" },
                    { 8, "Storyboard" }
                });

            migrationBuilder.InsertData(
                table: "ThirdParty",
                columns: new[] { "Id", "IsSoftDeleted", "Name", "RomajiName", "ShortName", "Url" },
                values: new object[,]
                {
                    { 6, false, "Anime News Network", null, "ANN", "https://www.animenewsnetwork.com/" },
                    { 1, false, "AniDB", null, null, "https://anidb.net/" },
                    { 2, false, "TheTVDB", null, null, "https://www.thetvdb.com/" },
                    { 3, false, "MyAnimeList", null, "MAL", "https://myanimelist.net/" },
                    { 4, false, "Anime-Planet", null, "AP", "https://www.anime-planet.com/" },
                    { 5, false, "AniList", null, null, "https://anilist.co/" },
                    { 7, false, "AllCinema", null, null, "https://www.allcinema.net/" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anime_CategoryId",
                table: "Anime",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Anime_IsSoftDeleted",
                table: "Anime",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Anime_SeasonId",
                table: "Anime",
                column: "SeasonId",
                filter: "[SeasonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeAgeRating_Name",
                table: "AnimeAgeRating",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCategory_Name",
                table: "AnimeCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCharacterMap_CharacterId",
                table: "AnimeCharacterMap",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeCharacterMap_RoleId",
                table: "AnimeCharacterMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeEpisode_ReleaseDate",
                table: "AnimeEpisode",
                column: "ReleaseDate",
                filter: "[ReleaseDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeEpisode_ReleaseId",
                table: "AnimeEpisode",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeEpisode_ReleaseId_EpisodeNumber_IsEpisodeNumberPoint5",
                table: "AnimeEpisode",
                columns: new[] { "ReleaseId", "EpisodeNumber", "IsEpisodeNumberPoint5" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeFavoriteMap_UserId",
                table: "AnimeFavoriteMap",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeGenreMap_GenreId",
                table: "AnimeGenreMap",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeListEntry_AnimeId",
                table: "AnimeListEntry",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeListEntry_StatusId",
                table: "AnimeListEntry",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeListStatus_Name",
                table: "AnimeListStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimePersonRoleMap_AnimeId",
                table: "AnimePersonRoleMap",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimePersonRoleMap_RoleId",
                table: "AnimePersonRoleMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelatedMap_RelatedAnimeId",
                table: "AnimeRelatedMap",
                column: "RelatedAnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelatedMap_RelationTypeId",
                table: "AnimeRelatedMap",
                column: "RelationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelease_AgeRatingId",
                table: "AnimeRelease",
                column: "AgeRatingId",
                filter: "[AgeRatingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelease_AnimeId",
                table: "AnimeRelease",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelease_EndDate",
                table: "AnimeRelease",
                column: "EndDate",
                filter: "[EndDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelease_IsSoftDeleted",
                table: "AnimeRelease",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelease_LocaleId",
                table: "AnimeRelease",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelease_StartDate",
                table: "AnimeRelease",
                column: "StartDate",
                filter: "[StartDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeRelease_StatusId",
                table: "AnimeRelease",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeReview_AnimeId",
                table: "AnimeReview",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeReview_IsSoftDeleted",
                table: "AnimeReview",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeReview_UserId",
                table: "AnimeReview",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeReviewVote_ReviewId",
                table: "AnimeReviewVote",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeReviewVote_UserId_ReviewId",
                table: "AnimeReviewVote",
                columns: new[] { "UserId", "ReviewId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSeason_Name",
                table: "AnimeSeason",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimeSysRec_AnimeRecId",
                table: "AnimeSysRec",
                column: "AnimeRecId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeTagMap_TagId",
                table: "AnimeTagMap",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeThirdPartyMap_ThirdPartyId",
                table: "AnimeThirdPartyMap",
                column: "ThirdPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeTwitterHashTagMap_TwitterHashTagId",
                table: "AnimeTwitterHashTagMap",
                column: "TwitterHashTagId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeUserRec_AnimeId_UserId",
                table: "AnimeUserRec",
                columns: new[] { "AnimeId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeUserRec_AnimeRecommendationId",
                table: "AnimeUserRec",
                column: "AnimeRecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeUserRec_IsSoftDeleted",
                table: "AnimeUserRec",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeUserRec_UserId",
                table: "AnimeUserRec",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimeUserRecVote_RecommendationId",
                table: "AnimeUserRecVote",
                column: "RecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Action",
                table: "Audit",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Date",
                table: "Audit",
                column: "Date")
                .Annotation("SqlServer:Clustered", true);

            migrationBuilder.CreateIndex(
                name: "IX_Audit_Table",
                table: "Audit",
                column: "Table");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_UserId",
                table: "Audit",
                column: "UserId",
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Character_Age",
                table: "Character",
                column: "Age",
                filter: "[Age] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Character_DOB",
                table: "Character",
                column: "DOB",
                filter: "[DOB] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Character_IsSoftDeleted",
                table: "Character",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterActorMap_AnimeReleaseId",
                table: "CharacterActorMap",
                column: "AnimeReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterActorMap_CharacterId",
                table: "CharacterActorMap",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterActorMap_LocaleId",
                table: "CharacterActorMap",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterActorMap_PersonId",
                table: "CharacterActorMap",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRole_Name",
                table: "CharacterRole",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterVAMap_AnimeReleaseId",
                table: "CharacterVAMap",
                column: "AnimeReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterVAMap_LocaleId",
                table: "CharacterVAMap",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterVAMap_PersonId",
                table: "CharacterVAMap",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_IsSoftDeleted",
                table: "Company",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAnimeMap_AnimeId",
                table: "CompanyAnimeMap",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAnimeMap_ReleaseId",
                table: "CompanyAnimeMap",
                column: "ReleaseId",
                filter: "[ReleaseId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAnimeMap_RoleId",
                table: "CompanyAnimeMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyGameMap_GameId",
                table: "CompanyGameMap",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyGameMap_RoleId",
                table: "CompanyGameMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMangaMap_MangaId",
                table: "CompanyMangaMap",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMangaMap_ReleaseId",
                table: "CompanyMangaMap",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyMangaMap_RoleId",
                table: "CompanyMangaMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRelatedMap_RelatedCompanyId",
                table: "CompanyRelatedMap",
                column: "RelatedCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRole_Name",
                table: "CompanyRole",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_CategoryId",
                table: "Game",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_IsSoftDeleted",
                table: "Game",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ReleaseDate",
                table: "Game",
                column: "ReleaseDate",
                filter: "[ReleaseDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Game_StatusId",
                table: "Game",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCategory_Name",
                table: "GameCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameCharacterMap_CharacterId",
                table: "GameCharacterMap",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCharacterMap_RoleId",
                table: "GameCharacterMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFavoriteMap_UserId",
                table: "GameFavoriteMap",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenreMap_GenreId",
                table: "GameGenreMap",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_GameListEntry_GameId",
                table: "GameListEntry",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameListEntry_StatusId",
                table: "GameListEntry",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_GameListEntry_UserId",
                table: "GameListEntry",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameListStatus_Name",
                table: "GameListStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameName_LocaleId",
                table: "GameName",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePersonRoleMap_GameId",
                table: "GamePersonRoleMap",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePersonRoleMap_RoleId",
                table: "GamePersonRoleMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRelatedMap_RelatedGameId",
                table: "GameRelatedMap",
                column: "RelatedGameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameRelatedMap_RelationTypeId",
                table: "GameRelatedMap",
                column: "RelationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GameReview_GameId",
                table: "GameReview",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameReview_IsSoftDeleted",
                table: "GameReview",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GameReview_UserId",
                table: "GameReview",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameReviewVote_ReviewId",
                table: "GameReviewVote",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_GameReviewVote_UserId_ReviewId",
                table: "GameReviewVote",
                columns: new[] { "UserId", "ReviewId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameSysRec_GameRecId",
                table: "GameSysRec",
                column: "GameRecId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTagMap_TagId",
                table: "GameTagMap",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_GameThirdPartyMap_ThirdPartyId",
                table: "GameThirdPartyMap",
                column: "ThirdPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTwitterHashTagMap_TwitterHashTagId",
                table: "GameTwitterHashTagMap",
                column: "TwitterHashTagId");

            migrationBuilder.CreateIndex(
                name: "IX_GameUserRec_GameId_UserId",
                table: "GameUserRec",
                columns: new[] { "GameId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameUserRec_GameRecommendationId",
                table: "GameUserRec",
                column: "GameRecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_GameUserRec_IsSoftDeleted",
                table: "GameUserRec",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GameUserRec_UserId",
                table: "GameUserRec",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameUserRecVote_RecommendationId",
                table: "GameUserRecVote",
                column: "RecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_Locale_LanguageCode",
                table: "Locale",
                column: "LanguageCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manga_CategoryId",
                table: "Manga",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Manga_IsSoftDeleted",
                table: "Manga",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MangaAgeRating_Name",
                table: "MangaAgeRating",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MangaCategory_Name",
                table: "MangaCategory",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapter_ReleaseId",
                table: "MangaChapter",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapter_ReleaseId_ChapterNumber",
                table: "MangaChapter",
                columns: new[] { "ReleaseId", "ChapterNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MangaChapter_VolumeId",
                table: "MangaChapter",
                column: "VolumeId",
                filter: "[VolumeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MangaCharacterMap_CharacterId",
                table: "MangaCharacterMap",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaCharacterMap_RoleId",
                table: "MangaCharacterMap",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaFavoriteMap_UserId",
                table: "MangaFavoriteMap",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaGenreMap_GenreId",
                table: "MangaGenreMap",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaListEntry_MangaId",
                table: "MangaListEntry",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaListEntry_StatusId",
                table: "MangaListEntry",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaListStatus_Name",
                table: "MangaListStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MangaPersonRoleMap_MangaId",
                table: "MangaPersonRoleMap",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaPersonRoleMap_PersonId",
                table: "MangaPersonRoleMap",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelatedMap_RelatedMangaId",
                table: "MangaRelatedMap",
                column: "RelatedMangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelatedMap_RelationTypeId",
                table: "MangaRelatedMap",
                column: "RelationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelease_AgeRatingId",
                table: "MangaRelease",
                column: "AgeRatingId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelease_EndDate",
                table: "MangaRelease",
                column: "EndDate",
                filter: "[EndDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelease_IsSoftDeleted",
                table: "MangaRelease",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelease_LocaleId",
                table: "MangaRelease",
                column: "LocaleId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelease_MangaId",
                table: "MangaRelease",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelease_StartDate",
                table: "MangaRelease",
                column: "StartDate",
                filter: "[StartDate] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MangaRelease_StatusId",
                table: "MangaRelease",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaReview_IsSoftDeleted",
                table: "MangaReview",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MangaReview_MangaId",
                table: "MangaReview",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaReview_UserId",
                table: "MangaReview",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaReviewVote_ReviewId",
                table: "MangaReviewVote",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaReviewVote_UserId_ReviewId",
                table: "MangaReviewVote",
                columns: new[] { "UserId", "ReviewId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MangaSysRec_MangaRecId",
                table: "MangaSysRec",
                column: "MangaRecId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaTagMap_TagId",
                table: "MangaTagMap",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaThirdPartyMap_ThirdPartyId",
                table: "MangaThirdPartyMap",
                column: "ThirdPartyId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaTwitterHashTagMap_TwitterHashTagId",
                table: "MangaTwitterHashTagMap",
                column: "TwitterHashTagId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaUserRec_IsSoftDeleted",
                table: "MangaUserRec",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MangaUserRec_MangaId_UserId",
                table: "MangaUserRec",
                columns: new[] { "MangaId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_MangaUserRec_MangaRecommendationId",
                table: "MangaUserRec",
                column: "MangaRecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaUserRec_UserId",
                table: "MangaUserRec",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaUserRecVote_RecommendationId",
                table: "MangaUserRecVote",
                column: "RecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaVolume_IsSoftDeleted",
                table: "MangaVolume",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MangaVolume_ReleaseId",
                table: "MangaVolume",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaVolume_ReleaseId_VolumeNumber",
                table: "MangaVolume",
                columns: new[] { "ReleaseId", "VolumeNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaGenre_IsSoftDeleted",
                table: "MediaGenre",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MediaGenre_Name",
                table: "MediaGenre",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaRelationType_Name",
                table: "MediaRelationType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaSeries_IsSoftDeleted",
                table: "MediaSeries",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MediaSeriesAnimeMap_AnimeId",
                table: "MediaSeriesAnimeMap",
                column: "AnimeId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaSeriesGameMap_GameId",
                table: "MediaSeriesGameMap",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaSeriesMangaMap_MangaId",
                table: "MediaSeriesMangaMap",
                column: "MangaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaStatus_Name",
                table: "MediaStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaTag_IsSoftDeleted",
                table: "MediaTag",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_MediaTag_Name",
                table: "MediaTag",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaTag_UserId",
                table: "MediaTag",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_IsSoftDeleted",
                table: "Person",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PersonRole_Name",
                table: "PersonRole",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThirdParty_IsSoftDeleted",
                table: "ThirdParty",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TwitterHashTag_Hashtag",
                table: "TwitterHashTag",
                column: "Hashtag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_IsSoftDeleted",
                table: "User",
                column: "IsSoftDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaimMap_ClaimId",
                table: "UserClaimMap",
                column: "ClaimId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeCharacterMap");

            migrationBuilder.DropTable(
                name: "AnimeEpisode");

            migrationBuilder.DropTable(
                name: "AnimeFavoriteMap");

            migrationBuilder.DropTable(
                name: "AnimeGenreMap");

            migrationBuilder.DropTable(
                name: "AnimeListEntry");

            migrationBuilder.DropTable(
                name: "AnimePersonRoleMap");

            migrationBuilder.DropTable(
                name: "AnimeRelatedMap");

            migrationBuilder.DropTable(
                name: "AnimeReleaseAiring");

            migrationBuilder.DropTable(
                name: "AnimeReleaseName");

            migrationBuilder.DropTable(
                name: "AnimeReleaseTrailer");

            migrationBuilder.DropTable(
                name: "AnimeReviewVote");

            migrationBuilder.DropTable(
                name: "AnimeSysRec");

            migrationBuilder.DropTable(
                name: "AnimeTagMap");

            migrationBuilder.DropTable(
                name: "AnimeThirdPartyMap");

            migrationBuilder.DropTable(
                name: "AnimeTwitterHashTagMap");

            migrationBuilder.DropTable(
                name: "AnimeUserRecVote");

            migrationBuilder.DropTable(
                name: "AppResource");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "CharacterActorMap");

            migrationBuilder.DropTable(
                name: "CharacterName");

            migrationBuilder.DropTable(
                name: "CharacterVAMap");

            migrationBuilder.DropTable(
                name: "CompanyAnimeMap");

            migrationBuilder.DropTable(
                name: "CompanyGameMap");

            migrationBuilder.DropTable(
                name: "CompanyMangaMap");

            migrationBuilder.DropTable(
                name: "CompanyName");

            migrationBuilder.DropTable(
                name: "CompanyRelatedMap");

            migrationBuilder.DropTable(
                name: "GameCharacterMap");

            migrationBuilder.DropTable(
                name: "GameFavoriteMap");

            migrationBuilder.DropTable(
                name: "GameGenreMap");

            migrationBuilder.DropTable(
                name: "GameListEntry");

            migrationBuilder.DropTable(
                name: "GameName");

            migrationBuilder.DropTable(
                name: "GamePersonRoleMap");

            migrationBuilder.DropTable(
                name: "GameRelatedMap");

            migrationBuilder.DropTable(
                name: "GameReviewVote");

            migrationBuilder.DropTable(
                name: "GameSysRec");

            migrationBuilder.DropTable(
                name: "GameTagMap");

            migrationBuilder.DropTable(
                name: "GameThirdPartyMap");

            migrationBuilder.DropTable(
                name: "GameTrailer");

            migrationBuilder.DropTable(
                name: "GameTwitterHashTagMap");

            migrationBuilder.DropTable(
                name: "GameUserRecVote");

            migrationBuilder.DropTable(
                name: "MangaChapterName");

            migrationBuilder.DropTable(
                name: "MangaCharacterMap");

            migrationBuilder.DropTable(
                name: "MangaFavoriteMap");

            migrationBuilder.DropTable(
                name: "MangaGenreMap");

            migrationBuilder.DropTable(
                name: "MangaListEntry");

            migrationBuilder.DropTable(
                name: "MangaPersonRoleMap");

            migrationBuilder.DropTable(
                name: "MangaRelatedMap");

            migrationBuilder.DropTable(
                name: "MangaReleaseName");

            migrationBuilder.DropTable(
                name: "MangaReviewVote");

            migrationBuilder.DropTable(
                name: "MangaSysRec");

            migrationBuilder.DropTable(
                name: "MangaTagMap");

            migrationBuilder.DropTable(
                name: "MangaThirdPartyMap");

            migrationBuilder.DropTable(
                name: "MangaTwitterHashTagMap");

            migrationBuilder.DropTable(
                name: "MangaUserRecVote");

            migrationBuilder.DropTable(
                name: "MangaVolumeName");

            migrationBuilder.DropTable(
                name: "MediaSeriesAnimeMap");

            migrationBuilder.DropTable(
                name: "MediaSeriesGameMap");

            migrationBuilder.DropTable(
                name: "MediaSeriesMangaMap");

            migrationBuilder.DropTable(
                name: "MediaSeriesName");

            migrationBuilder.DropTable(
                name: "PersonName");

            migrationBuilder.DropTable(
                name: "UserClaimMap");

            migrationBuilder.DropTable(
                name: "UserEmailCode");

            migrationBuilder.DropTable(
                name: "AnimeListStatus");

            migrationBuilder.DropTable(
                name: "AnimeReview");

            migrationBuilder.DropTable(
                name: "AnimeUserRec");

            migrationBuilder.DropTable(
                name: "AnimeRelease");

            migrationBuilder.DropTable(
                name: "CompanyRole");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "GameListStatus");

            migrationBuilder.DropTable(
                name: "GameReview");

            migrationBuilder.DropTable(
                name: "GameUserRec");

            migrationBuilder.DropTable(
                name: "MangaChapter");

            migrationBuilder.DropTable(
                name: "Character");

            migrationBuilder.DropTable(
                name: "CharacterRole");

            migrationBuilder.DropTable(
                name: "MediaGenre");

            migrationBuilder.DropTable(
                name: "MangaListStatus");

            migrationBuilder.DropTable(
                name: "PersonRole");

            migrationBuilder.DropTable(
                name: "MediaRelationType");

            migrationBuilder.DropTable(
                name: "MangaReview");

            migrationBuilder.DropTable(
                name: "MediaTag");

            migrationBuilder.DropTable(
                name: "ThirdParty");

            migrationBuilder.DropTable(
                name: "TwitterHashTag");

            migrationBuilder.DropTable(
                name: "MangaUserRec");

            migrationBuilder.DropTable(
                name: "MediaSeries");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "Anime");

            migrationBuilder.DropTable(
                name: "AnimeAgeRating");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "MangaVolume");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AnimeCategory");

            migrationBuilder.DropTable(
                name: "AnimeSeason");

            migrationBuilder.DropTable(
                name: "GameCategory");

            migrationBuilder.DropTable(
                name: "MangaRelease");

            migrationBuilder.DropTable(
                name: "Locale");

            migrationBuilder.DropTable(
                name: "Manga");

            migrationBuilder.DropTable(
                name: "MangaAgeRating");

            migrationBuilder.DropTable(
                name: "MediaStatus");

            migrationBuilder.DropTable(
                name: "MangaCategory");
        }
    }
}
