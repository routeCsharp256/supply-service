using System.Transactions;

namespace Ozon.DotNetCourse.SupplyService.Domain.Interfaces.Services
{
    public interface ITransactionFactory
    {
        TransactionScope CreateTransactionScope();

    }
}