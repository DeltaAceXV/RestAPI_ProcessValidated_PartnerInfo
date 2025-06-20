namespace RestAPI_ProcessValidated_PartnerInfo.DTO
{
    public class Result<T> : BaseResult
    {

        protected Result(int successInd, string message, T? Data) : base(successInd, message) { 
            base.Result = successInd;
            base.ResultMessage = message;
            this.Data = Data;
        }

        public bool SuccessBool => base.Result == 1;

        public T? Data { get; }
        public static Result<T> Success(T value) => new Result<T>(successInd: 1, Data: value, message: "");
        public static new Result<T> Failed(string message) => new Result<T>(successInd: 0, Data: default, message: $"{message}");

    }
}
