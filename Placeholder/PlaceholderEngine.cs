using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OcelotPlaceholders {

    /// <summary>
    /// Manage placeholders. Call 'Register' to add new placeholder types, then
    /// call'Transform' with the string to parse.
    /// </summary>
    public class PlaceholderEngine {

        private readonly IDictionary<string, Func<string, string>> Operators = new Dictionary<string, Func<string, string>>();

        /// <summary>
        /// Create a new PlaceholderEngine. "bold" and "link" placeholders are already handled.
        /// </summary>
        public PlaceholderEngine() {
            Register("bold", ParseBold);
            Register("link", ParseLink);
        }

        /// <summary>
        /// Register a placeholder handler
        /// </summary>
        /// <param name="name">The name of the placeholder</param>
        /// <param name="func">The handler. Function that takes a string and returns a string, or null if it can't handle the input.</param>
        public void Register(string name, Func<string, string> func) {
            Operators.Add(name.ToLower(), func);
        }

        /// <summary>
        /// Transform the placeholders in input based on currently registered handlers.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Transform(string input) {
            // Escape the input string
            string result = WebUtility.HtmlEncode(input);
            int offset = 0;

            Stack<int> starts = new Stack<int>();

            while (offset < result.Length) {
                char c = result[offset];

                if (c == '[') {
                    starts.Push(offset);
                } else if (c == ']') {
                    if (starts.Count > 0) {
                        int start = starts.Pop();
                        string text = result.Substring(start + 1, offset - start - 1);
                        string candidate = Parse(text);
                        if (candidate != null) {
                            result = result.Remove(start, offset - start + 1).Insert(start, candidate);
                            offset = start;
                        }
                    }
                }
                offset++;
            }

            return result;
        }

        private string Parse(string text) {
            int colon = text.IndexOf(':');
            if (colon == -1) {
                return null;
            }

            string type = text.Substring(0, colon).ToLower();
            string args = text.Substring(colon + 1);
            if (Operators.ContainsKey(type)) {
                return Operators[type].Invoke(args);
            }

            return null;
        }

        private string ParseBold(string args) {
            return "<strong>" + args +"</strong>";
        }

        private string ParseLink(string args) {
            string[] parts = args.Split(new char[] { ':' }, 2);

            return "<a href='" + parts[1] + "'>" + parts[0] + "</a>";
        }
    }
}
