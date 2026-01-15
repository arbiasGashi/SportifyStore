using Ordering.Application.Abstractions;

namespace Ordering.Application.Commands;

public sealed record DeleteOrderCommand(int OrderId) : ICommand;
