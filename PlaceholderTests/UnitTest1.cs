using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaceholderEngine;

namespace PlaceholderTests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void SmokeTets() {

            var data = new string[][] {
               new string[] { "No brackets", "No brackets" },
               
            };

            foreach (string[] pair in data) {
                string source = pair[0];
                string expected = pair[1];

                Assert.AreEqual(expected, Placeholder.Transform(source));
            }
        }
    }
}
