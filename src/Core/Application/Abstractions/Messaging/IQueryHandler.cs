namespace Application.Abstractions.Messaging
{
	using MediatR;

	internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
	where TQuery : IQuery<TResponse>
	{
	}
}
