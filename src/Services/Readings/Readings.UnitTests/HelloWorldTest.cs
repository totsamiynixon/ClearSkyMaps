using System;
using Xunit;

namespace Readings.UnitTests
{
    public class HelloWorldTest
    {
        [Fact]
        public void TestHelloWorld()
        {
            Console.WriteLine("Hello from xUnit");
            Assert.Contains("Hello from xUnit", "Hello from xUnit");
        }
    }
}
