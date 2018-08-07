using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SebasWW.BusinessFramework.Tracking
{
    public class TrackingParams
    {
        public TrackingParams(BusinessManager businessManager, GenericObjectBase businessObject, EntityEntry entityEntry)
        {
            BusinessManager = businessManager;
            BusinessObject = businessObject;
            EntityEntry = entityEntry;
        }

        public BusinessManager BusinessManager { get; }
        public GenericObjectBase BusinessObject { get; }
        public EntityEntry EntityEntry { get; }
    }
}
