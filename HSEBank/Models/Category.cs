using HSEBank.Exceptions;
using HSEBank.Export;

namespace HSEBank.Models
{
    public sealed class Category : IVisitable
    {
        public Category(Guid id, CategoryType type, string name)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException("Id категории не может быть пустым.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Имя категории обязательно.");
            }

            Id = id;
            Type = type;
            Name = name.Trim();
        }

        public Guid Id { get; }
        public CategoryType Type { get; }
        public string Name { get; private set; }

        public void Accept(IExportVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ValidationException("Имя категории не может быть пустым.");
            }

            Name = newName.Trim();
        }
    }
}