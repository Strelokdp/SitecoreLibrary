USE master

IF EXISTS(select * from sys.databases where name='SitecoreLibraryDb')
DROP DATABASE SitecoreLibraryDb

CREATE DATABASE SitecoreLibraryDb
go

USE SitecoreLibraryDb;

go

CREATE TABLE [Books](
   [BookID]  INT  IDENTITY (1, 1) NOT NULL,
   [BookName] NVARCHAR(200) NOT NULL,
   [BookQuantity] INT NOT NULL default(0),
    CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED ([BookID] ASC)
)

go

CREATE TABLE [Authors](
   [AuthorID]  INT  IDENTITY (1, 1) NOT NULL,
   [FirstName] NVARCHAR(200) NOT NULL,
   [LastName] NVARCHAR(200) NOT NULL,
	CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED ([AuthorID] ASC)
)

go

CREATE TABLE Books2Authors(
 [BookToAuthorID]  INT  IDENTITY (1, 1) NOT NULL,
 [BookID] INT NOT NULL,
 [AuthorID] INT NOT NULL,
 [TakenByUserID] uniqueidentifier not null,
 [IsTaken] bit not null default (0)

  CONSTRAINT [FK_Books2Authors_BookID] 
	FOREIGN KEY ([BookID])
    REFERENCES [Books] ([BookID]),

  CONSTRAINT [FK_Books2Authors_AuthorID] 
	FOREIGN KEY ([AuthorID])
    REFERENCES [Authors] ([AuthorID]),

	CONSTRAINT UniqueBookAuthor UNIQUE (AuthorID ,BookID)
)

go

Create Procedure [dbo].[GetBooksWithAuthors]  
as  
begin  
   select Books2Authors.BookToAuthorID, Books2Authors.IsTaken, Books2Authors.TakenByUserID, Books.BookID, Books.BookName, Books.BookQuantity, Authors.FirstName, Authors.LastName from Books2Authors
   join Books
   on Books2Authors.BookID = Books.BookID
   join Authors
   on Books2Authors.AuthorID = Authors.AuthorID
End  

go

create procedure [dbo].[AddNewBookWithAuthorDetails]  
(  
   @BookQuantity int,
   @BookName varchar (50),  
   @FirstName varchar (50),
   @LastName varchar (50)
)  
as  
begin  
	begin transaction
	 insert into Books(BookName, BookQuantity) values (@BookName, @BookQuantity)
	 declare @newBookID int = (select SCOPE_IDENTITY())
	 insert into Authors(FirstName, LastName) values (@FirstName, @LastName)
	 declare @newAuthorID int = (select SCOPE_IDENTITY())
	 insert into Books2Authors(bookID, authorID, TakenByUserID) values(@newBookID, @newAuthorID, 'b0954a60-cd75-44ff-b77f-38e2d7e17a89')
	commit transaction
End  
go

create procedure [dbo].[UpdateBookWithAuthorDetails]  
(  
   @Id int,
   @BookName varchar (50),  
   @FirstName varchar (50),  
   @LastName varchar (50),
   @BookQuantity int  
)  
as  

begin
	 declare @BookID int = (select BookID from Books2Authors where BookToAuthorID = @Id)
	 update Books set BookName=@BookName, BookQuantity = @BookQuantity where Books.BookID = @BookID

	 declare @AuthorID int = (select AuthorID from Books2Authors where BookToAuthorID = @Id)
	 update Authors set FirstName = @FirstName, LastName = @LastName where Authors.AuthorID = @AuthorID
End 

go

create procedure [dbo].[DeleteBookWithAuthorByID]  
(  
   @BookToAuthorId int  
)  
as   
begin  
   Delete from Books2Authors where BookToAuthorID = @BookToAuthorId
End
go


CREATE TABLE [dbo].[Applications](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[ApplicationName] [nvarchar](235) NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[Roles](
	[RoleId] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Roles]  WITH CHECK ADD  CONSTRAINT [RoleEntity_Application] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Applications] ([ApplicationId])
GO

ALTER TABLE [dbo].[Roles] CHECK CONSTRAINT [RoleEntity_Application]
GO


CREATE TABLE [dbo].[Profiles](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [nvarchar](max) NOT NULL,
	[PropertyValueStrings] [nvarchar](max) NOT NULL,
	[PropertyValueBinary] [varbinary](max) NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [User_Application] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Applications] ([ApplicationId])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [User_Application]
GO

ALTER TABLE [dbo].[Profiles]  WITH CHECK ADD  CONSTRAINT [ProfileEntity_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[Profiles] CHECK CONSTRAINT [ProfileEntity_User]
GO


CREATE TABLE [dbo].[Memberships](
	[UserId] [uniqueidentifier] NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowsStart] [datetime] NOT NULL,
	[Comment] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Memberships]  WITH CHECK ADD  CONSTRAINT [MembershipEntity_Application] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[Applications] ([ApplicationId])
GO

ALTER TABLE [dbo].[Memberships] CHECK CONSTRAINT [MembershipEntity_Application]
GO

ALTER TABLE [dbo].[Memberships]  WITH CHECK ADD  CONSTRAINT [MembershipEntity_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[Memberships] CHECK CONSTRAINT [MembershipEntity_User]
GO


CREATE TABLE [dbo].[UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [UsersInRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO

ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [UsersInRole_Role]
GO

ALTER TABLE [dbo].[UsersInRoles]  WITH CHECK ADD  CONSTRAINT [UsersInRole_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[UsersInRoles] CHECK CONSTRAINT [UsersInRole_User]
GO

alter table books2authors
add
  CONSTRAINT [FK_Books2Authors_TakenByUserID] 
	FOREIGN KEY ([TakenByUserID])
    REFERENCES [Users] ([UserId])
go

insert into Books( BookName, BookQuantity)
values
('Sword of Destiny', 3),
('Siala Chronicles', 2),
('The Hound of the Baskervilles', 3),
('The Lost World', 4)

insert into Authors( FirstName, LastName)
values
('Andrzej', 'Sapkowski'),
('Alexey', 'Pekhov'),
('Marina', 'Turchaninova'),
('Arthur Conan', 'Doyle')

insert into Applications (ApplicationId, ApplicationName, Description)
values
('089f7148-8e8d-4a4a-86ee-c28da2ac72d4', 'Temp', 'Temp')
go

insert into Users( UserId, ApplicationId, UserName, IsAnonymous, LastActivityDate)
values
('b0954a60-cd75-44ff-b77f-38e2d7e17a89', '089f7148-8e8d-4a4a-86ee-c28da2ac72d4', 'Temp', 0, GETDATE() )
go

insert into Books2Authors([BookID], [AuthorID], [TakenByUserID])
values
(1,1, 'b0954a60-cd75-44ff-b77f-38e2d7e17a89'),
(2,2, 'b0954a60-cd75-44ff-b77f-38e2d7e17a89'),
(3,4, 'b0954a60-cd75-44ff-b77f-38e2d7e17a89'),
(4,4, 'b0954a60-cd75-44ff-b77f-38e2d7e17a89'),
(2,3, 'b0954a60-cd75-44ff-b77f-38e2d7e17a89')
go

Create table [BookHistory]
(
HistoryRecID INT  IDENTITY (1, 1) NOT NULL,
BookID INT NOT NULL,
UserID uniqueidentifier NOT NULL,
TimeTaken datetime 
  CONSTRAINT [FK_BookHistory_BookID] 
	FOREIGN KEY ([BookID])
    REFERENCES [Books] ([BookID]),
  
  CONSTRAINT [FK_BookHistory_UserID] 
	FOREIGN KEY ([UserID])
    REFERENCES [Users] ([UserId])
)

go

create procedure [dbo].[TakeBookWithUser]  
(  
   @BookToAuthorId int,
   @UserId uniqueidentifier
)  
as   
begin
   update Books2Authors set IsTaken=1, TakenByUserID=@UserId where BookToAuthorID = @BookToAuthorId

   declare @BookID int = (select BookID from Books2Authors where BookToAuthorID = @BookToAuthorId)
   insert into BookHistory(UserID, TimeTaken, BookID) values (@UserId, CURRENT_TIMESTAMP, @BookID)

   update Books set BookQuantity -=1 where BookID=@BookID
End
go

create procedure [dbo].[ReturnBookWithUser]  
(  
   @BookToAuthorId int
)  
as   
begin
   update Books2Authors set IsTaken=0 where BookToAuthorID = @BookToAuthorId

   declare @BookID int = (select BookID from Books2Authors where BookToAuthorID = @BookToAuthorId)
   update Books set BookQuantity +=1 where BookID=@BookID
End
go

CREATE PROCEDURE [dbo].[GetBooksHistory]
	
AS
BEGIN
select BookHistory.HistoryRecID, Books.BookName, Books.BookID, users.UserName, BookHistory.TimeTaken from BookHistory
   join Books
   on BookHistory.BookID = Books.BookID
   join Users
   on BookHistory.UserID = Users.UserId
END
go