using MediatR;

namespace ECommerce.Application.Features.Authentication.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponseDto>;