namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Constructor |
                    AttributeTargets.Delegate |
                    AttributeTargets.Enum |
                    AttributeTargets.Event |
                    AttributeTargets.Field |
                    AttributeTargets.Interface |
                    AttributeTargets.Method |
                    AttributeTargets.Module |
                    AttributeTargets.Property |
                    AttributeTargets.Struct,
                    AllowMultiple = false)]
    internal sealed class NonNullTypesAttribute : Attribute
    {
        public NonNullTypesAttribute(bool enabled = true) { }
    }
}