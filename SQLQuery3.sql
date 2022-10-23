drop table users
CREATE TABLE [dbo].[Users] (
    [Id]       INT     NOT NULL ,
    [Name]     VARCHAR (30) NULL,
    [Gender]   VARCHAR (10) NULL,
    [Mobile]   VARCHAR (10) NULL,
    [Email]    VARCHAR (30) NULL,
    [Address]  VARCHAR (50) NULL,
    [Status]   INT          NULL,
    [Password] VARCHAR (30) NULL,
    [Role_id]  INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);




insert into users (Id,Name,Gender,Mobile,Email,Address,Status,Password,Role_id)
values(2,'Admin','Male','9098786578','admin@mail.com','India',0,'admin',1);

