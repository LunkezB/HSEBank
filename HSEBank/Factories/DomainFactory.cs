using HSEBank.Models;

namespace HSEBank.Factories
{
    public sealed class DomainFactory : IEntityFactory
    {
        public BankAccount CreateAccount(string name, decimal initialBalance = 0)
        {
            return new BankAccount(Guid.NewGuid(), name, initialBalance);
        }

        public Category CreateCategory(CategoryType type, string name)
        {
            return new Category(Guid.NewGuid(), type, name);
        }

        public Operation CreateOperation(OperationType type, Guid accountId, Guid categoryId, decimal amount,
            DateTime date, string? description)
        {
            return new Operation(Guid.NewGuid(), type, accountId, amount, date, categoryId, description);
        }
    }
}