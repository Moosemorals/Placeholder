using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OcelotPlaceholders;

namespace PlaceholderTests {
    [TestClass]
    public class StackTests {
        [TestMethod]
        public void SmokeTests() {

            List<string> input = new List<string>(File.ReadAllLines("Stacks/input-1.txt"));
            List<string> expected = new List<string>(File.ReadAllLines("Stacks/output-1.txt"));

            StackEngine engine = new StackEngine();

            List<string> output = engine.Transform(input);

            CollectionAssert.AreEqual(expected, output); 

        }
    }
}
