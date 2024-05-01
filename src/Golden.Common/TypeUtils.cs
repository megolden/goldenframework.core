using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Golden.Common
{
    public static class TypeUtils
    {
        private static readonly Lazy<ModuleBuilder> _dynamicModuleBuilder;
        private static readonly MethodInfo _getDefaultValueMethod;

        static TypeUtils()
        {
            _dynamicModuleBuilder = new Lazy<ModuleBuilder>(() =>
            {
                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName($"Assembly_{Guid.NewGuid():N}"),
                    AssemblyBuilderAccess.Run);

                var moduleBuilder = assemblyBuilder.DefineDynamicModule(
                    $"DynamicModule_{Guid.NewGuid():N}");

                return moduleBuilder;
            });

            _getDefaultValueMethod = typeof(TypeUtils).GetMethod(nameof(GetDefaultValue),
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod);
        }

        public static bool IsEnumerable(this Type type)
        {
            return IsEnumerable(type, out _);
        }

        public static bool IsEnumerable(this Type type, out Type? underlyingType)
        {
            var isIEnumerable =
                type.IsInterface
                && type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);

            if (isIEnumerable)
            {
                underlyingType = type.GetGenericArguments().First();
                return true;
            }

            foreach (var interfaceType in type.GetInterfaces())
            {
                if (IsEnumerable(interfaceType, out underlyingType))
                    return true;
            }

            underlyingType = null;
            return false;
        }

        public static object DefaultValue(this Type type)
        {
            return _getDefaultValueMethod.MakeGenericMethod(type).Invoke(null, new object[0]);
        }

        private static T GetDefaultValue<T>() => default(T);

        public static object GetMemberValue(this MemberInfo member)
        {
            if (member is FieldInfo field)
                return field.GetValue(null);
            else if (member is PropertyInfo property)
                return property.GetValue(null);
            else
                throw new InvalidOperationException();
        }

        public static object GetMemberValue(this MemberInfo member, object obj)
        {
            if (member is FieldInfo field)
                return field.GetValue(obj);
            else if (member is PropertyInfo property)
                return property.GetValue(obj);
            else
                throw new InvalidOperationException();
        }

        public static void SetMemberValue(this MemberInfo member, object value)
        {
            if (member is FieldInfo field)
                field.SetValue(null, value);
            else if (member is PropertyInfo property)
                property.SetValue(null, value, index: null);
            else
                throw new InvalidOperationException();
        }

        public static void SetMemberValue(this MemberInfo member, object value, object obj)
        {
            if (member is FieldInfo field)
                field.SetValue(obj, value);
            else if (member is PropertyInfo property)
                property.SetValue(obj, value, index: null);
            else
                throw new InvalidOperationException();
        }

        public static Type GetMemberType(this MemberInfo member)
        {
            if (member is FieldInfo field)
                return field.FieldType;
            else if (member is PropertyInfo property)
                return property.PropertyType;
            else if (member is MethodInfo method)
                return method.ReturnType;
            else if (member is ConstructorInfo constructor)
                return constructor.DeclaringType;
            else
                throw new InvalidOperationException();
        }

        public static T GetStaticValue<T>(this Type type, string name)
        {
            return (T)type.GetStaticValue(name);
        }

        public static object GetStaticValue(this Type type, string name)
        {
            var searchOptions = BindingFlags.Public |
                                BindingFlags.NonPublic |
                                BindingFlags.Instance |
                                BindingFlags.Static |
                                BindingFlags.IgnoreCase;

            var member = type.GetMember(name, searchOptions)[0];
            return member.GetMemberValue();
        }

        public static void SetStaticValue(this Type type, string name, object value)
        {
            var searchOptions = BindingFlags.Public |
                                BindingFlags.NonPublic |
                                BindingFlags.Instance |
                                BindingFlags.Static |
                                BindingFlags.IgnoreCase;

            var member = type.GetMember(name, searchOptions)[0];
            member.SetMemberValue(value);
        }

        public static void SetStaticValue(this Type type, object memberValues)
        {
            var properties = memberValues.GetType().GetProperties();
            properties.ForEach(property => type.SetStaticValue(property.Name, property.GetValue(memberValues)));
        }

        public static T CreateInstance<T>(this Type type, params object[] arguments)
        {
            return (T)type.CreateInstance(arguments);
        }

        public static object CreateInstance(this Type type, params object[] arguments)
        {
            return Activator.CreateInstance(
                type,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                binder: null,
                arguments,
                culture: null);
        }

        public static Type CreateType(IDictionary<string, Type> properties)
        {
            if (properties == null || properties.Count == 0)
                throw new ArgumentException("no property specified", nameof(properties));

            var name = $"Type_{Guid.NewGuid():N}";
            var module = _dynamicModuleBuilder.Value;
            var type = module.DefineType(name, TypeAttributes.Public);

            DefineConstructors(type, parameterTypes: properties.Values, out var constructorILGenerator);

            properties.ForEach((_, index) =>
                DefineProperty(name: _.Key, type: _.Value, index, ownerType: type, constructorILGenerator));

            constructorILGenerator.Emit(OpCodes.Ret);

            return type.CreateTypeInfo();
        }

        private static void DefineProperty(
            string name,
            Type type,
            int index,
            TypeBuilder ownerType,
            ILGenerator constructorILGenerator)
        {
            var field = ownerType.DefineField($"_{name}", type, FieldAttributes.Private);
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Ldarg, index + 1);
            constructorILGenerator.Emit(OpCodes.Stfld, field);

            var property = ownerType.DefineProperty(name, PropertyAttributes.None, type, parameterTypes: null);
            var getMethodBuilder = ownerType.DefineMethod(
                $"Get{property.Name}",
                MethodAttributes.Public,
                CallingConventions.HasThis,
                property.PropertyType,
                parameterTypes: null);
            var getMethodILGenerator = getMethodBuilder.GetILGenerator();
            getMethodILGenerator.Emit(OpCodes.Ldarg_0);
            getMethodILGenerator.Emit(OpCodes.Ldfld, field);
            getMethodILGenerator.Emit(OpCodes.Ret);
            property.SetGetMethod(getMethodBuilder);

            var setMethodBuilder = ownerType.DefineMethod(
                $"Set{property.Name}",
                MethodAttributes.Public,
                CallingConventions.HasThis,
                returnType: null,
                new[] { property.PropertyType });
            var setMethodILGenerator = setMethodBuilder.GetILGenerator();
            setMethodILGenerator.Emit(OpCodes.Ldarg_0);
            setMethodILGenerator.Emit(OpCodes.Ldarg_1);
            setMethodILGenerator.Emit(OpCodes.Stfld, field);
            setMethodILGenerator.Emit(OpCodes.Ret);
            property.SetSetMethod(setMethodBuilder);
        }

        private static void DefineConstructors(
            TypeBuilder type,
            IEnumerable<Type> parameterTypes,
            out ILGenerator ilGenerator)
        {
            type.DefineDefaultConstructor(MethodAttributes.Public);

            var constructor = type.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.HasThis,
                parameterTypes.ToArray());

            ilGenerator = constructor.GetILGenerator();
        }
    }
}
