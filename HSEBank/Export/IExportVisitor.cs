using HSEBank.Models;

namespace HSEBank.Export
{
    public interface IExportVisitor
    {
        void Visit(BankAccount account);
        void Visit(Category category);
        void Visit(Operation operation);
        string GetResult();
    }
}