USE [master]
GO

WHILE EXISTS(select NULL from sys.databases where name='QLCAFE')
BEGIN
    DECLARE @SQL varchar(max)
    SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
    FROM MASTER..SysProcesses
    WHERE DBId = DB_ID(N'QLCAFE') AND SPId <> @@SPId
    EXEC(@SQL)
    DROP DATABASE [QLCAFE]
END
GO


CREATE DATABASE [QLCAFE]
GO

USE [QLCAFE]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--food
--table
--food Category
--Bill
--Bill info

create table tablefood
(
	id int identity primary key,
	name nvarchar(100) not null default N'Bàn chưa có tên',
	status nvarchar(100) not null default N'Trống'--tình trạng bàn : trống hoặc có người ngồi

)
go

create table account 
( 
	displayname nvarchar(100) not null,
	username nvarchar(100) primary key,
	password nvarchar(1000),
	type int not null default 0-- admin =1 , 0 = staff
)
go

create table foodcategory
(
	id int identity primary key,
	name nvarchar(100) not null,
)
go

create table food	
(
	id int identity primary key,
	name nvarchar(100),
	idcategory int not null foreign key references dbo.foodcategory(id) on delete cascade,
	price float not null default 0
)
go

create table bill
(
	id int identity primary key,
	datecheckin date default getdate(),
	datecheckout date,
	idtable int not null foreign key references dbo.tablefood(id),
	status int not null default 0, --tình trạng thanh toán : 1 đã thanh toán , 0 chưa thanh toán
	discount int default 0 
)
go

create table billinfo
(
	id int identity primary key,
	idbill int not null foreign key references bill(id),
	idfood int not null foreign key references dbo.food(id) on delete cascade,
	count int not null default 0
)
go

insert into dbo.account(
	username,
	displayname,
	password,
	type
)
values ( N'khue' ,
	N'khuetm',
	N'1',
	1
)

insert into dbo.account(
	username,
	displayname,
	password,
	type
)
values ( N'staff' ,
	N'staff',
	N'1',
	0
)
go

create proc USP_GetAccountByUserName
@username nvarchar(100)
as
begin 
	select * from dbo.account where username = @username 
end 
go

exec dbo.USP_GetAccountByUserName @username= N'khue' 
go

create proc USP_Login
@username nvarchar(100) , @password nvarchar(100)
as
begin
	select * from account where username = @username and password = @password
end 
go

--Thêm bàn 
declare @i int =0
while @i <= 7
begin
	insert dbo.tablefood (name) values ( N'Bàn '  + CAST(@i as nvarchar(100)))
	set @i = @i+1
end

go


create proc USP_GetTableList
as select * from dbo.tablefood
go


--Thêm bàn 
--declare @i int =0
--while @i <= 12
--begin
--	insert dbo.tablefood (name) values ( N'Bàn '  + CAST(@i as nvarchar(100)))
--	set @i = @i+1
--end
--go

--Thêm category 
insert dbo.foodcategory (name)
values  ( N'Hải Sản' )
insert dbo.foodcategory (name)
values  ( N'Nông sản' )
insert dbo.foodcategory (name)
values  ( N'Đồ chay' )
insert dbo.foodcategory (name)
values  ( N'Rau củ quả' )
insert dbo.foodcategory (name)
values  ( N'Nước' )

--Thêm food
insert dbo.food (name,idcategory,price)
values  ( N'Mực nướng', --name
			1,--idcategory
			15000 --price
)
insert dbo.food (name,idcategory,price)
values  ( N'Nghêu', --name
			1,--idcategory
			6000 --price
)
insert dbo.food (name,idcategory,price)
values  ( N'Cơm trắng ', --name
			2,--idcategory
			6000 --price
)
insert dbo.food (name,idcategory,price)
values  ( N'Đậu hủ chiên ', --name
			3,--idcategory
			13000 --price
)
insert dbo.food (name,idcategory,price)
values  ( N'Dưa leo', --name
			4,--idcategory
			3000 --price
)
insert dbo.food (name,idcategory,price)
values  ( N'7 up', --name
			5,--idcategory
			9000 --price
)
insert dbo.food (name,idcategory,price)
values  ( N'Cà phê', --name
			5,--idcategory
			13000 --price
)



--Thêm bill
insert  dbo.bill (datecheckin, datecheckout, idtable, status)
values ( GETDATE(), --datecheckin
		null,--datecheckout
		3,--idtable,
		0--status
)
	
insert  dbo.bill (datecheckin, datecheckout, idtable, status)
values ( GETDATE(), --datecheckin
		null,--datecheckout
		4,--idtable,
		0--status
)

insert  dbo.bill (datecheckin, datecheckout, idtable, status)
values ( GETDATE(), --datecheckin
		getdate(),--datecheckout
		5,--idtable,
		1--status
)

-- Thêm bill info
insert billinfo (idbill, idfood, count)
values (1 , 1, 2)
	
insert billinfo (idbill, idfood, count)
values (1 , 3, 4)

insert billinfo (idbill, idfood, count)
values (2 , 5, 1)	

insert billinfo (idbill, idfood, count)
values (2 , 1, 2)	
insert billinfo (idbill, idfood, count)
values (3 , 6, 2)	
insert billinfo (idbill, idfood, count)
values (3 , 5, 2)	
insert billinfo (idbill, idfood, count)
values (1 , 5, 1)	
insert billinfo (idbill, idfood, count)
values (2 , 7, 2)	
go


create proc USP_InsertBill
@idtable int 
as
begin
	insert dbo.bill (datecheckin, datecheckout, idtable, status , discount)
	values ( getdate(), null , @idtable, 0, 0)
end
go



create proc USP_InsertBillInfo
@idbill int, @idfood int , @count int
as
begin 
	declare @isExitsBillInfo int
	declare @foodCount int =1

	select @isExitsBillInfo = id , @foodCount = count from dbo.billinfo where idbill = @idbill and idfood = @idfood

	if (@isExitsBillInfo >0)
	begin 
		declare @newcount int = @foodCount + @count 
		if( @newcount > 0) 
		update dbo.billinfo set count = @foodCount + @count where idfood = @idfood
		else 
		delete dbo.billinfo where idbill = @idbill and idfood = @idfood
	end
	else 
	begin
		insert billinfo (idbill, idfood, count)
		values (@idbill , @idfood , @count)	
	end
end
go

 
create trigger UTG_UpdateBillInfo
on dbo.billinfo for insert, update
as
begin
	declare @idbill int

	select @idbill = idbill from inserted

	declare @idtable INT

	select @idtable = idtable from dbo.bill where id = @idbill and status =0
	
	declare @count int
	select @count = count(*) from dbo.billinfo where idbill = @idbill

	if(@count > 0 )
	update dbo.tablefood set status = N'Có người' where id = @idtable
	else
	update dbo.tablefood set status = N'Trống' where id = @idtable
end
go


create trigger UTG_UpdateBill
on dbo.bill for update
as 
begin
	declare @idbill int
	select @idbill = id from inserted
	

	declare @idtable INT

	select @idtable = idtable from dbo.bill where id = @idbill 

	declare @count int = 0

	select @count = count (*) from dbo.bill where idtable = @idtable and status = 0

	if (@count = 0)
		update dbo.tablefood set status = N'Trống' where id = @idtable

end
go


create proc USP_SwitchTable
@idtable1 int , @idtable2 int
as begin
	declare @idfirstbill int
	declare @idsecondbill int

	declare @isFirstTableEmpty int =1
	declare @isSecondTableEmpty int =1

	select @idsecondbill = id from dbo.bill where idtable = @idtable2 and status = 0
	select @idfirstbill = id from dbo.bill where idtable = @idtable1 and status = 0


	if (@idfirstbill is null)
	begin
		insert  dbo.bill (datecheckin, datecheckout, idtable, status)
		values ( GETDATE(), --datecheckin
		null,--datecheckout
		@idtable1,--idtable,
		0--status
		) 
		select @idfirstbill = max(id) from dbo.bill where idtable = @idtable1 and status = 0
	end

	select @isFirstTableEmpty = count (*) from dbo.billinfo where idbill = @idfirstbill

		
	if (@idsecondbill is null)
	begin
		insert  dbo.bill (datecheckin, datecheckout, idtable, status)
		values ( GETDATE(), --datecheckin
		null,--datecheckout
		@idtable2,--idtable,
		0--status
		)
		select @idsecondbill = max(id) from dbo.bill where idtable = @idtable2 and status = 0				
	end
		select @isSecondTableEmpty= count (*) from dbo.billinfo where idbill = @idsecondbill



	select id into idbillinfotable from dbo.billinfo where idbill = @idsecondbill

	update dbo.billinfo set idbill = @idsecondbill where idbill = @idfirstbill

	update dbo.billinfo set idbill = @idfirstbill where id in (select * from idbillinfotable )

	drop table idbillinfotable

	if (@isFirstTableEmpty =0 )
		update dbo.tablefood set status = N'Trống' where id = @idtable2

	if (@isSecondTableEmpty =0 )
		update dbo.tablefood set status = N'Trống' where id = @idtable1

end
go

alter table dbo.bill add totalprice float
go

create proc USP_GetListBillByDate
@checkin date , @checkout date
as
begin
	select t.name as [Tên bàn], b.totalprice as [Tổng tiền], datecheckin as [Ngày checkin] ,datecheckout as [Ngày checkout], discount as [Giảm giá (%)]
	from dbo.bill as b, dbo.tablefood as t 
	where datecheckin >= @checkin and datecheckout <= @checkout and b.status = 1
	and t.id = b.idtable 
end
go


create proc USP_UpdateAccount
@username nvarchar(100), @displayname nvarchar(100), @password nvarchar(100), @newpassword nvarchar(100)
as
begin
	declare @isrightpass int = 0

	select @isrightpass = count (*) from dbo.account where username= @username and password = @password

	if(@isrightpass = 1)
	begin
		if (@newpassword = NULL or @newpassword = '')
		begin
			update dbo.account set displayname = @displayname where username = @username
		end
		else
			update dbo.account set displayname = @displayname, password = @newpassword where username = @username
	end
end
go

create trigger UTG_DeleteBillInfo
on dbo.billinfo for delete
as
begin
	declare @idbillinfo int
	declare @idbill int
	select @idbillinfo = id, @idbill = deleted.idbill from deleted

	declare @idtable int 
	select @idtable = idtable from dbo.bill where id = @idbill

	declare @count int = 0
	select @count = count (*) from dbo.billinfo as bi, dbo.bill as b where b.id = bi.idbill and b.id = @idbill and b.status = 0

	if(@count =0)
		update dbo.tablefood set status = N'Trống' where id = @idtable	 
end
go
 

CREATE FUNCTION [dbo].[GetUnsignString](@strInput NVARCHAR(4000)) 
RETURNS NVARCHAR(4000)
AS
BEGIN     
    IF @strInput IS NULL RETURN @strInput
    IF @strInput = '' RETURN @strInput
    DECLARE @RT NVARCHAR(4000)
    DECLARE @SIGN_CHARS NCHAR(136)
    DECLARE @UNSIGN_CHARS NCHAR (136)

    SET @SIGN_CHARS       = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'+NCHAR(272)+ NCHAR(208)
    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'

    DECLARE @COUNTER int
    DECLARE @COUNTER1 int
    SET @COUNTER = 1
 
    WHILE (@COUNTER <=LEN(@strInput))
    BEGIN   
      SET @COUNTER1 = 1
      --Tim trong chuoi mau
       WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
       BEGIN
     IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
     BEGIN           
          IF @COUNTER=1
              SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1)                   
          ELSE
              SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)    
              BREAK         
               END
             SET @COUNTER1 = @COUNTER1 +1
       END
      --Tim tiep
       SET @COUNTER = @COUNTER +1
    END
    RETURN @strInput
END
go




