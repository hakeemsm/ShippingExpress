namespace ShippingExpress.Domain.Services
{
    public class OperationResult<TEntity>:OperationResult
    {
        public OperationResult(bool isSuccess)
            : base(isSuccess) { }

        public TEntity Entity { get; set; }
    }

    public class OperationResult
    {
        public OperationResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; private set; }
    }
}