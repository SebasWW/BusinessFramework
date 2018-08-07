using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework.Log
{
    public class LogEntry
    {
        internal LogEntry
            (
                String dbSetName,
                Func<Object> primaryKeyGetter,
                String propertyName,
                Object oldValue,
                Func<Object> newValueGetter
            )
        {
            DbSetName = dbSetName;
            _primaryKeyGetter = primaryKeyGetter;
            PropertyName = propertyName;
            OldValue = oldValue;
            _newValueGetter = newValueGetter;
        }

        Func<Object> _primaryKeyGetter;
        Func<Object> _newValueGetter;

        public string DbSetName { get; }
        public object PrimaryKey { get => _primaryKeyGetter.Invoke(); }
        public string PropertyName { get; }
        public object OldValue { get; }
        public object NewValue { get => _newValueGetter.Invoke(); }
    }
}
