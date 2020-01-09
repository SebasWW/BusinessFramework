using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SebasWW.BusinessFramework.Tracking
{
    public class TrackingParams
    {
        public TrackingParams(BusinessContext businessManager, BusinessObjectBase businessObject, EntityEntry entityEntry)
        {
            BusinessManager = businessManager;
            BusinessObject = businessObject;
            EntityEntry = entityEntry;
        }

        public BusinessContext BusinessManager { get; }
        public BusinessObjectBase BusinessObject { get; }
        public EntityEntry EntityEntry { get; }
    }
}
