using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public sealed class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(70);

        RuleFor(x => x.ShippingAddress).NotNull();
        RuleFor(x => x.Payment).NotNull();

        RuleFor(x => x.ShippingAddress.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Payment.PaymentReference)
            .NotEmpty()
            .MaximumLength(200);
    }
}
