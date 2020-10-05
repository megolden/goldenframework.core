using System;
using FluentAssertions;
using Xunit;

namespace Golden.Common.Tests
{
    public class ContractTests
    {
        [Fact]
        void OnFailureThrow_throws_exception_when_condition_failed()
        {
            Action contract = () => Contract.Requires(false).OnFailureThrow("failed");

            contract.Should().ThrowExactly<Exception>().WithMessage("failed");
        }

        [Fact]
        void OnFailureThrow_throws_exception_with_argument_name_when_condition_failed()
        {
            Action contract = () => Contract.Requires("name", false).OnFailureThrow();

            contract.Should().ThrowExactly<Exception>();
        }

        [Fact]
        void OnFailureThrow_throws_exception_without_argument_name_when_condition_failed()
        {
            Action contract = () => Contract.Requires(false).OnFailureThrow();

            contract.Should().ThrowExactly<Exception>();
        }

        [Fact]
        void OnFailureThrow_dont_throws_any_exception_when_condition_passed()
        {
            Action contract = () => Contract.Requires(true).OnFailureThrow("dummy");

            contract.Should().NotThrow();
        }

        [Fact]
        void OnFailureThrow_throws_specified_exception_when_condition_failed()
        {
            var expectedException = new OutOfMemoryException("failed");

            Action contract = () => Contract.Requires(false).OnFailureThrow(expectedException);

            contract.Should().ThrowExactly<OutOfMemoryException>()
                             .WithMessage("failed")
                             .Which.Equals(expectedException);
        }

        [Fact]
        void OnFailureThrow_throws_specified_exception_type_when_condition_failed()
        {
            Action contract = () => Contract.Requires(false).OnFailureThrow<OutOfMemoryException>();

            contract.Should().ThrowExactly<OutOfMemoryException>();
        }
    }
}
