USE [AniNexus]

-- We need to disable PK checks temporarily.

-- Insert some test entries.
IF NOT EXISTS (SELECT 1 FROM [dbo].[Anime] WHERE [Id] = 4096)
BEGIN
	SET IDENTITY_INSERT [dbo].[Anime] ON
	INSERT INTO [dbo].[Anime]
	([Id], [CategoryId], [EnglishName], [RomanjiName], [KanjiName], [EpCount], [Synopsis], [Votes], [Rating])
	VALUES
	(4096, 0, 'Monthly Girls'' Nozaki-kun', 'Gekkan Shoujo Nozaki-kun', N'月刊少女野崎くん', 12, 'High school student Sakura Chiyo has a crush on schoolmate Nozaki Umetarou, but when she confesses her love to him, he mistakes her for a fan and gives her an autograph. When she says that she always wants to be with him, he invites her to his house, but has her help on some drawings. Chiyo discovers that Umetarou is actually a renowned shoujo manga artist named Yumeno Sakiko, and agrees to be his assistant. As they work on his manga Let''s Love, they encounter other schoolmates who assist them or serve as inspirations for characters in the stories.', 0, 0)
	SET IDENTITY_INSERT [dbo].[Anime] OFF

	-- Insert release information
	INSERT INTO [dbo].[AnimeRelease]
	([IsPrimary], [LocaleId], [StartDate], [EndDate], [LatestEpCount], [Stations], [IsAdult], [AnimeId])
	VALUES
	(1, 1, '2014-07-07', '2014-09-22', 12, NULL, 0, 4096),
	(0, 2, NULL, NULL, 0, NULL, 0, 4096)

	-- Insert anime alias.
	INSERT INTO [dbo].[AnimeAlias]
	([AnimeId], [Name])
	VALUES
	(4096, 'gsnk')

	-- Insert content tags.
	-- Due to the nature of how EF adds data via HasData() in seemingly random order,
	-- the tags below may not match up with the actual tags the show has. This is ok
	-- for now since this is simply a test to ensure data behaves correctly.
	INSERT INTO [dbo].[AnimeContentTagMap]
	([AnimeId], [ContentTagId])
	VALUES
	(4096, 1),
	(4096, 2),
	(4096, 3),
	(4096, 4),
	(4096, 5)
END

-- Insert Third Party Information
IF NOT EXISTS (SELECT 1 FROM [dbo].[AnimeThirdPartyMap] WHERE [AnimeId] = 4096 AND [ThirdPartyId] = 1)
BEGIN
	INSERT INTO [dbo].[AnimeThirdPartyMap]
	([AnimeId], [ThirdPartyId], [ExternalAnimeId])
	VALUES
	(4096, 1, '10542')
END