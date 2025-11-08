namespace HSEBank.Export
{
    public interface IVisitable
    {
        void Accept(IExportVisitor visitor);
    }
}