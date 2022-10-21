drop table users
CREATE TABLE [dbo].[Users] (
    [Id]       INT       NOT NULL,
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



set identity_insert Users off;

insert into users values(1,'Admin','Male','9098786578','admin@mail.com','India',1,'admin',1);

