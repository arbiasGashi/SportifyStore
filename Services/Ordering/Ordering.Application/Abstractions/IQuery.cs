using MediatR;

namespace Ordering.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse> { }