// ------------------------------------------------------------------------------------------------
//  <copyright file="BikeTypeUpdateInputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Areas.Manage.Models.Nomenclatures;

public class BikeTypeUpdateInputModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}