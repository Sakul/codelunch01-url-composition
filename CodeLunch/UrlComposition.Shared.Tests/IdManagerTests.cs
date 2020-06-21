using FluentAssertions;
using System;
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

        [Theory]
        [InlineData("ncrtchk-2003", "ncrt-2003", true)]
        public void IdCases(string input, string target, bool expected)
        {
            sut.IsMatched(input, target).Should().Be(expected);
        }

        [Theory]
        [ClassData(typeof(SystemCanDecompositionCorrectlyCases))]
        public void SystemCanDecompositionCorrectly(string input, IdComposition expected)
        {
            new TestPattern(input).ActualComposition.Should().BeEquivalentTo(expected);
        }
        class SystemCanDecompositionCorrectlyCases : TheoryData<string, IdComposition>
        {
            public SystemCanDecompositionCorrectlyCases()
            {
                WorkCodeOnly();
            }
            void WorkCodeOnly()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "ncrt"
                };
                Add("ncrt", expected);
            }
        }

        class TestPattern : PatternBase
        {
            public IdComposition ActualComposition => IdComposition;
            public TestPattern(string id) : base(id) { }
            public override bool Match(string id) => throw new NotImplementedException();
        }
    }
}
