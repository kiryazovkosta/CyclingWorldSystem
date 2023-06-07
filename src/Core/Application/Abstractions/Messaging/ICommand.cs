﻿namespace Application.Abstractions.Messaging;

using MediatR;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}