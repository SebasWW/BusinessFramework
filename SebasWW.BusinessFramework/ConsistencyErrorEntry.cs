namespace SebasWW.BusinessFramework
{
	public class ConsistencyErrorEntry : BusinessException
	{
        public ConsistencyErrorEntry(object id, string key, string message) : base(message)
        {
            Id = id;
            Key = key;
        }
        public object Id { get; }

        public string Key { get; }
    }
}
