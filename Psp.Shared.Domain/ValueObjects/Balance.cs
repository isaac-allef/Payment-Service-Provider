namespace Psp.Shared.Domain;

public struct Balance
{
    public double AvailableAmount { get; private set; } = 0;
    public double WaitingFundsAmount { get; private set; } = 0;

    private Balance(double availableAmount, double waitingFundsAmount)
    {
        AvailableAmount = availableAmount;
        WaitingFundsAmount = waitingFundsAmount;
    }

    public static Balance New(double availableAmount, double waitingFundsAmount)
    {
        if (availableAmount < 0) throw new ArgumentOutOfRangeException(nameof(availableAmount));
        if (waitingFundsAmount < 0) throw new ArgumentOutOfRangeException(nameof(waitingFundsAmount));
        
        return new Balance(availableAmount, waitingFundsAmount);
    }
}
