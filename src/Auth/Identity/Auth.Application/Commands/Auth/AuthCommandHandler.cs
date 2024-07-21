using Auth.Application.Common;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Commands.Auth
{
    public record AuthCommand(string UserName, string Password) : ICommand<AuthResult>;
    public record AuthResult(string UserId, string Name, string Token);
    public class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Username is required");
        }
    }
    public class AuthCommandHandler
        (ITokenGenerator tokenGenerator, IIdentityService identityService)
        : ICommandHandler<AuthCommand, AuthResult>
    {
        public async Task<AuthResult> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var result = await identityService.SigninUserAsync(request.UserName, request.Password);

            if (!result)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var (userId, fullName, userName, email, roles) = await identityService.GetUserDetailsAsync(await identityService.GetUserIdAsync(request.UserName));

            string token = tokenGenerator.GenerateJWTToken((userId, userName, roles));

            return new AuthResult(userId, userName, token);
        }
    }
}
