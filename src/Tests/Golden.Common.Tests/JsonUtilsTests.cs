using FluentAssertions;
using Xunit;
using static System.Text.Json.JsonSerializer;

namespace Golden.Common.Tests
{
    public class JsonUtilsTests
    {
        [Fact]
        void ToJson_serializes_an_object_to_string_with_json_format()
        {
            var obj = new Fake { Id = 10, Name = "Test" };

            var result = obj.ToJson();

            var expectedObj = Deserialize<Fake>(result);
            expectedObj.Should().BeEquivalentTo(obj);
        }

        [Fact]
        void JsonTo_deserializes_a_json_string_to_specified_type()
        {
            var json = @"{""Id"":10,""Name"":""Sample""}";
            var expectedObj = new Fake { Id = 10, Name = "Sample" };

            var result = json.JsonTo<Fake>();

            result.Should().BeEquivalentTo(expectedObj);
        }

        [Fact]
        void JsonTo_deserializes_a_specified_property_of_json_string()
        {
            var json = @"{""Id"":10,""Name"":""Sample""}";
            var propertyName = "Name";
            var expectedValue = "Sample";

            var result = json.JsonTo<string>(propertyName);

            result.Should().Be(expectedValue);
        }

        [Fact]
        void JsonTo_deserializes_a_specified_nested_property_of_json_string()
        {
            var json = @"{""Nested"":{""Name"":""Sample""}}";
            var propertyName = "Nested.Name";
            var expectedValue = "Sample";

            var result = json.JsonTo<string>(propertyName);

            result.Should().Be(expectedValue);
        }

        class Fake
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Fake Nested { get; set; }
        }
    }
}
