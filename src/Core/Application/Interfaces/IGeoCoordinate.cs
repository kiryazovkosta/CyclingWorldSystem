namespace Application.Interfaces;

using System.Threading.Tasks;

public interface IGeoCoordinate
{
	Task<double> GetDistance(decimal longitude, decimal latitude, decimal otherLongitude, decimal otherLatitude);
}