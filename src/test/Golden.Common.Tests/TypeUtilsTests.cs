using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class TypeUtilsTests
    {
        [Fact]
        void IsEnumerable_returns_true_with_underlying_type_when_IEnumerable_interface_is_passed()
        {
            var isEnumerable = typeof(IEnumerable<int>).IsEnumerable(out var actualUnderlyingType);

            isEnumerable.Should().BeTrue();
            actualUnderlyingType.Should().Be<int>();
        }

        [Theory]
        [InlineData(typeof(IntEnumerable), typeof(int))]
        [InlineData(typeof(PositiveIntEnumerable), typeof(int))]
        void IsEnumerable_returns_true_with_underlying_type_when_Concrete_class_of_IEnumerable_is_passed(
            Type type,
            Type expectedUnderlyingType)
        {
            var isEnumerable = type.IsEnumerable(out var actualUnderlyingType);

            isEnumerable.Should().BeTrue();
            actualUnderlyingType.Should().Be(expectedUnderlyingType);
        }

        [Fact]
        void IsEnumerable_returns_true_when_IEnumerable_interface_is_passed()
        {
            var isEnumerable = typeof(IEnumerable<int>).IsEnumerable();

            isEnumerable.Should().BeTrue();
        }

        [Fact]
        void IsEnumerable_returns_true_when_Concrete_class_of_IEnumerable_is_passed()
        {
            var isEnumerable = typeof(PositiveIntEnumerable).IsEnumerable();

            isEnumerable.Should().BeTrue();
        }

        [Fact]
        void SetMemberValue_sets_Property_member_value()
        {
            var obj = new TypeMember();
            var property = typeof(TypeMember).GetProperty("PublicProperty");
            var value = 5;

            property.SetMemberValue(value, obj);

            obj.PublicProperty.Should().Be(value);
        }

        [Fact]
        void SetMemberValue_sets_Field_member_value()
        {
            var obj = new TypeMember();
            var field = typeof(TypeMember).GetField("PublicField");
            var value = 5;

            field.SetMemberValue(value, obj);

            obj.PublicField.Should().Be(value);
        }

        [Fact]
        void SetMemberValue_sets_static_Field_member_value()
        {
            var field = typeof(TypeMember).GetField("PublicStaticField");
            var value = 5;

            field.SetMemberValue(value);

            TypeMember.PublicStaticField.Should().Be(value);
        }

        [Fact]
        void SetMemberValue_sets_static_Property_member_value()
        {
            var property = typeof(TypeMember).GetProperty("PublicStaticProperty");
            var value = 5;

            property.SetMemberValue(value);

            TypeMember.PublicStaticProperty.Should().Be(value);
        }

        [Fact]
        void GetMemberValue_gets_Property_member_value()
        {
            var obj = new TypeMember { PublicProperty = 10 };
            var property = typeof(TypeMember).GetProperty("PublicProperty");

            var result = property.GetMemberValue(obj);

            result.Should().Be(10);
        }

        [Fact]
        void GetMemberValue_gets_Field_member_value()
        {
            var obj = new TypeMember { PublicField = 10 };
            var field = typeof(TypeMember).GetField("PublicField");

            var result = field.GetMemberValue(obj);

            result.Should().Be(10);
        }

        [Fact]
        void GetMemberValue_gets_static_Field_member_value()
        {
            TypeMember.PublicStaticField = 10;
            var field = typeof(TypeMember).GetField("PublicStaticField");

            var value = field.GetMemberValue();

            value.Should().Be(10);
        }

        [Fact]
        void GetMemberValue_gets_static_Property_member_value()
        {
            TypeMember.PublicStaticProperty = 10;
            var property = typeof(TypeMember).GetProperty("PublicStaticProperty");

            var value = property.GetMemberValue();

            value.Should().Be(10);
        }

        [Theory]
        [InlineData(typeof(string), null)]
        [InlineData(typeof(int), 0)]
        void DefaultValue_(Type type, object expectedDefaultValue)
        {
            var defaultValue = type.DefaultValue();

            defaultValue.Should().Be(expectedDefaultValue);
        }

        [Fact]
        void GetMemberType_returns_property_type()
        {
            var property = typeof(TypeMember).GetProperty("PublicProperty");

            var type = property.GetMemberType();

            type.Should().Be(property.PropertyType);
        }

        [Fact]
        void GetMemberType_returns_field_type()
        {
            var field = typeof(TypeMember).GetField("PublicField");

            var type = field.GetMemberType();

            type.Should().Be(field.FieldType);
        }

        [Fact]
        void GetMemberType_returns_method_returnType()
        {
            var method = typeof(TypeMember).GetMethod("GetPrivateProperty");

            var type = method.GetMemberType();

            type.Should().Be(method.ReturnType);
        }

        [Fact]
        void GetMemberType_returns_constructor_DeclaringType()
        {
            var constructor = typeof(TypeMember).GetConstructor(Type.EmptyTypes);

            var type = constructor.GetMemberType();

            type.Should().Be(constructor.DeclaringType);
        }

        [Fact]
        void CreateInstance_creates_instance_of_specified_type()
        {
            var type = typeof(FakeType);

            var instance = type.CreateInstance<FakeType>();

            instance.Should().BeOfType<FakeType>();
        }

        [Fact]
        void CreateInstance_creates_instance_of_specified_type_with_constructor_arguments()
        {
            var type = typeof(FakeType);

            var instance = type.CreateInstance<FakeType>(10);

            instance.Should().BeOfType<FakeType>();
            instance.Code.Should().Be(10);
        }

        [Fact]
        void CreateInstance_creates_instance_of_specified_type_with_private_constructor()
        {
            var type = typeof(FakeType);

            var instance = type.CreateInstance<FakeType>(10, "dummy");

            instance.Should().BeOfType<FakeType>();
            instance.Code.Should().Be(10);
            instance.Name.Should().Be("dummy");
        }

        [Fact]
        void CreateType_fails_when_no_properties_specified()
        {
            Action createType_Null = () => TypeUtils.CreateType(properties: null);
            Action createType_Empty = () => TypeUtils.CreateType(new Dictionary<string, Type>());

            createType_Null.Should().Throw<Exception>();
            createType_Empty.Should().Throw<Exception>();
        }

        [Fact]
        void CreateType_creates_a_type_with_specified_properties()
        {
            var type = TypeUtils.CreateType(new Dictionary<string, Type>
            {
                { "Code", typeof(int) },
                { "Name", typeof(string) }
            });

            type.Should().HaveProperty<int>("Code");
            type.Should().HaveProperty<string>("Name");
        }

        [Fact]
        void CreateType_creates_a_type_with_readable_properties()
        {
            var type = TypeUtils.CreateType(new Dictionary<string, Type>
            {
                { "Code", typeof(int) },
                { "Name", typeof(string) }
            });
            var instance = Activator.CreateInstance(type, new object[] { 100, "test" });

            type.GetProperty("Code").GetValue(instance).Should().Be(100);
            type.GetProperty("Name").GetValue(instance).Should().Be("test");
        }

        [Fact]
        void CreateType_creates_a_type_with_writable_properties()
        {
            var type = TypeUtils.CreateType(new Dictionary<string, Type>
            {
                { "Code", typeof(int) },
                { "Name", typeof(string) }
            });
            var instance = Activator.CreateInstance(type);

            type.GetProperty("Code").SetValue(instance, 100);
            type.GetProperty("Name").SetValue(instance, "test");

            instance.Should().BeEquivalentTo(new
            {
                Code = 100,
                Name = "test"
            });
        }

        [Fact]
        void CreateType_creates_a_type_with_default_constructors()
        {
            var properties = new Dictionary<string, Type>
            {
                { "Code", typeof(int) },
                { "Name", typeof(string) }
            };
            var type = TypeUtils.CreateType(properties);

            type.Should().HaveConstructor(Array.Empty<Type>()).Which.IsPublic.Should().BeTrue();
            type.Should().HaveConstructor(properties.Values.ToArray()).Which.IsPublic.Should().BeTrue();
        }

        [Fact]
        void CreateType_creates_a_type_with_a_constructor_with_all_properties_as_argument()
        {
            var type = TypeUtils.CreateType(new Dictionary<string, Type>
            {
                { "Code", typeof(int) },
                { "Name", typeof(string) }
            });
            var instance = Activator.CreateInstance(type, new object[] { 10, "Amin" });

            instance.Should().BeEquivalentTo(new
            {
                Code = 10,
                Name = "Amin"
            });
        }
    }

    class IntEnumerable : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            throw new InvalidOperationException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new InvalidOperationException();
        }
    }
    class PositiveIntEnumerable : IntEnumerable
    {
    }

    public class FakeType
    {
        public int Code { get; private set; }
        public string Name { get; private set; }

        public FakeType()
        {
        }

        public FakeType(int code)
        {
            Code = code;
        }

        private FakeType(int code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
