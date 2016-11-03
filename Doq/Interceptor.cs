using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using static Doq.DynamicMock;

namespace Doq
{
    internal class Interceptor : IInterceptor
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();
        private readonly List<MethodData> _methods = new List<MethodData>();

        void IInterceptor.Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name.StartsWith("get_"))
                HandleGetter(invocation);
            else
                HandleMethod(invocation);
        }

        private void HandleGetter(IInvocation invocation)
        {
            string name = invocation.Method.Name.Substring("get_".Length);
            invocation.ReturnValue = _properties[name];
        }


        private void HandleMethod(IInvocation invocation)
        {
            var method = _methods
                .Where(m => CaptionsEquality(m.Name, invocation.Method.Name))
                .Where(m => CheckArgs(m.Args, invocation.Arguments, invocation.Method.GetParameters()))
                .Single();

            invocation.ReturnValue = ConvertReturnValue(method.ReturnedValue, invocation.Method.ReturnType);
            HandleOut(invocation, method);
        }

        private bool CheckArgs(object[] expected, object[] actual, ParameterInfo[] info)
        {
            if (expected.Length != actual.Length) return false;

            for (int i = 0; i < expected.Length; i++)
            {
                object exp = expected[i], act = actual[i];
                if (exp == _)
                    continue;
                if (exp.Equals(act))
                    continue;
                if ((exp as Predicate<dynamic>)?.Invoke(act) == true)
                    continue;
                if (info[i].IsOut)
                    continue;
                return false;
            }
            return true;
        }

        private static void HandleOut(IInvocation invocation, MethodData method)
        {
            for (int i = 0; i < invocation.Arguments.Length; i++)
                invocation.Arguments[i] = method.Args[i];
        }

        private static object ConvertReturnValue(object value, Type type)
        {
            if (value == null || value.GetType() == type) return value;

            if (type.IsInterface)
            {
                ProxyGenerator generator = new ProxyGenerator();
                return generator.CreateInterfaceProxyWithoutTarget(type, new Wrapper(value));
            }
            else
            {
                var result = type.GetConstructor(new Type[0]).Invoke(new object[0]);
                foreach (var property in value.GetType().GetProperties())
                {
                    var propertyValue = property.GetValue(value);
                    type.GetProperties()
                        .Where(p => CaptionsEquality(property.Name, p.Name))
                        .Single()
                        .SetValue(result, propertyValue);
                }
                return result;
            }
        }


        private static bool CaptionsEquality(string expected, string actual)
            => Regex.IsMatch(actual, $"^{expected.Replace("_", ".+")}$");

        public void RegisterProperty(string name, object value) => _properties[name] = value;

        public void RegisterMethod(MethodData data) => _methods.Add(data);


        private class Wrapper : IInterceptor
        {
            private readonly object _obj;
            public Wrapper(object obj)
            {
                _obj = obj;
            }

            public void Intercept(IInvocation invocation)
            {
                var method = _obj.GetType().GetMethod(invocation.Method.Name);
                if (method == null && invocation.Method.Name == "Dispose")
                    method = _obj.GetType().GetMethod("Reset");
                var result = method.Invoke(_obj, invocation.Arguments);
                if (method.ReturnType != invocation.Method.ReturnType)
                    result = ConvertReturnValue(result, invocation.Method.ReturnType);

                invocation.ReturnValue = result;
            }
        }

    }
}
