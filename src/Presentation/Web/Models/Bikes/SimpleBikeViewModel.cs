// ------------------------------------------------------------------------------------------------
//  <copyright file="SimpleBikeViewModel.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models.Bikes;

public sealed record SimpleBikeViewModel(Guid Id, string Name);