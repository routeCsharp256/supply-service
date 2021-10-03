using System;
using System.Transactions;
using Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services;

namespace Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Database
{
    public class TransactionFactory: ITransactionFactory
    {
        public TransactionScope CreateTransactionScope()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TimeSpan.FromSeconds(30)
                },
                TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}