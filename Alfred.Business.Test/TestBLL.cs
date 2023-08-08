using Alfred.Util.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alfred.Business.Test
{
    public class TestBLL
    {
        TestService testService=new TestService();

        public async Task<TData<long>> ResetEmail()
        {
            return new TData<long>();
        }

        public async Task<List<TestEntity>> GetList()
        {
            var list= new List<TestEntity>();
            list= await testService.GetList();
            return list;
        }
    }
}
