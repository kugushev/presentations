using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Doq
{
    public class DynamicMock : DynamicObject
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();
        private readonly Interceptor _interceptor = new Interceptor();

        public static dynamic _ { get; } = new object();

        public static Predicate<dynamic> If(Predicate<dynamic> lamb) => lamb;

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _interceptor.RegisterProperty(binder.Name, value);
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var data = new MethodData
            {
                Name = binder.Name,
                Args = args
            };

            _interceptor.RegisterMethod(data);
            result = data;
            return true;
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var type = binder.Type;
            result = type.IsInterface
                ? _generator.CreateInterfaceProxyWithoutTarget(type, _interceptor)
                : _generator.CreateClassProxy(type, _interceptor);
            return true;
        }

        public object ConvertTo(Type type)
        {
            return type.IsInterface
                ? _generator.CreateInterfaceProxyWithoutTarget(type, _interceptor)
                : _generator.CreateClassProxy(type, _interceptor);
        }
    }
}
