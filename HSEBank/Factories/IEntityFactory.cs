using HSEBank.Models;

namespace HSEBank.Factories
{
    public interface IEntityFactory
    {
        BankAccount CreateAccount(string name, decimal initialBalance = 0);
        Category CreateCategory(CategoryType type, string name);

        Operation CreateOperation(OperationType type, Guid accountId, Guid categoryId, decimal amount, DateTime date,
            string? description);
    }
}