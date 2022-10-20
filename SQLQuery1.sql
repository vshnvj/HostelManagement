------------------------Database= HostelDatabase
create table Rooms
(
Room_no int primary key,
Capacity int,
available int,
Rent float
);



create table Roles
( 
Role_id int primary key,
Role varchar(20)
);


create table Users
(
Id int primary key,
Name varchar(30),
Gender varchar(10),
Mobile varchar(10),
Email varchar(30),
Address varchar(50),
Status int ,
Password varchar(30),
Role_id int foreign key  references Roles(Role_id)

);

create table Allocation
(
Room_no int foreign key  references Rooms(Room_no),

User_id int foreign key  references Users(Id),
Date_of_allocation date
);


create table Payment
(
Id int primary key,
User_id int foreign key references Users(Id),
Amount int,
Date_of_payment date

);
----------------------------
--Insert Into 
--        SELECT 'Vishal',PWDENCRYPT('Cricket@2020')

--        GO


