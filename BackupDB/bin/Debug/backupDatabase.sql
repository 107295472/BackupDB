USE [master]
GO
/****** 对象:  StoredProcedure [dbo].[usp_backup]    脚本日期: 05/29/2012 14:15:18 ******/
if exists (select 1
          from sysobjects
          where  id = object_id('usp_backup')
          and type in ('P','PC'))
   drop procedure usp_backup
go
create proc [dbo].[usp_backup]
@dbUrl varchar(200),
@dbName varchar(200)
As
-----------------------------------------------------------------------------------
declare @backup_fullpath  nvarchar(255),
        @backup_path      nvarchar(50),
        @str_date         nvarchar(50)
   set @backup_path =@dbUrl
------------------------------------------------------------------------------------
 set @str_date =@dbName+substring(replace(replace(replace(convert(varchar(20),getdate(), 120),' ',''),'-',''),':',''),1,10)+'.bak'
set @backup_fullpath = @backup_path  +  @str_date   
backup database @dbName to disk = @backup_fullpath

------------------------------------------------------------------------------------
select @str_date

--backup   log   DevDb   with   no_log 截断事务日志
--declare @backup_fullpath  nvarchar(255),
--        @backup_path      nvarchar(50),
--        @str_date         nvarchar(50)
-- set @str_date ='BlueChip_'+substring(replace(replace(replace(convert(varchar(20),getdate(), 120),' ',''),'-',''),':',''),1,10)+'.bak'
--set @backup_fullpath = 'E:\backup\'  +  @str_date   
--backup database BlueChip to disk = @backup_fullpath