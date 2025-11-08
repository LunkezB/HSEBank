using HSEBank.Exceptions;
using HSEBank.Export;

namespace HSEBank.Models
{
    public sealed class Operation : IVisitable
    {
        public Operation(Guid id, OperationType type, Guid bankAccountId, decimal amount, DateTime date,
            Guid categoryId, string? description)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException("Id операции не может быть пустым.");
            }

            if (bankAccountId == Guid.Empty)
            {
                throw new ValidationException("bank_account_id обязателен.");
            }

            if (categoryId == Guid.Empty)
            {
                throw new ValidationException("category_id обязателен.");
            }

            if (amount <= 0)
            {
                throw new ValidationException("Сумма операции должна быть > 0.");
            }

            Id = id;
            Type = type;
            BankAccountId = bankAccountId;
            Amount = amount;
            Date = date;
            CategoryId = categoryId;
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        public Guid Id { get; }
        public OperationType Type { get; }
        public Guid BankAccountId { get; }
        public decimal Amount { get; private set; }
        public DateTime Date { get; private set; }
        public string? Description { get; private set; }
        public Guid CategoryId { get; private set; }

        public decimal SignedAmount => Amount * (int)Type;

        public void Accept(IExportVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Update(decimal amount, DateTime date, Guid categoryId, string? description)
        {
            if (amount <= 0)
            {
                throw new ValidationException("Сумма операции должна быть > 0.");
            }

            if (categoryId == Guid.Empty)
            {
                throw new ValidationException("category_id обязателен.");
            }

            Amount = amount;
            Date = date;
            CategoryId = categoryId;
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }
    }
}