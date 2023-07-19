// ------------------------------------------------------------------------------------------------
//  <copyright file="TestApplicationContext.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Tests;

using Persistence;

public class TestApplicationContext : IDisposable
{
    private readonly ApplicationDbContext _context;

    protected TestApplicationContext()
    {
        this._context = ApplicationDbContextFactory.Create();
    }

    public void Dispose()
    {
        this._context.Dispose();
    }

    public ApplicationDbContext Context => this._context;
}