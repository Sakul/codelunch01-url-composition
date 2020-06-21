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
        [ClassData(typeof(SystemCanDecompositionInvalidInputCases))]
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
                    Work = "crt",
                    Operation = "chk",
                    Id = "0001",
                    IsValid = true,
                };
                Add("ncrtchk-0001", expected);
            }
            void MinimumRequirement_Without_Correlation()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Operation = "chk",
                    Step = "1",
                    Id = "0001",
                    IsValid = true,
                };
                Add("ncrtchk.1-0001", expected);
            }
            void MinimumRequirement_Without_Step()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Operation = "chk",
                    Id = "0001",
                    Correlation = "9999",
                    IsValid = true,
                };
                Add("ncrtchk-0001~9999", expected);
            }
            void FullCases()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Operation = "chk",
                    Step = "1",
                    Id = "0001",
                    Correlation = "9999",
                    IsValid = true,
                };
                Add("ncrtchk.1-0001~9999", expected);
            }
        }
        class SystemCanDecompositionInvalidInputCases : TheoryData<string, IdComposition>
        {
            public SystemCanDecompositionInvalidInputCases()
            {
                InvalidStateCode_Not_m_Or_n();
                InvalidStateCode_UpperCasse_N();
                InvalidStateCode_UpperCasse_M();
                StateCodeOnly_n();
                StateCodeOnly_m();
                StateCodeOnly_N();
                StateCodeOnly_M();
                WorkOnly();
                WorkOnly_CaseSensitive();
                StepOnly();
                CorrelationOnly();
                StateAndWorkOnly();
                StateAndStepOnly();
                StateAndIdOnly();
                StateAndCorrelationOnly();
                WorkAndStepOnly();
                WorkAndIdOnly();
                WorkAndCorrelationOnly();
                StepAndIdOnly();
                StepAndCorrelationOnly();
                TooLongOperation();
                TooLongStep();
                TooLongId();
                TooLongCorrelation();
                InvalidInput();
            }
            void InvalidStateCode_Not_m_Or_n()
            {
                var expected = new IdComposition
                {
                    StateCode = null,
                    Work = "*cr",
                    Operation = "tchk",
                    Step = "1",
                    Id = "0001",
                    Correlation = "9999",
                };
                Add("*crtchk.1-0001~9999", expected);
            }
            void InvalidStateCode_UpperCasse_N()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Operation = "chk",
                    Step = "1",
                    Id = "0001",
                    Correlation = "9999"
                };
                Add("Ncrtchk.1-0001~9999", expected);
            }
            void InvalidStateCode_UpperCasse_M()
            {
                var expected = new IdComposition
                {
                    StateCode = 'm',
                    Work = "crt",
                    Operation = "chk",
                    Step = "1",
                    Id = "0001",
                    Correlation = "9999"
                };
                Add("Mcrtchk.1-0001~9999", expected);
            }
            void StateCodeOnly_n()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                };
                Add("n", expected);
            }
            void StateCodeOnly_m()
            {
                var expected = new IdComposition
                {
                    StateCode = 'm',
                };
                Add("m", expected);
            }
            void StateCodeOnly_N()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                };
                Add("N", expected);
            }
            void StateCodeOnly_M()
            {
                var expected = new IdComposition
                {
                    StateCode = 'm',
                };
                Add("M", expected);
            }
            void WorkOnly()
            {
                var expected = new IdComposition
                {
                    Work = "crt"
                };
                Add("crt", expected);
            }
            void WorkOnly_CaseSensitive()
            {
                var expected = new IdComposition
                {
                    Work = "cRt"
                };
                Add("cRt", expected);
            }
            void StepOnly()
            {
                var expected = new IdComposition
                {
                    Step = "1",
                };
                Add(".1", expected);
            }
            void CorrelationOnly()
            {
                var expected = new IdComposition
                {
                    Correlation = "9999",
                };
                Add("~9999", expected);
            }
            void StateAndWorkOnly()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                };
                Add("ncrt", expected);
            }
            void StateAndStepOnly()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Step = "1",
                };
                Add("n.1", expected);
            }
            void StateAndIdOnly()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Id = "0001",
                };
                Add("n-0001", expected);
            }
            void StateAndCorrelationOnly()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Correlation = "9999",
                };
                Add("n~9999", expected);
            }
            void WorkAndStepOnly()
            {
                var expected = new IdComposition
                {
                    Work = "crt",
                    Step = "1",
                };
                Add("crt.1", expected);
            }
            void WorkAndIdOnly()
            {
                var expected = new IdComposition
                {
                    Work = "crt",
                    Id = "0001",
                };
                Add("crt-0001", expected);
            }
            void WorkAndCorrelationOnly()
            {
                var expected = new IdComposition
                {
                    Work = "crt",
                    Correlation = "9999",
                };
                Add("crt~9999", expected);
            }
            void StepAndIdOnly()
            {
                var expected = new IdComposition
                {
                    Step = "1",
                    Id = "0001",
                };
                Add(".1-0001", expected);
            }
            void StepAndCorrelationOnly()
            {
                var expected = new IdComposition
                {
                    Step = "1",
                    Correlation = "9999",
                };
                Add(".1~9999", expected);
            }
            void TooLongOperation()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Operation = "operation777",
                };
                Add("ncrtoperation777", expected);
            }
            void TooLongStep()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Step = "0123456789abcdefgh",
                };
                Add("ncrt.0123456789abcdefgh", expected);
            }
            void TooLongId()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Id = "77777777777777777777",
                };
                Add("ncrt-77777777777777777777", expected);
            }
            void TooLongCorrelation()
            {
                var expected = new IdComposition
                {
                    StateCode = 'n',
                    Work = "crt",
                    Correlation = "99999999999999",
                };
                Add("ncrt~99999999999999", expected);
            }
            void InvalidInput()
            {
                var expected = new IdComposition();
                Add(string.Empty, expected);
                Add(" ", expected);
                Add(null, expected);
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
