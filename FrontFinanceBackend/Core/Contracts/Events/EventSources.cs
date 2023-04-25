using System.Collections.Generic;

namespace Org.Front.Core.Contracts.Models.Events
{
    public static class EventSources
    {
        public static IReadOnlyDictionary<EventSource, string> Mapping = new Dictionary<EventSource, string>()
        {
            [EventSource.BrokerService] = "Org.Front.Core.Logic.BrokerService",
            [EventSource.BrokerServiceOrders] = "Org.Front.Core.Logic.BrokerService.Orders",
            [EventSource.BrokerageAccountService] = "Org.Front.Core.Logic.BrokerageAccountService",
            [EventSource.PortfolioService] = "Org.Front.Core.Logic.PortfolioService",
            [EventSource.BlacklistingService] = "Org.Front.Core.Logic.BlacklistingService",
            [EventSource.TraceMessage] = "Trace"
        };
    }

    public enum EventSource
    {
        BrokerService,
        BrokerageAccountService,
        PortfolioService,
        TraceMessage,
        BlacklistingService,
        BrokerServiceOrders,
    }
}
