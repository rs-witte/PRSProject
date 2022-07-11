IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Users] (
    [ID] int NOT NULL IDENTITY,
    [Username] nvarchar(30) NOT NULL,
    [Password] nvarchar(30) NOT NULL,
    [Firstname] nvarchar(30) NOT NULL,
    [Lastname] nvarchar(30) NOT NULL,
    [Phone] nvarchar(12) NULL,
    [Email] nvarchar(255) NULL,
    [IsReviewer] bit NOT NULL,
    [IsAdmin] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [Vendors] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(30) NOT NULL,
    [Name] nvarchar(30) NOT NULL,
    [Address] nvarchar(30) NOT NULL,
    [City] nvarchar(30) NOT NULL,
    [State] nvarchar(2) NOT NULL,
    [Zip] nvarchar(5) NOT NULL,
    [Phone] nvarchar(12) NULL,
    [Email] nvarchar(255) NULL,
    CONSTRAINT [PK_Vendors] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Requests] (
    [Id] int NOT NULL IDENTITY,
    [Description] nvarchar(80) NOT NULL,
    [Justification] nvarchar(80) NOT NULL,
    [RejectionReason] nvarchar(80) NULL,
    [DeliveryMode] nvarchar(20) NOT NULL,
    [Status] nvarchar(10) NOT NULL,
    [Total] decimal(11,2) NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Requests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Requests_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([ID]) ON DELETE CASCADE
);
GO

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [PartNbr] nvarchar(30) NOT NULL,
    [Name] nvarchar(30) NOT NULL,
    [Price] decimal(11,2) NOT NULL,
    [Unit] nvarchar(30) NOT NULL,
    [PhotoPath] nvarchar(255) NOT NULL,
    [VendorId] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Vendors_VendorId] FOREIGN KEY ([VendorId]) REFERENCES [Vendors] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [RequestLines] (
    [ID] int NOT NULL IDENTITY,
    [RequestID] int NOT NULL,
    [ProductID] int NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_RequestLines] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_RequestLines_Products_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RequestLines_Requests_RequestID] FOREIGN KEY ([RequestID]) REFERENCES [Requests] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Products_PartNbr] ON [Products] ([PartNbr]);
GO

CREATE INDEX [IX_Products_VendorId] ON [Products] ([VendorId]);
GO

CREATE INDEX [IX_RequestLines_ProductID] ON [RequestLines] ([ProductID]);
GO

CREATE INDEX [IX_RequestLines_RequestID] ON [RequestLines] ([RequestID]);
GO

CREATE INDEX [IX_Requests_UserId] ON [Requests] ([UserId]);
GO

CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);
GO

CREATE UNIQUE INDEX [IX_Vendors_Code] ON [Vendors] ([Code]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220520183935_Initial DB Build Attempt', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'PhotoPath');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Products] ALTER COLUMN [PhotoPath] nvarchar(255) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220601134353_Updated PhotoPath Setting', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Requests]') AND [c].[name] = N'Total');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Requests] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Requests] ADD DEFAULT 0.0 FOR [Total];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Requests]') AND [c].[name] = N'Status');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Requests] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Requests] ADD DEFAULT N'NEW' FOR [Status];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Requests]') AND [c].[name] = N'DeliveryMode');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Requests] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Requests] ADD DEFAULT N'Pickup' FOR [DeliveryMode];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RequestLines]') AND [c].[name] = N'Quantity');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [RequestLines] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [RequestLines] ADD DEFAULT 1 FOR [Quantity];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220602112924_reconfigured default values Requests and RequestLines', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220615125913_apply changes to DB table properties', N'6.0.5');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220615130300_Change made to Status on Request', N'6.0.5');
GO

COMMIT;
GO

