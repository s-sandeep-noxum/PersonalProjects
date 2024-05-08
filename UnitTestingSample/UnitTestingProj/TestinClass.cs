using UnitTestingSample;

namespace UnitTestingProj
{
    [TestClass]
    public class TestinClass
    {
        [TestMethod]
        public void CheckVersion()
        {
            MainClass mainClass = new MainClass();
            Assert.IsTrue(mainClass.CheckVersion());
        }
    }
}