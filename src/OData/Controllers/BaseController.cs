using EntityFramework.Data;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace OData.Controllers;

public abstract class BaseController : ODataController
{
    private readonly MoviesRentContext _context;

    protected BaseController(MoviesRentContext context)
    {
        _context = context;
    }

    protected async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
}