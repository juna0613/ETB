using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB.Exec
{
    public interface IExecutable
    {
        void Setup(params string[] args);
        ExecResult Execute();
        void Teardown();
    }
}
