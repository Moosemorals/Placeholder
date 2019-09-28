using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OcelotPlaceholders;

namespace PlaceholderTests {
    [TestClass]
    public class SimpleTests {
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "Placeholders.csv", "Placeholders#csv", DataAccessMethod.Sequential)]
        public void Basics() {

            string source = Convert.ToString(TestContext.DataRow["source"]);
            string expected = Convert.ToString(TestContext.DataRow["expected"]);
            string description = Convert.ToString(TestContext.DataRow["description"]);

            PlaceholderEngine engine = new PlaceholderEngine();

            Assert.AreEqual(expected, engine.Transform(source), description);
        }

        [TestMethod]
        public void Register() {
            PlaceholderEngine engine = new PlaceholderEngine();

            engine.Register("double", d => d + d);

            Assert.AreEqual("aa", engine.Transform("[double:a]"));
        }

        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "Labels.csv", "Labels#csv", DataAccessMethod.Sequential)]
        public void Labels() {
            IDictionary<string, string> labels = new Dictionary<string, string> {
                { "alpha", "Hello" },
                { "beta", "World" },
                { "Hello", "Nested" },
                { "Hello World", "Now that's just silly" },
                { "Hello:World", "Now that's just silly" },
            };

            PlaceholderEngine engine = new PlaceholderEngine();

            engine.Register("label", arg => labels.ContainsKey(arg) ? labels[arg] : null);
            string source = Convert.ToString(TestContext.DataRow["source"]);
            string expected = Convert.ToString(TestContext.DataRow["expected"]);

            Assert.AreEqual(expected, engine.Transform(source));
        }

        private TestContext testContextInstance;
        public TestContext TestContext {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }
    }


}
