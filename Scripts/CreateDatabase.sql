USE [master];
GO

IF NOT EXISTS(SELECT name FROM master.sys.databases WHERE name = 'TasksDb')
BEGIN
	CREATE DATABASE TasksDb;
END
GO