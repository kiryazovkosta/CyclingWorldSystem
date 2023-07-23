namespace Persistence.Infrastructure;

using System.Security.Principal;
using Application.Interfaces;

public class DesignTimePrincipalProvider : ICurrentPrincipalProvider
{
    public IPrincipal GetCurrentPrincipal()
    {
        return Thread.CurrentPrincipal!;
    }

    public string? GetUserName()
    {
        return this.GetCurrentPrincipal().Identity?.Name;
    }
}