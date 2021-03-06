if exists(select 1 from sys.databases where name = 'Chinook') DROP DATABASE Chinook
GO

CREATE DATABASE Chinook
GO

USE [Chinook]
GO

/****** Object:  Table [dbo].[hibernate_unique_key]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[hibernate_unique_key]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[hibernate_unique_key](
	[next_hi] [int] NULL
)
END
GO
/****** Object:  Table [dbo].[MediaType]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MediaType](
	[MediaTypeId] [int] NOT NULL,
	[Name] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
 CONSTRAINT [PK__MediaType__0AD2A005] PRIMARY KEY CLUSTERED 
(
	[MediaTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Playlist]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Playlist]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Playlist](
	[PlaylistId] [int] NOT NULL,
	[Name] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Playlist__060DEAE8] PRIMARY KEY CLUSTERED 
(
	[PlaylistId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Artist]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Artist]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Artist](
	[ArtistId] [int] NOT NULL,
	[Name] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Artist__08EA5793] PRIMARY KEY CLUSTERED 
(
	[ArtistId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] NOT NULL,
	[LastName] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[FirstName] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Title] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[BirthDate] [datetime] NULL,
	[HireDate] [datetime] NULL,
	[Address] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[City] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[State] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Country] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[PostalCode] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Phone] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Fax] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Email] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[ReportsTo] [int] NULL,
 CONSTRAINT [PK__Employee__00551192] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Genre]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Genre]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Genre](
	[GenreId] [int] NOT NULL,
	[Name] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
 CONSTRAINT [PK__Genre__7E6CC920] PRIMARY KEY CLUSTERED 
(
	[GenreId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Album]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Album]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Album](
	[AlbumId] [int] NOT NULL,
	[Title] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[ArtistId] [int] NULL,
 CONSTRAINT [PK__Album__0425A276] PRIMARY KEY CLUSTERED 
(
	[AlbumId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Track]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Track]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Track](
	[TrackId] [int] NOT NULL,
	[Name] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[AlbumId] [int] NULL,
	[MediaTypeId] [int] NULL,
	[GenreId] [int] NULL,
	[Composer] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Milliseconds] [int] NULL,
	[Bytes] [int] NULL,
	[UnitPrice] [numeric](10, 2) NULL,
 CONSTRAINT [PK__Track__0EA330E9] PRIMARY KEY CLUSTERED 
(
	[TrackId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[PlaylistTrack]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PlaylistTrack]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PlaylistTrack](
	[PlaylistId] [int] NOT NULL,
	[TrackId] [int] NOT NULL
)
END
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] NOT NULL,
	[FirstName] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[LastName] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Company] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Address] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[City] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[State] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Country] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[PostalCode] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Phone] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Fax] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Email] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[SupportRepId] [int] NULL,
 CONSTRAINT [PK__Customer__0CBAE877] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Invoice]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Invoice](
	[InvoiceId] [int] NOT NULL,
	[CustomerId] [int] NULL,
	[InvoiceDate] [datetime] NULL,
	[BillingAddress] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[BillingCity] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[BillingState] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[BillingCountry] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[BillingPostalCode] [nvarchar](255) COLLATE Traditional_Spanish_CI_AS NULL,
	[Total] [numeric](10, 2) NULL,
 CONSTRAINT [PK__Invoice__7C8480AE] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  Table [dbo].[InvoiceLine]    Script Date: 03/30/2010 22:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvoiceLine]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[InvoiceLine](
	[InvoiceLineId] [int] NOT NULL,
	[InvoiceId] [int] NULL,
	[TrackId] [int] NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [numeric](10, 2) NULL,
 CONSTRAINT [PK__InvoiceLine__023D5A04] PRIMARY KEY CLUSTERED 
(
	[InvoiceLineId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO
/****** Object:  ForeignKey [FKA0CE20AA71897002]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FKA0CE20AA71897002]') AND parent_object_id = OBJECT_ID(N'[dbo].[Album]'))
ALTER TABLE [dbo].[Album]  WITH CHECK ADD  CONSTRAINT [FKA0CE20AA71897002] FOREIGN KEY([ArtistId])
REFERENCES [dbo].[Artist] ([ArtistId])
GO
ALTER TABLE [dbo].[Album] CHECK CONSTRAINT [FKA0CE20AA71897002]
GO
/****** Object:  ForeignKey [FKFE9A39C09970892A]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FKFE9A39C09970892A]') AND parent_object_id = OBJECT_ID(N'[dbo].[Customer]'))
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FKFE9A39C09970892A] FOREIGN KEY([SupportRepId])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FKFE9A39C09970892A]
GO
/****** Object:  ForeignKey [FK20E4895F4F019B89]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK20E4895F4F019B89]') AND parent_object_id = OBJECT_ID(N'[dbo].[Employee]'))
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK20E4895F4F019B89] FOREIGN KEY([ReportsTo])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK20E4895F4F019B89]
GO
/****** Object:  ForeignKey [FKDC6081720C5DAD6]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FKDC6081720C5DAD6]') AND parent_object_id = OBJECT_ID(N'[dbo].[Invoice]'))
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FKDC6081720C5DAD6] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FKDC6081720C5DAD6]
GO
/****** Object:  ForeignKey [FK64ABF9253AAD7382]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK64ABF9253AAD7382]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceLine]'))
ALTER TABLE [dbo].[InvoiceLine]  WITH CHECK ADD  CONSTRAINT [FK64ABF9253AAD7382] FOREIGN KEY([TrackId])
REFERENCES [dbo].[Track] ([TrackId])
GO
ALTER TABLE [dbo].[InvoiceLine] CHECK CONSTRAINT [FK64ABF9253AAD7382]
GO
/****** Object:  ForeignKey [FK64ABF9256A861DFE]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK64ABF9256A861DFE]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceLine]'))
ALTER TABLE [dbo].[InvoiceLine]  WITH CHECK ADD  CONSTRAINT [FK64ABF9256A861DFE] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoice] ([InvoiceId])
GO
ALTER TABLE [dbo].[InvoiceLine] CHECK CONSTRAINT [FK64ABF9256A861DFE]
GO
/****** Object:  ForeignKey [FKFDB1981B33DC0761]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FKFDB1981B33DC0761]') AND parent_object_id = OBJECT_ID(N'[dbo].[PlaylistTrack]'))
ALTER TABLE [dbo].[PlaylistTrack]  WITH CHECK ADD  CONSTRAINT [FKFDB1981B33DC0761] FOREIGN KEY([PlaylistId])
REFERENCES [dbo].[Playlist] ([PlaylistId])
GO
ALTER TABLE [dbo].[PlaylistTrack] CHECK CONSTRAINT [FKFDB1981B33DC0761]
GO
/****** Object:  ForeignKey [FKFDB1981B3AAD7382]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FKFDB1981B3AAD7382]') AND parent_object_id = OBJECT_ID(N'[dbo].[PlaylistTrack]'))
ALTER TABLE [dbo].[PlaylistTrack]  WITH CHECK ADD  CONSTRAINT [FKFDB1981B3AAD7382] FOREIGN KEY([TrackId])
REFERENCES [dbo].[Track] ([TrackId])
GO
ALTER TABLE [dbo].[PlaylistTrack] CHECK CONSTRAINT [FKFDB1981B3AAD7382]
GO
/****** Object:  ForeignKey [FK5FEAAD4059AC03F0]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK5FEAAD4059AC03F0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Track]'))
ALTER TABLE [dbo].[Track]  WITH CHECK ADD  CONSTRAINT [FK5FEAAD4059AC03F0] FOREIGN KEY([AlbumId])
REFERENCES [dbo].[Album] ([AlbumId])
GO
ALTER TABLE [dbo].[Track] CHECK CONSTRAINT [FK5FEAAD4059AC03F0]
GO
/****** Object:  ForeignKey [FK5FEAAD406F31F4C0]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK5FEAAD406F31F4C0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Track]'))
ALTER TABLE [dbo].[Track]  WITH CHECK ADD  CONSTRAINT [FK5FEAAD406F31F4C0] FOREIGN KEY([GenreId])
REFERENCES [dbo].[Genre] ([GenreId])
GO
ALTER TABLE [dbo].[Track] CHECK CONSTRAINT [FK5FEAAD406F31F4C0]
GO
/****** Object:  ForeignKey [FK5FEAAD40CC29227D]    Script Date: 03/30/2010 22:47:03 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK5FEAAD40CC29227D]') AND parent_object_id = OBJECT_ID(N'[dbo].[Track]'))
ALTER TABLE [dbo].[Track]  WITH CHECK ADD  CONSTRAINT [FK5FEAAD40CC29227D] FOREIGN KEY([MediaTypeId])
REFERENCES [dbo].[MediaType] ([MediaTypeId])
GO
ALTER TABLE [dbo].[Track] CHECK CONSTRAINT [FK5FEAAD40CC29227D]
GO
