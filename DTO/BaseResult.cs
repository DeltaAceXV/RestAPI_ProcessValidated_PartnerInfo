using Newtonsoft.Json;

namespace RestAPI_ProcessValidated_PartnerInfo.DTO
{
    public class BaseResult
    {

        public BaseResult(int result, string resultMessage)
        {
            this.Result = result;
            this.ResultMessage = resultMessage;
        }

        public int Result { get; private protected set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ResultMessage { get; private protected set; }

        public static BaseResult Success() => new BaseResult(result:1, default!);
        public static BaseResult Failed(string message) => new BaseResult(result:0, resultMessage:$"{message}");

    }
}
