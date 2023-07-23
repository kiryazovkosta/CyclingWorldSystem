// ------------------------------------------------------------------------------------------------
//  <copyright file="PictureImputModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models;

public sealed record PicturesInputModel(List<IFormFile> Files);