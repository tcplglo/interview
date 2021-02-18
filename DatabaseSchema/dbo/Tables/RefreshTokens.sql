CREATE TABLE [dbo].[RefreshTokens] (
    [Id]        NVARCHAR (450) NULL,
    [AppUserId] NVARCHAR (450) NULL,
    [Created]   DATETIME2 (7)  NOT NULL,
    [Modified]  DATETIME2 (7)  NOT NULL,
    [Token]     NVARCHAR (MAX) NULL,
    [Expires]   DATETIME2 (7)  NOT NULL
);

