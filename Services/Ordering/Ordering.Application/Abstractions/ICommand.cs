using MediatR;

namespace Ordering.Application.Abstractions;

public interface ICommandMarker { }
public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandMarker { }
public interface ICommand : IRequest, ICommandMarker { }
