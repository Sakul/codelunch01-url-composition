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
        [ClassData(typeof(SystemMustNotDecompositionInvalidInputCases))]
        public void SystemCanDecompositionCorrectly(string input, IdComposition expected)
        {
            new TestPattern(input).ActualComposition.Should().BeEquivalentTo(expected);
        }
        class SystemCanDecompositionCorrectlyCases : TheoryData<string, IdComposition>
        {
            public SystemCanDecompositionCorrectlyCases()
            {
                MinimumRequirement();
                MinimumRequirement_Without_Correlation();
                MinimumRequirement_Without_Step();
                FullCases();
            }
            void MinimumRequirement()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "ncrt",
                    Operation = "chk",
                    Id = "0001",
                };
                Add("ncrtchk-0001", expected);
            }
            void MinimumRequirement_Without_Correlation()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "ncrt",
                    Operation = "chk",
                    Step = "1",
                    Id = "0001",
                };
                Add("ncrtchk.1-0001", expected);
            }
            void MinimumRequirement_Without_Step()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "ncrt",
                    Operation = "chk",
                    Id = "0001",
                    Correlation = "9999",
                };
                Add("ncrtchk-0001~9999", expected);
            }
            void FullCases()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "ncrt",
                    Operation = "chk",
                    Step = "1",
                    Id = "0001",
                    Correlation = "9999",
                };
                Add("ncrtchk.1-0001~9999", expected);
            }
        }
        class SystemMustNotDecompositionInvalidInputCases : TheoryData<string, IdComposition>
        {
            public SystemMustNotDecompositionInvalidInputCases()
            {
                NoId();
                StateCodeInvalid();
                TooShort();
                InvalidInput();
            }
            void StateCodeInvalid()
            {
                Add("crtchk.1-0001~9999", null);
                Add("Ncrtchk.1-0001~9999", null);
                Add("Mcrtchk.1-0001~9999", null);
            }
            void NoId()
            {
                Add("ncrt", null);
                Add("ncrtchk.1", null);
                Add("ncrtchk~9999", null);
                Add("ncrtchk.1~9999", null);
            }
            void TooShort()
            {
                Add("n-0001", null);
                Add("nc-0001", null);
                Add("ncr-0001", null);
                Add("ncrt-0001", null);
                Add("ncrtc-0001", null);
                Add("ncrtch-0001", null);
            }
            void InvalidInput()
            {
                Add(string.Empty, null);
                Add(" ", null);
                Add(null, null);
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
