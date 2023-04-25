// using Microsoft.ApplicationInsights;
// using Microsoft.ApplicationInsights.DataContracts;
// using Microsoft.ApplicationInsights.Extensibility;
// using Microsoft.ApplicationInsights.Extensibility.Implementation;
// using Org.Front.Core.Contracts.Ports.Logging;
// using System;
//
// namespace Org.Front.Core.Logic.Telemetry
// {
//     public class TelemetryLoggingManager : ILoggingManager
//     {
//         private readonly TelemetryClient telemetryClient;
//
//         public TelemetryLoggingManager(TelemetryClient telemetryClient)
//         {
//             this.telemetryClient = telemetryClient;
//         }
//
//         public IDisposable CreateScope(string scopeName)
//         {
//             var holder = telemetryClient.StartOperation<RequestTelemetry>(scopeName);
//             return new OperationHolderStopper<RequestTelemetry>(holder, telemetryClient);
//         }
//
//         private class OperationHolderStopper<T> : IDisposable
//             where T : OperationTelemetry
//         {
//             private readonly TelemetryClient telemetryClient;
//             private readonly IOperationHolder<T> operationHolder;
//
//             public OperationHolderStopper(
//                 IOperationHolder<T> operationHolder,
//                 TelemetryClient telemetryClient)
//             {
//                 this.operationHolder = operationHolder;
//                 this.telemetryClient = telemetryClient;
//             }
//
//             public void Dispose() => telemetryClient.StopOperation(operationHolder);
//         }
//     }
// }
