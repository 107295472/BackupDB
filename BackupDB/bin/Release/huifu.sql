--restore filelistonly from disk='E:\backup\test2012052915.bak' -- 查看次备份文件的信息
if exists (select 1
          from sysobjects
          where  id = object_id('Usp_restore')
          and type in ('P','PC'))
   drop procedure Usp_restore
go
create proc Usp_restore
@dbName varchar(200),
@dbNewName varchar(200),
@bakUrl varchar(200),
@dbMdfUrl varchar(200),
@dbLdfUrl varchar(200)
as
declare @temp varchar(100)
set @temp=@dbName+'_log'
restore database @dbNewName from disk=@bakUrl with replace,
move @dbName to @dbMdfUrl,
move @temp to @dbLdfUrl
select 1

--exec sp_detach_db 'test1'
--restore database test1 from disk='E:\backup\test2012052915.bak' with replace,
--move 'BlueChip' to 'E:\db\test1.mdf',
--move 'BlueChip_log' to 'E:\db\test1_log.ldf'
--exec sp_attach_db test1,'E:\db\test1.mdf','E:\db\test1_log.ldf' 

--create database test1
--on
--(
--	name='test1',
--	filename='E:\db\test1.mdf'
--)