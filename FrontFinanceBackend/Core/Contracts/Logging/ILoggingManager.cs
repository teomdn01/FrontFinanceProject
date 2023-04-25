using System;

namespace Org.Front.Core.Contracts.Ports.Logging
{
    public interface ILoggingManager
    {
        IDisposable CreateScope(string scopeName);
    }
}
