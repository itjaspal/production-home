using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IJobInProcessService
    {
        JobInProcessView ScanAdd(JobInProcessSearchView model);
        JobInProcessView ScanCancel(JobInProcessSearchView model);
        JobInProcessView EntyAdd(JobInProcessSearchView model);
        JobInProcessView EntryCancel(JobInProcessSearchView model);
    }
}
