using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OcelotPlaceholders {
    public class StackEngine {
        public List<string> Transform(List<string> input) {
            // Can't stack single lines or empty list.
            if (input.Count < 2) {
                return input;
            }

            List<StackElement> lines = new List<StackElement>();
            StackElement current = new StackElement(input[0]);

            for (int i = 1; i < input.Count; i += 1) {
                StackElement line = new StackElement(input[i]);
                if (!current.AddChild(line)) {
                    lines.Add(current);
                    current = line;
                }
            }

            lines.Add(current); 

            List<string> result = new List<string>();

            while (lines.Count > 0) {
                StackElement next = lines[0];
                lines.RemoveAt(0);
                lines.InsertRange(0, next.Children);
                result.Add(next.ToString());
            }

            return result;
        }

        internal class StackElement {
            private static readonly Regex WHITESPACE = new Regex(@"\s+");
            private readonly string[] parts;

            private StackElement() {
                Children = new List<StackElement>();
            }

            internal StackElement(StackElement other) : this() {
                parts = other.parts;
                MatchStart = other.MatchEnd;
                MatchEnd = parts.Length;
                Depth = other.Depth + 1;
            }

            internal StackElement(string line) : this() {
                parts = WHITESPACE.Split(line);
                MatchStart = 0;
                MatchEnd = parts.Length;
                Depth = 0;
            }

            internal bool AddChild(StackElement other) {
                for (int i = 0; i < this.MatchEnd; i += 1) {
                    if (this.parts[i] != other.parts[i]) {
                        if (i < 3) {
                            return false;
                        } else {
                            this.MatchEnd = i;
                            other.MatchStart = i;
                            break;
                        }

                    }
                }
                if (Children.Count == 0) {
                    Children.Add(new StackElement(this));
                }
                other.Depth = Depth + 1;
                Children.Add(other);
                return true;
            }

            public void Recurse() {
                if (Children.Count < 2) {
                    return;
                }

                StackElement current = Children[0];
                for (int i = 1; i < Children.Count; i += 1) {
                    StackElement line = Children[i];
                    if (!current.AddChild(line)) {
                        current.Recurse();
                        current = line;
                    }
                }
            }

            public override string ToString() {
                StringBuilder result = new StringBuilder();

                result.Append('\t', Depth);

                bool first = true;
                for (int i = MatchStart; i < MatchEnd; i += 1) {
                    if (!first) {
                        result.Append(" ");
                    }
                    result.Append(parts[i]);
                    first = false;
                }

                return result.ToString();
            }

            internal int Depth { get; set; }
            internal int MatchStart { get; set; }
            internal int MatchEnd { get; set; }
            internal List<StackElement> Children { get; set; }
        }
    }
}
