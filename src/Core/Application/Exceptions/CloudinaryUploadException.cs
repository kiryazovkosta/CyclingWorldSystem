// ------------------------------------------------------------------------------------------------
//  <copyright file="CloudinaryUploadException.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Application.Exceptions;

using Domain.Exceptions.Base;

public class CloudinaryUploadException : BadRequestException
{
    public CloudinaryUploadException(string message) 
        : base(message)
    {
    }
}