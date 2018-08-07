using System;
using System.Collections.Generic;
using System.Text;

namespace SebasWW.BusinessFramework.Tracking
{
    public interface ITracker
    {
        void OnChanging(TrackingParams pars);
        void OnChangedUncommitted(TrackingParams pars);
        void OnChanged(TrackingParams pars);
    }
}
