﻿// ------------------------------------------------------------------------------------------------
//  <copyright file="ParseGpxFileCommandHandlerTests.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests.ApplicationTests.Entities.GpxFiles.Commands;

using Application.Entities.GpxFiles.Commands.ParseGpxFile;
using Application.Entities.GpxFiles.Models;
using Application.Interfaces;
using Application.Services;
using Domain.Repositories;
using Domain.Repositories.Abstractions;
using Mapster;
using Moq;
using Persistence;

public class ParseGpxFileCommandHandlerTests : IDisposable
{
    private readonly ApplicationDbContext _context = ApplicationDbContextTestFactory.Create();
    private readonly IGpxService gpxService = new GpxService();
    private readonly IGeoCoordinate geoCoordinateService = new GeoCoordinate();
    private readonly Mock<IWaypointRepository> waypointRepository = new();
    private readonly Mock<IUnitOfWork> unitOfWork = new();

    public void Dispose()
    {
        this._context.Dispose();
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenIGpxServiceIsNull()
    {
        //Arrange & Act
        Func<ParseGpxFileCommandHandler> act = () => new ParseGpxFileCommandHandler(
            null!,
            this.geoCoordinateService,
            this.waypointRepository.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'gpxService')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenIGeoCoordinateServiceIsNull()
    {
        //Arrange & Act
        Func<ParseGpxFileCommandHandler> act = () => new ParseGpxFileCommandHandler(
            this.gpxService,
            null!,
            this.waypointRepository.Object,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'geoCoordinateService')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenICurrentUserServiceIsNull()
    {
        //Arrange & Act
        Func<ParseGpxFileCommandHandler> act = () => new ParseGpxFileCommandHandler(
            this.gpxService,
            this.geoCoordinateService,
            null!,
            this.unitOfWork.Object);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'waypointRepository')", exception.Message);
    }
    
    [Fact]
    public void Ctor_Throws_ArgumentNullExceptionWhenUnitOfWorkIsNull()
    {
        //Arrange & Act
        Func<ParseGpxFileCommandHandler> act = () => new ParseGpxFileCommandHandler(
            this.gpxService,
            this.geoCoordinateService,
            this.waypointRepository.Object,
            null!);

        //Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);
        Assert.Equal("Value cannot be null. (Parameter 'unitOfWork')", exception.Message);
    }
    
    [Fact]
    public async Task  Handle_Should_ReturnSuccessWhenInputIsValid()
    {
        //Arrange
        var handler = new ParseGpxFileCommandHandler(
            this.gpxService,
            this.geoCoordinateService,
            this.waypointRepository.Object,
            this.unitOfWork.Object);
        var request = new GpxFileRequest(
            "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4NCjxncHggY3JlYXRvcj0iU3RyYXZhR1BYIiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiB4c2k6c2NoZW1hTG9jYXRpb249Imh0dHA6Ly93d3cudG9wb2dyYWZpeC5jb20vR1BYLzEvMSBodHRwOi8vd3d3LnRvcG9ncmFmaXguY29tL0dQWC8xLzEvZ3B4LnhzZCBodHRwOi8vd3d3Lmdhcm1pbi5jb20veG1sc2NoZW1hcy9HcHhFeHRlbnNpb25zL3YzIGh0dHA6Ly93d3cuZ2FybWluLmNvbS94bWxzY2hlbWFzL0dweEV4dGVuc2lvbnN2My54c2QgaHR0cDovL3d3dy5nYXJtaW4uY29tL3htbHNjaGVtYXMvVHJhY2tQb2ludEV4dGVuc2lvbi92MSBodHRwOi8vd3d3Lmdhcm1pbi5jb20veG1sc2NoZW1hcy9UcmFja1BvaW50RXh0ZW5zaW9udjEueHNkIiB2ZXJzaW9uPSIxLjEiIHhtbG5zPSJodHRwOi8vd3d3LnRvcG9ncmFmaXguY29tL0dQWC8xLzEiIHhtbG5zOmdweHRweD0iaHR0cDovL3d3dy5nYXJtaW4uY29tL3htbHNjaGVtYXMvVHJhY2tQb2ludEV4dGVuc2lvbi92MSIgeG1sbnM6Z3B4eD0iaHR0cDovL3d3dy5nYXJtaW4uY29tL3htbHNjaGVtYXMvR3B4RXh0ZW5zaW9ucy92MyI+DQogPG1ldGFkYXRhPg0KICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjM2WjwvdGltZT4NCiA8L21ldGFkYXRhPg0KIDx0cms+DQogIDxuYW1lPkFmdGVybm9vbiBNb3VudGFpbiBCaWtlIFJpZGU8L25hbWU+DQogIDx0cmtzZWc+DQogICA8dHJrcHQgbGF0PSI0Mi41MTEzNDIwIiBsb249IjI3LjQ3MjQ4NjAiPg0KICAgIDxlbGU+OTAuODwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6MzZaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjI5PC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTI5NzAiIGxvbj0iMjcuNDcyNTQ1MCI+DQogICAgPGVsZT45Mi4wPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MjozN1o8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+Mjk8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExMjk3MCIgbG9uPSIyNy40NzI1NDUwIj4NCiAgICA8ZWxlPjkyLjI8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjM4WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4yOTwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTEyNzcwIiBsb249IjI3LjQ3MjU4NjAiPg0KICAgIDxlbGU+OTIuNjwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6MzlaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjI5PC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTI2MzAiIGxvbj0iMjcuNDcyNjI5MCI+DQogICAgPGVsZT45Mi40PC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0Mjo0MFo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+Mjk8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExMjY1MCIgbG9uPSIyNy40NzI3NDIwIj4NCiAgICA8ZWxlPjkyLjI8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjQxWjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4yOTwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTEzMDQwIiBsb249IjI3LjQ3MjgzMDAiPg0KICAgIDxlbGU+OTIuMDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6NDJaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjI5PC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTMxNzAiIGxvbj0iMjcuNDcyODc0MCI+DQogICAgPGVsZT45Mi4yPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0Mjo0M1o8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+Mjk8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExMzMwMCIgbG9uPSIyNy40NzI5MTcwIj4NCiAgICA8ZWxlPjkyLjI8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjQ0WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4yOTwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTEzNzAwIiBsb249IjI3LjQ3Mjk5ODAiPg0KICAgIDxlbGU+OTIuMDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6NDVaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTQxMzAiIGxvbj0iMjcuNDczMDgwMCI+DQogICAgPGVsZT45Mi4wPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0Mjo0Nlo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExNDUwMCIgbG9uPSIyNy40NzMxNjcwIj4NCiAgICA8ZWxlPjkxLjY8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjQ3WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE0ODIwIiBsb249IjI3LjQ3MzI1NzAiPg0KICAgIDxlbGU+OTEuNDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6NDhaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTUwOTAiIGxvbj0iMjcuNDczMzQ5MCI+DQogICAgPGVsZT45MS4wPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0Mjo0OVo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExNTI1MCIgbG9uPSIyNy40NzM0MjYwIj4NCiAgICA8ZWxlPjkwLjg8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjUwWjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE1MzkwIiBsb249IjI3LjQ3MzUwMDAiPg0KICAgIDxlbGU+OTAuNjwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6NTFaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTU1NTAiIGxvbj0iMjcuNDczNTcxMCI+DQogICAgPGVsZT45MC40PC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0Mjo1Mlo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExNTc0MCIgbG9uPSIyNy40NzM2NDQwIj4NCiAgICA8ZWxlPjkwLjI8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjUzWjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE1OTUwIiBsb249IjI3LjQ3MzcxMzAiPg0KICAgIDxlbGU+OTAuMDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6NTRaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTYxNTAiIGxvbj0iMjcuNDczNzgyMCI+DQogICAgPGVsZT45MC4wPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0Mjo1NVo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExNjMwMCIgbG9uPSIyNy40NzM4NDMwIj4NCiAgICA8ZWxlPjg5Ljg8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjU2WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE2NDkwIiBsb249IjI3LjQ3Mzg5OTAiPg0KICAgIDxlbGU+ODkuODwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDI6NTdaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTY3MjAiIGxvbj0iMjcuNDczOTQ2MCI+DQogICAgPGVsZT44OS44PC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0Mjo1OFo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExNjkxMCIgbG9uPSIyNy40NzM5OTMwIj4NCiAgICA8ZWxlPjkwLjA8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQyOjU5WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE3MDgwIiBsb249IjI3LjQ3NDA0MjAiPg0KICAgIDxlbGU+OTAuMDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDM6MDBaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTcyMjAiIGxvbj0iMjcuNDc0MDg1MCI+DQogICAgPGVsZT45MC4wPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MzowMVo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExNzQ1MCIgbG9uPSIyNy40NzQxMjMwIj4NCiAgICA8ZWxlPjkwLjA8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQzOjAyWjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE3NjAwIiBsb249IjI3LjQ3NDE2NjAiPg0KICAgIDxlbGU+OTAuMDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDM6MDNaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTc2ODAiIGxvbj0iMjcuNDc0MjAyMCI+DQogICAgPGVsZT44OS44PC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MzowNFo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExNzY2MCIgbG9uPSIyNy40NzQyMzMwIj4NCiAgICA8ZWxlPjg5Ljg8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQzOjA1WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE3NjkwIiBsb249IjI3LjQ3NDI2ODAiPg0KICAgIDxlbGU+ODkuODwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDM6MDZaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTc3OTAiIGxvbj0iMjcuNDc0Mjk4MCI+DQogICAgPGVsZT44OS44PC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MzowN1o8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExODAyMCIgbG9uPSIyNy40NzQzMjAwIj4NCiAgICA8ZWxlPjg5Ljg8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQzOjA4WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE4NDQwIiBsb249IjI3LjQ3NDMxNzAiPg0KICAgIDxlbGU+OTAuMDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDM6MDlaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTg3MTAiIGxvbj0iMjcuNDc0Mjg2MCI+DQogICAgPGVsZT45MC4wPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MzoxMFo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExODk2MCIgbG9uPSIyNy40NzQyNDMwIj4NCiAgICA8ZWxlPjkwLjA8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQzOjExWjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTE5MTcwIiBsb249IjI3LjQ3NDE4ODAiPg0KICAgIDxlbGU+ODkuODwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDM6MTJaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMTkzODAiIGxvbj0iMjcuNDc0MTMzMCI+DQogICAgPGVsZT44OS42PC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MzoxM1o8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTExOTY1MCIgbG9uPSIyNy40NzQwODMwIj4NCiAgICA8ZWxlPjg5LjY8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQzOjE0WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTIwMDEwIiBsb249IjI3LjQ3NDA0NDAiPg0KICAgIDxlbGU+ODkuMjwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDM6MTVaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMjA0MTAiIGxvbj0iMjcuNDc0MDExMCI+DQogICAgPGVsZT44OS4wPC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MzoxNlo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICAgPHRya3B0IGxhdD0iNDIuNTEyMDg3MCIgbG9uPSIyNy40NzQwMTAwIj4NCiAgICA8ZWxlPjg4LjY8L2VsZT4NCiAgICA8dGltZT4yMDIyLTA4LTA4VDE0OjQzOjE3WjwvdGltZT4NCiAgICA8ZXh0ZW5zaW9ucz4NCiAgICAgPGdweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgICAgPGdweHRweDphdGVtcD4zMDwvZ3B4dHB4OmF0ZW1wPg0KICAgICA8L2dweHRweDpUcmFja1BvaW50RXh0ZW5zaW9uPg0KICAgIDwvZXh0ZW5zaW9ucz4NCiAgIDwvdHJrcHQ+DQogICA8dHJrcHQgbGF0PSI0Mi41MTIxMzEwIiBsb249IjI3LjQ3NDAzOTAiPg0KICAgIDxlbGU+ODguNDwvZWxlPg0KICAgIDx0aW1lPjIwMjItMDgtMDhUMTQ6NDM6MThaPC90aW1lPg0KICAgIDxleHRlbnNpb25zPg0KICAgICA8Z3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgICA8Z3B4dHB4OmF0ZW1wPjMwPC9ncHh0cHg6YXRlbXA+DQogICAgIDwvZ3B4dHB4OlRyYWNrUG9pbnRFeHRlbnNpb24+DQogICAgPC9leHRlbnNpb25zPg0KICAgPC90cmtwdD4NCiAgIDx0cmtwdCBsYXQ9IjQyLjUxMjE1MzAiIGxvbj0iMjcuNDc0MDY3MCI+DQogICAgPGVsZT44Ny44PC9lbGU+DQogICAgPHRpbWU+MjAyMi0wOC0wOFQxNDo0MzoxOVo8L3RpbWU+DQogICAgPGV4dGVuc2lvbnM+DQogICAgIDxncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICAgIDxncHh0cHg6YXRlbXA+MzA8L2dweHRweDphdGVtcD4NCiAgICAgPC9ncHh0cHg6VHJhY2tQb2ludEV4dGVuc2lvbj4NCiAgICA8L2V4dGVuc2lvbnM+DQogICA8L3Rya3B0Pg0KICA8L3Rya3NlZz4NCiA8L3Ryaz4NCjwvZ3B4Pg==");
        var command = request.Adapt<ParseGpxFileCommand>();
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
            
        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        var response = result.Value;
        Assert.Equal(0m, response.NegativeElevation);
        Assert.Equal(0.8m, response.PositiveElevation);
    }
}