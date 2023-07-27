// ------------------------------------------------------------------------------------------------
//  <copyright file="CenterCoordinateDto.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Domain.Entities.Dtos;

public class CenterCoordinateDto
{
    public decimal Longitude { get; init; }
    public decimal Latitude { get; init; }
}