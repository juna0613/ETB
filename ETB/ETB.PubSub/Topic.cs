using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ETB.PubSub
{
    [Serializable]
    public class Topic
    {
        public string Name { get; private set; }
        public Topic(string name)
        {
            Name = name;
        }
    }
}
