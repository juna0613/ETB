using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.PubSub
{
    public class SimplePublisher : IPublisher
    {
        private readonly Topic _topic;
        private List<ISubscriber> _subs = new List<ISubscriber>();
        public SimplePublisher(Topic topic)
        {
            _topic = topic;
        }
        #region IPublisher メンバ

        public void Publish(object arg)
        {
            foreach (var sub in _subs)
            {
                try
                {
                    sub.Notify(new PublishArg(this, _topic, arg));
                }
                catch
                {
                    // ignore
                }
            }
        }

        public void Register(ISubscriber subscriber)
        {
            _subs.Add(subscriber);
        }

        #endregion
    }
}
