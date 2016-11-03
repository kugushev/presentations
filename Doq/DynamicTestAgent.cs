using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doq
{
    public class DynamicTestAgent<T>
        where T : class
    {
        private readonly DynamicMock _mock = new DynamicMock();


        public dynamic Arrange { get { return _mock; } }
        public dynamic Act { get { return new ActExecutor(this); } }

        private object InvokeAct(InvokeMemberBinder binder, object[] args)
        {
            var type = typeof(T);
            var ctor = type.GetConstructors().Single();
            var ctorArgs = ctor.GetParameters().Select(p => _mock.ConvertTo(p.ParameterType)).ToArray();
            var testable = ctor.Invoke(ctorArgs);

            return type.GetMethod(binder.Name).Invoke(testable, args);
        }

        class ActExecutor : DynamicObject
        {
            private readonly DynamicTestAgent<T> _agent;
            public ActExecutor(DynamicTestAgent<T> agent)
            {
                _agent = agent;
            }

            public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
            {
                result = _agent.InvokeAct(binder, args);
                return true;
            }

        }
    }
}
