namespace Core.Models;

public enum BrokerAuthStatus
{
    Failed,
    ChallengeFailed,
    Succeeded,
    ChallengeIssued,
    MfaRequired,
    OpenInBrokerModule,
    Delayed
}