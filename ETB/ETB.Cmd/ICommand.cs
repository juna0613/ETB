using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.Cmd
{
    public interface ICommand
    {
        CommandStatus DoCommand();
    }
}
