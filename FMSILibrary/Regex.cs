namespace FMSILibrary {
    public class Regex {
        static int n = 0;
        public static ENfa Evaluate(String input) {
            input = PrepareString(input);
            String expr = "(" + input + ")";
            Stack<string> ops = new();
            Stack<string> unionOps = new();
            Stack<ENfa> vals = new();
            Stack<ENfa> unionVals = new();
            List<int> nestedParTracker = new();
            int nestedParCounter = -1;

            for (int i = 0; i < expr.Length; i++) {
                String s = expr.Substring(i, 1);
                if (s.Equals("(")) { 
                    nestedParTracker.Add(0); 
                    nestedParCounter++; 
                }
                else if (s.Equals("+")) {
                    ops.Push(s);
                    nestedParTracker[nestedParCounter]++;
                }
                else if (s.Equals("-")) {
                    ops.Push(s);
                    nestedParTracker[nestedParCounter]++;
                }
                else if (s.Equals("*")) { ENfa v = vals.Pop();
                                          vals.Push(ENfa.Star(v)); }
                else if (s.Equals(")"))
                {
                    int count = nestedParTracker[nestedParCounter];
                    nestedParTracker.RemoveAt(nestedParCounter--);
                    // int count = ops.Count;
                    while (count > 0) {
                        String op = ops.Pop();
                        ENfa v = vals.Pop();
                        if(op.Equals("+")) {
                            unionOps.Push(op);
                            unionVals.Push(v);
                        }
                        //if (op.Equals("+")) v = ENfa.Union(vals.Pop(), v);
                        else if (op.Equals("-")) {
                            v = ENfa.Concatenation(vals.Pop(), v);
                            vals.Push(v);
                        }
                        // else if (op.Equals("*")) {
                        //     v = ENfa.Star(v);
                        //     vals.Push(v);
                        // }
                        count--;

                        
                    }
                    if(unionOps.Count > 0) {
                        int unionCount = unionOps.Count;
                        unionVals.Push(vals.Pop());
                        while (unionCount > 0) {
                        ENfa v = unionVals.Pop();
                        unionVals.Push(ENfa.Union(unionVals.Pop(), v));
                        unionCount--;
                        unionOps.Pop();
                        }
                    }
                    if(unionVals.Count == 1)
                        vals.Push(unionVals.Pop());
                }
                else {
                    ENfa eNfa = new();
                    if(s == "$") {
                        eNfa.SetStartState("q" + n);
                        eNfa.AddFinalState("q" + n++);
                    }
                    else if(s == "O") {
                        eNfa.SetStartState("q" + n++);
                    }
                    else {
                        eNfa.SetStartState("q" + n);
                        eNfa.AddTransition("q" + n++, s.ToCharArray()[0], new HashSet<string>{"q" + n});
                        eNfa.AddFinalState("q" + n++);
                    }
                    vals.Push(eNfa);
                }

            }
            if(vals.Count > 0)
                return vals.Pop();
            else
                return unionVals.Pop();
        }
        public static string PrepareString(string input) {
            HashSet<char> specialCharacters = new HashSet<char>{'+', '-', '*', '(', ')'};
            List<char> str = input.ToList();
            for(int i = 0; i < str.Count - 1; i++) {
                if(!specialCharacters.Contains(str[i]) && !specialCharacters.Contains(str[i + 1])) {
                    str.Insert(i++ + 1, '-');
                }
            }
            for(int i = 0; i < str.Count - 1; i++) {
                if(!specialCharacters.Contains(str[i]) && str[i+1] == '(')
                    str.Insert(i++ + 1, '-');
                if(!specialCharacters.Contains(str[i + 1]) && str[i] == ')')
                    str.Insert(i++ + 1, '-');
                if(!specialCharacters.Contains(str[i + 1]) && str[i] == '*')
                    str.Insert(i++ + 1, '-');    
            }
            char[] str1 = str.ToArray();
            return new String(str1);
        }
    }
}