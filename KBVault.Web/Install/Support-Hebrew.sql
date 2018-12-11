/******  Hebrew Support ******/
ALTER DATABASE kbvault
COLLATE Hebrew_100_CI_AI

/****** Object:  Table [dbo].[Article] Hebrew Support ******/
ALTER TABLE Article
ALTER COLUMN SefName nvarchar(100);

/****** Object:  Table [dbo].[Attachment] Hebrew Support ******/
ALTER TABLE Attachment
ALTER COLUMN MimeType nvarchar(50)

/****** Object:  Table [dbo].[Category] Hebrew Support ******/
ALTER TABLE Category
ALTER COLUMN SefName nvarchar(100)

