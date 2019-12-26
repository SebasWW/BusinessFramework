namespace SebasWW.BusinessFramework
{
	public class ConsistencyErrorEntry : BusinessException
	{
        public ConsistencyErrorEntry(object id, string key, string message) : base(key, message)
        {
            Id = id;
        }
        public object Id { get; }
    }
}
