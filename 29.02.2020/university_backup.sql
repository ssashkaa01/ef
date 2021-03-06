USE [master]
GO
/****** Object:  Database [University]    Script Date: 20.02.2020 12:20:19 ******/
CREATE DATABASE [University]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'University', FILENAME = N'C:\Users\ssashkoo01\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\University.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'University_log', FILENAME = N'C:\Users\ssashkoo01\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\University.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [University] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [University].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [University] SET ANSI_NULL_DEFAULT ON 
GO
ALTER DATABASE [University] SET ANSI_NULLS ON 
GO
ALTER DATABASE [University] SET ANSI_PADDING ON 
GO
ALTER DATABASE [University] SET ANSI_WARNINGS ON 
GO
ALTER DATABASE [University] SET ARITHABORT ON 
GO
ALTER DATABASE [University] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [University] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [University] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [University] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [University] SET CURSOR_DEFAULT  LOCAL 
GO
ALTER DATABASE [University] SET CONCAT_NULL_YIELDS_NULL ON 
GO
ALTER DATABASE [University] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [University] SET QUOTED_IDENTIFIER ON 
GO
ALTER DATABASE [University] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [University] SET  DISABLE_BROKER 
GO
ALTER DATABASE [University] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [University] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [University] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [University] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [University] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [University] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [University] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [University] SET RECOVERY FULL 
GO
ALTER DATABASE [University] SET  MULTI_USER 
GO
ALTER DATABASE [University] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [University] SET DB_CHAINING OFF 
GO
ALTER DATABASE [University] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [University] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [University] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [University] SET QUERY_STORE = OFF
GO
USE [University]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [University]
GO
/****** Object:  Table [dbo].[Specializations]    Script Date: 20.02.2020 12:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specializations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 20.02.2020 12:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[Address] [nvarchar](50) NULL,
	[SpecializationId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 20.02.2020 12:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AmountOfHour] [int] NOT NULL,
	[SpecializationId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Specializations] ON 

INSERT [dbo].[Specializations] ([Id], [Name]) VALUES (1, N'Filology')
INSERT [dbo].[Specializations] ([Id], [Name]) VALUES (2, N'Psyhology')
INSERT [dbo].[Specializations] ([Id], [Name]) VALUES (3, N'Mathematics')
INSERT [dbo].[Specializations] ([Id], [Name]) VALUES (4, N'Geography')
SET IDENTITY_INSERT [dbo].[Specializations] OFF
SET IDENTITY_INSERT [dbo].[Students] ON 

INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (1, N'Vasiliy1', N'Pupkin1', N'+380992775566', N'Kiev', 1)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (2, N'Vasiliy2', N'Pupkin2', N'+380992775566', N'Kiev', 1)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (3, N'Vasiliy3', N'Pupkin3', N'+380992775566', N'Kiev', 1)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (4, N'Vasiliy4', N'Pupkin4', N'+380992775566', N'Kiev', 2)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (5, N'Vasiliy5', N'Pupkin5', N'+380992775566', N'Kiev', 2)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (6, N'Vasiliy6', N'Pupkin6', N'+380992775566', N'Kiev', 3)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (7, N'Vasiliy7', N'Pupkin7', N'+380992775566', N'Kiev', 4)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (8, N'Vasiliy8', N'Pupkin8', N'+380992775566', N'Kiev', 4)
INSERT [dbo].[Students] ([Id], [Name], [Surname], [Phone], [Address], [SpecializationId]) VALUES (10, N'wqwe', N'asdg', N'0982221144', N'address', 1)
SET IDENTITY_INSERT [dbo].[Students] OFF
SET IDENTITY_INSERT [dbo].[Subjects] ON 

INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (1, N'Predmet1', 120, 1)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (2, N'Predmet2', 100, 1)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (3, N'Predmet5', 90, 1)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (4, N'Predmet8', 120, 1)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (5, N'Predmet6', 110, 2)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (6, N'Predmet7', 100, 2)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (7, N'Predmet3', 50, 3)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (8, N'Predmet9', 120, 3)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (9, N'Predmet4', 80, 4)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (10, N'Predmet10', 120, 4)
INSERT [dbo].[Subjects] ([Id], [Name], [AmountOfHour], [SpecializationId]) VALUES (11, N'Predmet11', 120, 4)
SET IDENTITY_INSERT [dbo].[Subjects] OFF
ALTER TABLE [dbo].[Students]  WITH CHECK ADD FOREIGN KEY([SpecializationId])
REFERENCES [dbo].[Specializations] ([Id])
GO
ALTER TABLE [dbo].[Subjects]  WITH CHECK ADD FOREIGN KEY([SpecializationId])
REFERENCES [dbo].[Specializations] ([Id])
GO
USE [master]
GO
ALTER DATABASE [University] SET  READ_WRITE 
GO
