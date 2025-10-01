using AutoMapper;
using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookLibraryCleanArchitecture.Application.Commands
{
    public class RegisterUserCommandHandler(IAccountService accountService, IMapper mapper) 
        : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public async Task<RegisterResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
           var registerRequestDto = _mapper.Map<RegisterRequestDto>(request);

            _logger?.LogInformation("Registering user: {UserName}", registerRequestDto.UserName);
            return await _accountService.RegisterUserAsync(registerRequestDto);
        }
    }
}
