using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Alfred.Util.Model;

namespace Alfred.Business.AutoJob
{
    public interface IJobTask
    {
        Task<TData> Start();
    }
}
