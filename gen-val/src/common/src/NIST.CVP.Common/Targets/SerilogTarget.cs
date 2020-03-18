using System;
using System.Linq;
using NLog;
using NLog.Targets;
using Serilog;

namespace NIST.CVP.Common.Targets
{
    /// <summary>
    /// Provides a Serilog target for use by NLog.
    /// Should allow for all NLog related logging to log to Serilog until all NLog calls are replaced with
    /// <see cref="Microsoft.Extensions.Logging"/>.
    /// </summary>
    /// <remarks>From: https://stackoverflow.com/questions/49592351/redirect-all-nlog-output-to-serilog-with-a-custom-target</remarks>
    [Target(TargetName)]
    public sealed class SerilogTarget : TargetWithLayout
    {
        public const string TargetName = "SerilogTarget";
        
        protected override void Write(LogEventInfo logEvent)
        {
            var log = Log.ForContext(Serilog.Core.Constants.SourceContextPropertyName, logEvent.LoggerName);
            var logEventLevel = ConvertLevel(logEvent.Level);
            if ((logEvent.Parameters?.Length ?? 0) == 0)
            {
                // NLog treats a single string as a verbatim string; Serilog treats it as a String.Format format and hence collapses doubled braces
                // This is the most direct way to emit this without it being re-processed by Serilog (via @nblumhardt)
                var template = new Serilog.Events.MessageTemplate(new[] { new Serilog.Parsing.TextToken(logEvent.FormattedMessage) });
                log.Write(new Serilog.Events.LogEvent(DateTimeOffset.Now, logEventLevel, logEvent.Exception, template, Enumerable.Empty<Serilog.Events.LogEventProperty>()));
            }
            else
                // Risk: tunneling an NLog format and assuming it will Just Work as a Serilog format
#pragma warning disable Serilog004 // Constant MessageTemplate verifier
                log.Write(logEventLevel, logEvent.Exception, logEvent.Message, logEvent.Parameters);
#pragma warning restore Serilog004
        }

        static Serilog.Events.LogEventLevel ConvertLevel(LogLevel logEventLevel)
        {
            if (logEventLevel == LogLevel.Trace)
                return Serilog.Events.LogEventLevel.Verbose;
            if (logEventLevel == LogLevel.Debug)
                return Serilog.Events.LogEventLevel.Debug;
            if (logEventLevel == LogLevel.Info)
                return Serilog.Events.LogEventLevel.Information;
            if (logEventLevel == LogLevel.Warn)
                return Serilog.Events.LogEventLevel.Warning;    
            if (logEventLevel == LogLevel.Error)
                return Serilog.Events.LogEventLevel.Error;
            return Serilog.Events.LogEventLevel.Fatal;
        }
        
        public static void ReplaceAllNLogTargetsWithSingleSerilogForwarder()
        {
            // sic: blindly overwrite the forwarding rules every time
            var target = new SerilogTarget();
            var cfg = new NLog.Config.LoggingConfiguration();
            cfg.AddTarget(nameof(SerilogTarget), target);
            cfg.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Trace, target));
            // NB assignment must happen last; rules get ingested upon assignment
            LogManager.Configuration = cfg;
        }
    }
}