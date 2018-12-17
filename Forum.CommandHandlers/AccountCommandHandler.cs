﻿using System.Threading.Tasks;
using ENode.Commanding;
using ENode.Infrastructure;
using Forum.Commands.Accounts;
using Forum.Domain.Accounts;

namespace Forum.CommandHandlers
{
    public class AccountCommandHandler :
        ICommandHandler<RegisterNewAccountCommand>
    {
        private readonly ILockService _lockService;
        private readonly RegisterAccountService _registerAccountService;

        public AccountCommandHandler(ILockService lockService, RegisterAccountService registerAccountService)
        {
            _lockService = lockService;
            _registerAccountService = registerAccountService;
        }

        public Task HandleAsync(ICommandContext context, RegisterNewAccountCommand command)
        {
            return Task.Factory.StartNew(() => _lockService.ExecuteInLock(typeof(Account).Name, () =>
            {
                var account = new Account(command.AggregateRootId, command.Name, command.Password);
                _registerAccountService.RegisterAccount(account);
                context.Add(account);
            }));
        }
    }
}
