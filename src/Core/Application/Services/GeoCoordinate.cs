namespace Application.Services;

using Application.Interfaces;
using System;
using System.Threading.Tasks;

public class GeoCoordinate : IGeoCoordinate
{
	public Task<double> GetDistance(decimal longitude, decimal latitude, decimal otherLongitude, decimal otherLatitude)
	{
		var d1 = (double)latitude * (Math.PI / 180.0);
		var num1 = (double)longitude * (Math.PI / 180.0);
		var d2 = (double)otherLatitude * (Math.PI / 180.0);
		var num2 = (double)otherLongitude * (Math.PI / 180.0) - num1;
		var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

		double result = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
		return Task.FromResult(result);
	}
}