using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OcelotPlaceholders {
    public class StackEngine {
        private static readonly int StackDepth = 3;
        private static readonly Regex Whitespace = new Regex(@"\s+");

        public List<string> Transform(List<string> input) {
            Word root = BuildTree(input);

            List<string> result = new List<string>();
            foreach (Word w in root.Next) {
                result.AddRange(BuildDisplay(w, 0));
            }
            return result;
        }

        private Word BuildTree(List<string> input) {
            // Phase one - Convert input (a list of strings) into a 'Trie' (https://en.wikipedia.org/wiki/Trie) of words. 

            // Start with an empty string. We'll discard it later
            // but it gives us somwhere to hold the results.
            Word root = new Word("");
            foreach (string line in input) {
                // Split each line into words
                string[] words = Whitespace.Split(line);
                Word current = root;

                // Then walk along the list words
                foreach (string word in words) {
                    
                    // If the current word doesn't have any children (that is, it's the end of a line)
                    // Or the most recently added child (from the previous line) isn't a match
                    // then add a new child.
                    // Otherwise, this line and the previous line match
                    if (current.Next.Count == 0 || current.Next.Last().Text != word) {
                        Word next = new Word(word);
                        current.Next.Add(next);
                        current = next;
                    } else {
                        current = current.Next.Last();
                    }
                }
            }

            return root;
        }

        private List<string> BuildDisplay(Word root, int depth) {
            // Phase two - turn the Trie into a list of sentances

            StringBuilder phrase = new StringBuilder();
            // Add indents
            phrase.Append('\t', depth);

            List<string> result = new List<string>();
            Word current = root;
            int length = 0;

            // While there's exactly one child (zero children 
            // is end of an input line, more than one child 
            // is a potential branch point), build up a
            // prhase from the Trie.
            while (true) {
                if (length > 0) {
                    phrase.Append(" ");
                }
                phrase.Append(current.Text);
                length += 1;
                if (current.Next.Count != 1) {
                    break;
                }
                current = current.Next[0];
            }

            if (length < StackDepth && current.Next.Count > 0) {
                // Busines logic: The constructed phrases must have at least
                // StackDepth (probably 3) words, unless they're the end of a line.
                // If we're here, we've got a branch that happened too soon,
                // so we need to duplicate it and re-build the input lines
                foreach (Word next in current.Next) {
                    List<string> partials = BuildDisplay(next, 0);
                    foreach (string s in partials) { 
                        result.Add(phrase.ToString() + " " + s);
                    }
                }
            } else {
                // Add what we've got so far to the result
                result.Add(phrase.ToString());
                // If we're at a branch, build up the child phrases and
                // add them to the result
                if (current.Next.Count > 0) {
                    foreach (Word next in current.Next) {
                        result.AddRange(BuildDisplay(next, depth + 1));
                    }
                }
            }

            return result;
        }

        internal class Word {
            internal Word(string word) {
                Next = new List<Word>();
                Text = word;
            }
            internal List<Word> Next { get; }
            internal string Text { get; }
        }
    }
}
