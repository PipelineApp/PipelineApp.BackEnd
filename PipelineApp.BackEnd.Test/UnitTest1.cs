using System;
using Xunit;

namespace PipelineApp.BackEnd.Test
{
    using FluentAssertions;

    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            1.Should().Be(1);
        }
    }
}
