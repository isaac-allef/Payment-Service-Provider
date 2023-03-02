namespace Psp.Api.Validators;

public sealed class ValidatorNotification
{
    public List<string> Errors { get; private set; }
    public bool IsValid { get => Errors.Count == 0; }

    public ValidatorNotification()
    {
        Errors = new();
    }
    
    public void AddError(string messageError)
        => Errors.Add(messageError);
}
