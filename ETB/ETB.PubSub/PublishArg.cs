using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.PubSub
{
    [Serializable]
    public sealed class PublishArg
    {
        public IPublisher Publisher { get; private set; }
        public Topic Topic { get; private set; }
        public object Arg { get; private set; }

        public PublishArg(IPublisher publisher, Topic topic, object realArg)
        {
            Publisher = publisher;
            Topic = topic;
            Arg = realArg;
        }
    }
}
