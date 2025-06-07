using Newtonsoft.Json;
using RestAPI_ProcessValidated_PartnerInfo.Attributes;
using System.Reflection;

namespace RestAPI_ProcessValidated_PartnerInfo.Service
{
    public interface IApiRequestLogger
    {
        Task LogRequest<T>(T data);
    }

    public class ApiRequestLogger : IApiRequestLogger
    {
        private readonly ILoggerService _loggerService;
        private readonly IAesEncryptionService _aesEncryptionService;

        public ApiRequestLogger(ILoggerService loggerService
            , IAesEncryptionService aesEncryptionService) { 
            this._loggerService = loggerService;
            this._aesEncryptionService = aesEncryptionService;
        }

        public async Task LogRequest<T>(T data)
        {
            var screenedData = await this.ScreenSensitiveData(data);

            await this._loggerService.Information(
                JsonConvert.SerializeObject(screenedData)    
            );

            return;
        }

        private Task<object> ScreenSensitiveData<T>(T data)
        {
            var type = data!.GetType();
            var clone = Activator.CreateInstance(type);


            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead || !prop.CanWrite) continue;

                var value = prop.GetValue(data);

                if (Attribute.IsDefined(prop, typeof(SensitiveAttribute)))
                {
                    var result = this._aesEncryptionService.Encrypt(value.ToString());

                    if (result.SuccessBool)
                    {
                        prop.SetValue(clone, $"{ result.Data }");
                    }

                }
                else
                {
                    prop.SetValue(clone, value);
                }
            }

            return Task.FromResult(clone)!;
        }

    }
}
