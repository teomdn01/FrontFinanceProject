// using System;
// using System.Collections.Generic;
// using Microsoft.ApplicationInsights;
// using Org.Front.Core.Contracts.Models.Events;
// using Org.Front.Core.Contracts.Ports;
//
// namespace Org.Front.Core.Logic
// {
//     public class EventService : IEventService
//     {
//         private readonly TelemetryClient telemetryClient;
//
//         public EventService(TelemetryClient telemetryClient)
//         {
//             this.telemetryClient = telemetryClient;
//         }
//
//         public void TrackEvent(EventSource eventSource, Guid userId, string eventName, IDictionary<string, string> eventData, bool isB2BClient = false)
//         {
//             var userType = isB2BClient ? "Client Id" : "User Id";
//             eventData.Add(userType, userId.ToString());
//             telemetryClient.TrackEvent($"{EventSources.Mapping[eventSource]} : {eventName}", eventData);
//         }
//
//
//         public void TrackTrace(string caption, string message, Guid? userId = null)
//         {
//             if (string.IsNullOrEmpty(message))
//             {
//                 return;
//             }
//
//             var eventData = new Dictionary<string, string>()
//             {
//                 {"Message", message}
//             };
//
//             if (userId.HasValue)
//             {
//                 eventData.Add("User Id", userId.ToString());
//             }
//             
//             telemetryClient.TrackEvent($"{EventSources.Mapping[EventSource.TraceMessage]} : {caption}", eventData);
//         }
//     }
// }