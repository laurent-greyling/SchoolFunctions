using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;

namespace SchoolFunctions.Telemetry
{
    public class ApplicationInsights : IApplicationInsights
    {
        static TelemetryConfiguration Configuration = TelemetryConfiguration.Active;

        public TelemetryClient Create()
        {
            Configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            Configuration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

            return new TelemetryClient(Configuration)
            {
                InstrumentationKey = Configuration.InstrumentationKey
            };
        }
    }
}
