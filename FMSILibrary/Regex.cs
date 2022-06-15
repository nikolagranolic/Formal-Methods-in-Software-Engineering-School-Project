namespace FMSILibrary {
    public class Regex {
        // staticko polje koje koristim pri imenovanju stanja novokreiranih automata
        static int n = 0;

        // evaluacija regexa i kreiranje ENFA od input stringa
        // O(n^2)
        public static ENfa Evaluate(String input) {
            // pripremanje stringa
            input = PrepareString(input);
            // dodavanje spoljnih zagrada
            String expr = "(" + input + ")";
            // stek za obicne operatore (konkatenacija)
            Stack<string> ops = new();
            // stek za uniju
            Stack<string> unionOps = new();
            // stek za vrijednosti kod obicnih operatora
            Stack<ENfa> vals = new();
            // stek za "sabirke" (kod unije)
            Stack<ENfa> unionVals = new();
            // pratim da znam kad dodjem na zatvorenu zagradu koliko unazad idem tj. koliko operatora i operanada da skidam sa steka
            List<int> nestedParTracker = new();
            // indeks aktivne otvorene zagrade u pocetku -1
            int nestedParCounter = -1;
            
            // analiza regexa simbol po simbol
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
                // zvijezda je operator koji se izvrsava trenutno tj. sa steka skidam automat i odmah primjenjujem zvijezdu pa vracam automat nazad
                else if (s.Equals("*")) { ENfa v = vals.Pop();
                                          vals.Push(ENfa.Star(v)); }
                else if (s.Equals(")"))
                {
                    // count - da znam koliko operanada da uzimam, tj. da ne idem skroz dok kraja vec samo do odgovarajuce otvarajuce zagrade
                    int count = nestedParTracker[nestedParCounter];
                    nestedParTracker.RemoveAt(nestedParCounter--);
                    while (count > 0) {
                        String op = ops.Pop();
                        ENfa v = vals.Pop();
                        // pluseve i sabirke prebacujem na stekove za uniju, jer je unija nizeg prioriteta nego konkatenacija
                        if(op.Equals("+")) {
                            unionOps.Push(op);
                            unionVals.Push(v);
                        }
                        else if (op.Equals("-")) {
                            v = ENfa.Concatenation(vals.Pop(), v);
                            vals.Push(v);
                        }
                        count--;

                        
                    }
                    // ovdje rjesavamo "sabirke" (unija je najnizeg prioriteta)
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
                    // ako smo zavrsili evaluaciju zagrade unijom prebacuje se taj automat na stek sa ostalim vrijednostima
                    if(unionVals.Count == 1)
                        vals.Push(unionVals.Pop());
                }
                // slucajevi ako je potrebno da generisemo automat za odgovarajuci jedinicni regularni izraz ($ - epsilon, O - prazan jezik, ili neki simbol)
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
                    // stavljamo to na stek sa vrijednostima
                    vals.Push(eNfa);
                }

            }
            // da vratimo rezultat u zavisnosti od toga da li nam se krajnji automat nalazi na steku za unije ili na steku obicnom za vrijednosti
            if(vals.Count > 0)
                return vals.Pop();
            else
                return unionVals.Pop();
        }

        // funkcija koja dodaje minuse gdje je to potrebno, npr. "ab*a" -> "a-b*-a"
        // O(n)
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
                if(str[i] == '*' && str[i + 1] == '(')
                    str.Insert(i++ + 1, '-');
            }
            char[] str1 = str.ToArray();
            return new String(str1);
        }
    }
}