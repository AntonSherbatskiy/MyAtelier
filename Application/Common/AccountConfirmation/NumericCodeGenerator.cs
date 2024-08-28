namespace Application.Common.AccountConfirmation;

public class NumericCodeGenerator : ICodeGenerator<int>
{
    public int Generate()
    {
        return new Random().Next(1000, 5000);
    }
}