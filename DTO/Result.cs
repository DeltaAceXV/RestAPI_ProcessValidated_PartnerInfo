namespace RestAPI_ProcessValidated_PartnerInfo.DTO
{
    public class Result<T>
    {

        protected Result(int successInd, string message, T? Data) { 
            this.SuccessInd = successInd;
            this.Message = message;
            this.Data = Data;
        }

        public int SuccessInd { get; }
        public bool SuccessBool => this.SuccessInd == 1;
        public string Message { get; }
        public T? Data { get; }

        public static Result<T> Success(T value) => new Result<T>(successInd: 1, Data: value, message: "");
        public static Result<T> Error(string message) => new Result<T>(successInd: 0, Data: default, message: $"{message}");

    }
}
