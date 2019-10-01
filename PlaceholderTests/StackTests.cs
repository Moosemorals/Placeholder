using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OcelotPlaceholders;

namespace PlaceholderTests {
    [TestClass]
    public class StackTests {
        [TestMethod]
        public void SmokeTest() {
            List<string> input = new List<string>(File.ReadAllLines("Stacks/input-1.txt"));
            List<string> expected = new List<string>(File.ReadAllLines("Stacks/output-1.txt"));
            StackEngine engine = new StackEngine();
            List<string> output = engine.Transform(input);
            CollectionAssert.AreEqual(expected, output); 
        }

        [TestMethod]
        public void StackTest() {
            List<string> input = new List<string>(File.ReadAllLines("Stacks/input-2.txt"));
            List<string> expected = new List<string>(File.ReadAllLines("Stacks/output-2.txt"));
            StackEngine engine = new StackEngine();
            List<string> output = engine.Transform(input);
            CollectionAssert.AreEqual(expected, output); 
        }

        [TestMethod]
        public void DepthTest() {
            List<string> input = new List<string>(File.ReadAllLines("Stacks/input-3.txt"));
            List<string> expected = new List<string>(File.ReadAllLines("Stacks/output-3.txt"));
            StackEngine engine = new StackEngine();
            List<string> output = engine.Transform(input);
            CollectionAssert.AreEqual(expected, output); 
        }
    }
}
