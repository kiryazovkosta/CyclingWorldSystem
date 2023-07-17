// ------------------------------------------------------------------------------------------------
//  <copyright file="EmailAttachment.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Domain.Entities.Dtos;

public sealed record EmailAttachment(byte[] Content, string FileName, string MimeType);