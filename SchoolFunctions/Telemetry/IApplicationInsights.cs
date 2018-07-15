using Microsoft.ApplicationInsights;

namespace SchoolFunctions.Telemetry
{
    interface IApplicationInsights
    {
        /// <summary>
        /// Create telemetry for application insights to log details
        /// </summary>
        /// <returns></returns>
        TelemetryClient Create();
    }
}
