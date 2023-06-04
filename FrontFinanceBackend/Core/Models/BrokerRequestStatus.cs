namespace Core.Models;

public enum BrokerRequestStatus
{
    /// <summary>
    /// Institution was successfully connected
    /// </summary>
    Succeeded = 0,
    /// <summary>
    /// There was a problem connecting to the institution
    /// </summary>
    Failed = 1,
    /// <summary>
    /// Authorization error occurred. In most of the cases it means that the institution token should
    /// be refreshed
    /// </summary>
    NotAuthorized = 2
}