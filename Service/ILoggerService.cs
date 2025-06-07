using log4net;
using System.Reflection;

namespace RestAPI_ProcessValidated_PartnerInfo.Service
{
    public interface ILoggerService
    {
        public Task Information(string message);
        public Task Warn(string message);
        public Task Error(string message, Exception? ex);
    }

    public class LoggerService : ILoggerService
    {
        private readonly ILog _logger;

        public LoggerService()
        {
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepo, new FileInfo("Config/log4net.config"));
            log4net.Util.LogLog.InternalDebugging = true;
            this._logger = LogManager.GetLogger(typeof(LoggerService));
        }

        public Task Information(string message) { 
            this._logger.Info(message);
            return Task.CompletedTask;
        }

        public Task Warn(string message)
        {
            this._logger.Warn(message);
            return Task.CompletedTask;
        }

        public Task Error(string message, Exception? ex)
        {
            this._logger.Error(message, ex);
            return Task.CompletedTask;
        }

    }
}
