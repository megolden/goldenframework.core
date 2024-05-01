using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class ObjectUtilsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData((string)null)]
        void IsNull_returns_true_when_null_object_is_passed(object value)
        {
            var result = value.IsNull();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData("")]
        [InlineData("a")]
        void IsNull_returns_false_when_non_null_value_is_passed(object value)
        {
            var result = value.IsNull();

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData("")]
        [InlineData("a")]
        void IsNotNull_returns_true_when_non_null_object_is_passed(object value)
        {
            var result = value.IsNotNull();

            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData((string)null)]
        void IsNotNull_returns_false_when_null_value_is_passed(object value)
        {
            var result = value.IsNotNull();

            result.Should().BeFalse();
        }

        [Fact]
        void GetHashCode_returns_hash_code_of_multiple_values()
        {
            var values = new object[] { 1, 2.5, "a" };

            var hashcode = ObjectUtils.GetHashCode(values);

            hashcode.Should().Be(-1061147664);
        }

        [Fact]
        void IsIn_returns_true_when_values_contains_specified_value()
        {
            var values = new[] { 1, 2, 4 };
            var existValue = 2;

            var result = existValue.IsIn(values);

            result.Should().BeTrue();
        }

        [Fact]
        void IsIn_returns_false_when_values_does_not_contains_specified_value()
        {
            var values = new[] { 1, 2, 4 };
            var notExistValue = 3;

            var result = notExistValue.IsIn(values);

            result.Should().BeFalse();
        }

        [Fact]
        void NotIn_returns_true_when_values_no_contains_specified_value()
        {
            var values = new[] { 1, 2, 4 };
            var notExistValue = 3;

            var result = notExistValue.NotIn(values);

            result.Should().BeTrue();
        }

        [Fact]
        void NotIn_returns_false_when_values_contains_specified_value()
        {
            var values = new[] { 1, 2, 4 };
            var existValue = 2;

            var result = existValue.NotIn(values);

            result.Should().BeFalse();
        }

        [Fact]
        void IsInRange_returns_true_when_value_is_in_specified_range()
        {
            var value = 2;
            var range = (Start: 1, End: 3);

            var result = value.IsInRange(range.Start, range.End);

            result.Should().BeTrue();
        }

        [Fact]
        void IsInRange_returns_false_when_value_does_not_in_specified_range()
        {
            var value = 3;
            var range = (Start: 1, End: 2);

            var result = value.IsInRange(range.Start, range.End);

            result.Should().BeFalse();
        }

        [Fact]
        void IsInRangea_returns_false_when_range_end_value_passed_with_exclusive_mode()
        {
            var value = 2;
            var range = (Start: 1, End: 2);

            var result = value.IsInRange(range.Start, range.End, inclusive: false);

            result.Should().BeFalse();
        }

        [Fact]
        void IsInRangea_returns_true_when_range_end_value_passed_with_inclusive_mode()
        {
            var value = 2;
            var range = (Start: 1, End: 2);

            var result = value.IsInRange(range.Start, range.End, inclusive: true);

            result.Should().BeTrue();
        }

        [Fact]
        void As_converts_value_to_specified_type()
        {
            var list = new List<int> { 1, 2 };

            var asEnumerableResult = list.As<IEnumerable<int>>();

            asEnumerableResult.Should().NotBeNull();
        }

        [Fact]
        void As_returns_default_value_when_conversion_failed()
        {
            var list = new List<int> { 1, 2 };

            var asSetResult = list.As<ISet<int>>();

            asSetResult.Should().BeNull();
        }

        [Fact]
        void Is_returns_true_when_value_is_specified_type()
        {
            var list = new List<int> { 1, 2 };

            var isEnumerableResult = list.Is<IEnumerable<int>>();

            isEnumerableResult.Should().BeTrue();
        }

        [Fact]
        void Is_returns_false_when_value_is_not_specified_type()
        {
            var list = new List<int> { 1, 2 };

            var isSetResult = list.Is<ISet<int>>();

            isSetResult.Should().BeFalse();
        }

        [Fact]
        void NotEqualTo_returns_false_when_equal_value_is_passed()
        {
            int value = 2;
            object equalValue = 2;

            var result = value.NotEqualTo(equalValue);

            result.Should().BeFalse();
        }

        [Fact]
        void NotEqualTo_returns_true_when_not_equal_value_is_passed()
        {
            int value = 2;
            object notEqualValue = 3;

            var result = value.NotEqualTo(notEqualValue);

            result.Should().BeTrue();
        }

        [Fact]
        void SetValue_sets_public_property_value()
        {
            var obj = new TypeMember();

            obj.SetValue("PublicProperty", 1);

            obj.PublicProperty.Should().Be(1);
        }

        [Fact]
        void SetValue_sets_non_public_property_value()
        {
            var obj = new TypeMember();

            obj.SetValue("PrivateProperty", 10);

            obj.GetPrivateProperty().Should().Be(10);
        }

        [Fact]
        void SetValue_sets_public_field_value()
        {
            var obj = new TypeMember();

            obj.SetValue("PublicField", 10);

            obj.PublicField.Should().Be(10);
        }

        [Fact]
        void SetValue_sets_non_public_field_value()
        {
            var obj = new TypeMember();

            obj.SetValue("PrivateField", 10);

            obj.GetPrivateField().Should().Be(10);
        }

        [Fact]
        void SetValue_sets_static_member_value_when_object_passed()
        {
            var obj = new TypeMember();

            obj.SetValue("StaticPrivateField", 10);

            TypeMember.GetStaticPrivateField().Should().Be(10);
        }

        [Fact]
        void SetValue_searches_members_with_no_case_sensitive()
        {
            var obj = new TypeMember();

            obj.SetValue("publicPROPERTY", 10);

            obj.PublicProperty.Should().Be(10);
        }

        [Fact]
        void SetValue_sets_multiple_member_values()
        {
            var obj = new TypeMember();

            obj.SetValue(new
            {
                PublicProperty = 10,
                PrivateProperty = 5
            });

            obj.PublicProperty.Should().Be(10);
            obj.GetPrivateProperty().Should().Be(5);
        }

        [Fact]
        void GetValue_gets_public_property_value()
        {
            var obj = new TypeMember { PublicProperty = 10 };

            var result = obj.GetValue("PublicProperty");

            result.Should().Be(10);
        }

        [Fact]
        void GetValue_gets_public_property_value_as_specified_type()
        {
            var obj = new TypeMember { PublicProperty = 105 };

            var result = obj.GetValue<int>("PublicProperty");

            result.Should().Be(105);
        }

        [Fact]
        void GetValue_gets_non_public_property_value()
        {
            var obj = new TypeMember();
            obj.SetPrivateProperty(10);

            var value = obj.GetValue("PrivateProperty");

            value.Should().Be(10);
        }

        [Fact]
        void GetValue_gets_public_field_value()
        {
            var obj = new TypeMember();
            obj.PublicField = 10;

            var value = obj.GetValue("PublicField");

            value.Should().Be(10);
        }

        [Fact]
        void GetValue_gets_non_public_field_value()
        {
            var obj = new TypeMember();
            obj.SetPrivateField(10);

            var value = obj.GetValue("PrivateField");

            value.Should().Be(10);
        }

        [Fact]
        void GetValue_gets_static_member_value_when_object_passed()
        {
            var obj = new TypeMember();
            TypeMember.SetStaticPrivateField(10);

            var value = obj.GetValue("StaticPrivateField");

            value.Should().Be(10);
        }

        [Fact]
        void GetValue_searches_members_with_no_case_sensitive()
        {
            var obj = new TypeMember { PublicProperty = 11 };

            var value = obj.GetValue("publicPROPERTY");

            value.Should().Be(11);
        }

        [Fact]
        void With_invokes_action_on_passed_object()
        {
            var list = new List<int> { 1, 2, 3 };

            list = list.With(items => items.Remove(2));

            list.Should().BeEquivalentTo(new[] {1, 3});
        }
    }

    class TypeMember
    {
        public int PublicProperty { get; set; }

        private int PrivateProperty { get; set; }
        public int GetPrivateProperty() => PrivateProperty;
        public void SetPrivateProperty(int value) => PrivateProperty = value;

        private int PrivateField;
        public int GetPrivateField() => PrivateField;
        public void SetPrivateField(int value) => PrivateField = value;

        public int PublicField;

        private static int StaticPrivateField;
        public static int GetStaticPrivateField() => StaticPrivateField;
        public static void SetStaticPrivateField(int value) => StaticPrivateField = value;

        public static int PublicStaticField;

        public static int PublicStaticProperty { get; set; }
    }
}
