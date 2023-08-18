# CyclingWorldSystem
This system is used as individual project for SoftUni's module ASP.NET Advanced - June 2023

## Description
This is simple ASP.NET Core system for logging cycling activities. It is based on .Net Core and use ASP.NET CORE WebAPI for backend and ASP.NET Core MVC for frontend. All application data is comming from backend endpoints. For usage of system you need to start WebApi project and after that the Web project. It's used Cookie-based Authentication because all identity functionalitty come also from backend endpoints.

## System requiments
- Web server to host WebApi backend endpoint
- Microsoft SQL Server for database
- Web server to host Web frontend
  
## Initial data
When first migration is applied the system provide 3 users with 3 roles
- Admin with role Administrator and password P@ssw0rd
- Manager with role Manager and password P@ssw0rd
- User with role User and password P@ssw0rd.
After that you can  execure sql script https://github.com/kiryazovkosta/CyclingWorldSystem/blob/main/CyclingWorldSystemInitialScript.sql to populate database with test data

## Used technologies
