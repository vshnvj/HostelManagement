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


create table Rooms
(
Room_no int primary key,
Capacity int,
Type varchar(30),
Rent float
);



create table Roles
( 
Role_id int primary key,
Role varchar(20)
);




insert into Roles values(1,"Owner");
insert into Roles values(0,"Guest");

-------------------------------


