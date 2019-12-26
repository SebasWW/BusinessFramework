using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework.Log
{
    public class LogEntry
    {
        internal LogEntry
            (
                string dbSetName,
                Func<object> primaryKeyGetter,
                string propertyName,
                object oldValue,
                Func<object> newValueGetter
            )
        {
            DbSetName = dbSetName;
            _primaryKeyGetter = primaryKeyGetter;
            PropertyName = propertyName;
            OldValue = oldValue;
            _newValueGetter = newValueGetter;
        }

        Func<object> _primaryKeyGetter;
        Func<object> _newValueGetter;

        public string DbSetName { get; }
        public object PrimaryKey { get => _primaryKeyGetter.Invoke(); }
        public string PropertyName { get; }
        public object OldValue { get; }
        public object NewValue { get => _newValueGetter.Invoke(); }
    }
}
