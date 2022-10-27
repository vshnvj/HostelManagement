--  Database= HostelDatabase

CREATE TABLE [dbo].[Users] (
    [Id]       INT identity(1,1)    NOT NULL ,
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


set identity_insert  Users OFF; 

insert into users (Name,Gender,Mobile,Email,Address,Status,Password,Role_id)
values('Admin','Male','9098786578','admin@mail.com','India',0,'admin',1);



CREATE TABLE [dbo].[Payment] (
    [Id]              INT  IDENTITY (1, 1) NOT NULL,
    [User_id]         INT  NULL,
    [Amount]          INT  NULL,
    [Date_of_payment] DATE NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


set identity_insert  Payment OFF; 

CREATE TABLE [dbo].[Rooms] (
    [Room_no]   INT        NOT NULL,
    [Capacity]  INT        NULL,
    [available] INT        NULL,
    [Rent]      FLOAT (53) NULL,
    PRIMARY KEY CLUSTERED ([Room_no] ASC)
);




create table Roles
( 
Role_id int primary key,
Role varchar(20)
);




insert into Roles values(1,"Owner");
insert into Roles values(0,"Guest");

-------------------------------
CREATE TABLE [dbo].[Allocation] (
    [Room_no]            INT  NULL,
    [User_id]            INT  NULL,
    [Date_of_allocation] DATE NULL,
    [Id]                 INT  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Room_no]) REFERENCES [dbo].[Rooms] ([Room_no])
);



