using HSEBank.Exceptions;
using HSEBank.Export;

namespace HSEBank.Models
{
    public sealed class BankAccount : IVisitable
    {
        public BankAccount(Guid id, string name, decimal initialBalance)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException("Id счёта не может быть пустым.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Имя счёта обязательно.");
            }

            Id = id;
            Name = name.Trim();
            Balance = initialBalance;
        }

        public Guid Id { get; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }

        public void Accept(IExportVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ValidationException("Имя счёта не может быть пустым.");
            }

            Name = newName.Trim();
        }

        internal void ApplyDelta(decimal delta)
        {
            Balance += delta;
        }
    }
}