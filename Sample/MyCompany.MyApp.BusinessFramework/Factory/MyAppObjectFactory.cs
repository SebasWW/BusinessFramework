using System;
using SebasWW.BusinessFramework;
using SebasWW.BusinessFramework.Factory;

namespace MyCompany.MyApp.Factory
{
    public abstract class MyAppObjectFactory<TObject, TEntry>: BusinessObjectFactory<TObject, TEntry, Int32>
        where TObject : BusinessObject<TEntry, Int32>
        where TEntry : class
    {
    }
}
