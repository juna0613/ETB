using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.PubSub
{
    public interface ISubscriber
    {
        void Notify(PublishArg arg);
    }
}
