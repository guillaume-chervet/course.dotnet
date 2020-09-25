USE [master]

CREATE DATABASE FibonacciData
GO

USE FibonacciData
GO
CREATE SCHEMA sch_fib AUTHORIZATION [dbo];
GO

if not exists (select * from sysobjects where name='Fibonacci' and xtype='U')
BEGIN
CREATE TABLE [sch_fib].[Fibonacci](
	[Id] uniqueidentifier NOT NULL DEFAULT newid(),
	[Input] [int] NOT NULL,
	[Output] [bigint] NOT NULL,
 CONSTRAINT [PK_Fibonacci] PRIMARY KEY CLUSTERED ([FIB_Id]))
END
