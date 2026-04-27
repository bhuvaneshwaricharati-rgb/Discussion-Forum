-- ============================================
-- Discussion Forum Database Schema
-- SQL Server / LocalDB
-- ============================================

-- Users Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(500) NOT NULL,
        DisplayName NVARCHAR(100) NOT NULL,
        Bio NVARCHAR(500) NULL DEFAULT '',
        AvatarUrl NVARCHAR(300) NULL DEFAULT '',
        Role NVARCHAR(20) NOT NULL DEFAULT 'User',  -- User, Moderator, Admin
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        LastLoginAt DATETIME NULL
    );
END

-- Threads Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Threads')
BEGIN
    CREATE TABLE Threads (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(200) NOT NULL,
        Body NVARCHAR(MAX) NOT NULL,
        AuthorId INT NOT NULL,
        Status NVARCHAR(20) NOT NULL DEFAULT 'Active', -- Active, Closed, Pinned, Archived
        ViewCount INT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL,
        CONSTRAINT FK_Threads_Users FOREIGN KEY (AuthorId) REFERENCES Users(Id)
    );
END

-- Replies Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Replies')
BEGIN
    CREATE TABLE Replies (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ThreadId INT NOT NULL,
        AuthorId INT NOT NULL,
        Body NVARCHAR(MAX) NOT NULL,
        IsEdited BIT NOT NULL DEFAULT 0,
        LikeCount INT NOT NULL DEFAULT 0,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        UpdatedAt DATETIME NULL,
        CONSTRAINT FK_Replies_Threads FOREIGN KEY (ThreadId) REFERENCES Threads(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Replies_Users FOREIGN KEY (AuthorId) REFERENCES Users(Id)
    );
END

-- Likes Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Likes')
BEGIN
    CREATE TABLE Likes (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ReplyId INT NOT NULL,
        UserId INT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_Likes_Replies FOREIGN KEY (ReplyId) REFERENCES Replies(Id) ON DELETE CASCADE,
        CONSTRAINT FK_Likes_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT UQ_Likes_User_Reply UNIQUE (ReplyId, UserId)
    );
END

-- Audit Logs Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLogs')
BEGIN
    CREATE TABLE AuditLogs (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NULL,
        Action NVARCHAR(100) NOT NULL,
        Details NVARCHAR(500) NULL,
        IpAddress NVARCHAR(50) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
    );
END
