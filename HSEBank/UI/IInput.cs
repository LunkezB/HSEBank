namespace HSEBank.UI
{
    public interface IInput
    {
        string Ask(string prompt);
        string? AskOptional(string prompt);
        Guid AskGuid(string prompt);
        int AskInt(string prompt);
        decimal AskDecimal(string prompt, IFormatProvider? fp = null);
        DateTime AskDate(string prompt, string fmt, IFormatProvider? fp = null);
    }
}