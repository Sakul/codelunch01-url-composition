using FluentAssertions;
using Xunit;

namespace UrlComposition.Shared.Tests
{
    public class IdManagerTests
    {
        private IIdManager sut;

        public IdManagerTests()
        {
            sut = new IdManager();
        }

        [Theory]
        [InlineData("ncrtchk-1001", "ncrtchk-2002", false)]
        [InlineData("nprdviw-3030~1234", "nprdviw-3030~1234", true)]
        [InlineData("nprdviw-3030~1234", "nprdviw-3030~200", false)]
        public void StraightforwardCases(string input, string target, bool expected)
        {
            sut.IsMatched(input, target).Should().Be(expected);
        }

        [Theory]
        [InlineData("nprdviw-3030~1234", "~1234", true)]
        [InlineData("ncrtchk-2020~1234", "~1234", true)]
        [InlineData("nprdviw-3030~3456", "~1234", false)]
        public void CorrelationCases(string input, string target, bool expected)
        {
            sut.IsMatched(input, target).Should().Be(expected);
        }
    }
}
