using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.PubSub
{
    public interface IPublisher
    {
        void Publish(object arg);
        void Register(ISubscriber subscriber);
    }
}
