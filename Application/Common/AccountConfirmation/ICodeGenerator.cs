namespace Application.Common.AccountConfirmation;

public interface ICodeGenerator<out T>
{
    T Generate();
}