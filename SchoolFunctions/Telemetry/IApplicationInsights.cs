using Microsoft.ApplicationInsights;

namespace SchoolFunctions.Telemetry
{
    interface IApplicationInsights
    {
        TelemetryClient Create();
    }
}
