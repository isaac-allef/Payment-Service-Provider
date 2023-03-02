using System.ComponentModel;

namespace Psp.Shared.Domain;

public enum PayableStatus
{
    [Description("PAID")]
    PAID,

    [Description("WAITING_FUNDS")]
    WAITING_FUNDS
}
