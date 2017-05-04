using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace system_core_with_authentication.Scheduler
{
    public interface IScheduler
    {
        void CheckMinimumStock();
        void Update();
    }
}
