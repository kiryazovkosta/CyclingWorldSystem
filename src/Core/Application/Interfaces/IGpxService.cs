namespace Application.Interfaces;

using Domain.Entities.GpxFile;

public interface IGpxService
{
	Task<Gpx> Get(string xml);
}
