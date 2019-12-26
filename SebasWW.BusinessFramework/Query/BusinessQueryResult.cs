namespace SebasWW.BusinessFramework.Query
{
	public class BusinessQueryResult<TResult>
    {
        internal TResult Result{get; set;}

        public BusinessQueryResult(TResult result)
        {
            Result = result;
        }
    }
}
