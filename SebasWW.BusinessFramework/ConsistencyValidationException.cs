namespace SebasWW.BusinessFramework
{
    public class ConsistencyValidationException : BusinessException
    {
        internal ConsistencyValidationException(object id, string key, string message) : base( message)
        {
            Id = id;
            Key = key;
        }

        internal ConsistencyValidationException(ConsistencyErrorEntry entry) : base(entry.Message)
        {
            Id = entry.Id;
            Key = entry.Key;
        }

        public object Id { get;}
        public string Key { get; }
    }
}
