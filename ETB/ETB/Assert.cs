using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB
{
    public class MultiException : Exception
    {
        private readonly IEnumerable<Exception> _es;
        public MultiException(params Exception[] es)
        {
            _es = es;
        }
        
        public override string Message
        {
            get
            {
                return _es.Select(e => e.Message).Join(Environment.NewLine);
            }
        }
        public override string StackTrace
        {
            get
            {
                return _es.Select(e => e.StackTrace).Join(Environment.NewLine);
            }
        }
    }
    public class AssertionHelper
    {
        public class Assertion
        {
            private readonly Func<bool> _func;
            private readonly string _message;
            internal Assertion(bool status, string message = "") : this(() => status, message)
            {
            }
            internal Assertion(Func<bool> statusFunc, string message = "")
            {
                _func = statusFunc;
                _message = message;
            }

            public void DoAssert()
            {
                if (_func != null)
                {
                    try
                    {
                        if (!_func())
                        {
                            throw new ApplicationException(_message);
                        }
                    }
                    catch (Exception e)
                    {
                        Logging.Logger.Instance.Warn(e.Message);
                        throw;
                    }
                }
            }
        }
        private readonly List<Assertion> _assertions = new List<Assertion>();
        private AssertionHelper()
        {
        }
        public void Add(bool status, string message = "")
        {
            _assertions.Add(new Assertion(status, message));
        }
        
        public void Add(Func<bool> status, string message = "")
        {
            _assertions.Add(new Assertion(status, message));
        }
        public void AddNotNull<T>(T data, string message = default(string))
        {
            _assertions.Add(new Assertion(() => data != null, "{0} is null".Format2(message)));
        }
        public void AddNotNulls<T>(IEnumerable<T> data, string message = default(string))
        {
            
            foreach(var x in data)
            {
                AddNotNull(x, message);
            }
        }

        private void DoAssert(bool throwException = true)
        {
            var overallStatus = true;
            var exceptions = new List<Exception>();
            foreach(var a in _assertions)
            {
                try
                {
                    a.DoAssert();
                }
                catch(Exception e)
                {
                    overallStatus = false;
                    exceptions.Add(e);
                }
            }
            if(!overallStatus && throwException)
            {
                throw new MultiException(exceptions.ToArray());
            }
        }
        public static void DoAssert(Action<AssertionHelper> assertions, bool throwException = true)
        {
            var helper = new AssertionHelper();
            assertions(helper);
            helper.DoAssert(throwException);
        }
    }

}
