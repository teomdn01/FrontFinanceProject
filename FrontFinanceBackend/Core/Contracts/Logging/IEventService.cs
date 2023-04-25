using Org.Front.Core.Contracts.Models.Events;
using System;
using System.Collections.Generic;

namespace Org.Front.Core.Contracts.Ports
{
    public interface IEventService
    {
        void TrackEvent(EventSource eventSource, Guid userId, string eventName,  IDictionary<string, string> eventData, bool isB2BClient = false);
        void TrackTrace(string caption, string message, Guid? userId = null);
    }
}