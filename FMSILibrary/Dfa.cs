namespace FMSILibrary {
    public class Dfa {
        public Dfa() {}
        // copy constructor
        public Dfa(Dfa other) {
            delta = new(other.delta);
            finalStates = new(other.finalStates);
            allStates = new(other.allStates);
            alphabet = new(other.alphabet);
            startState = other.startState;
            currentState = other.currentState;
        }
        private Dictionary<(string, char), string> delta = new();
        private HashSet<char> alphabet = new();
        private HashSet<string> allStates = new();
        private HashSet<string> finalStates = new();

        private string startState = "";
        private string currentState = "";
        public int FinalStates {
            get {
                return finalStates.Count;
            }
        }
        public HashSet<char> getAlphabet() {
            return alphabet;
        }
        public string getStartState() {
            return startState;
        }
        public HashSet<string> getAllStates() {
            return allStates;
        }
        public HashSet<string> getFinalStates() {
            return finalStates;
        }
        public Dictionary<(string, char), string> getDelta() {
            return delta;
        }

        //dodavanje prelaza iz stanja u stanje za odredjeni simbol (sve ovo se cuva u dictionary-ju)
        // O(1)
        public void AddTransition(string currentState, char symbol, string nextState) {
            alphabet.Add(symbol);
            delta[(currentState, symbol)] = nextState;
            allStates.Add(currentState);
            allStates.Add(nextState);
        }
        // O(1)
        public void SetStartState(string state) {
            startState = state;
        }
        // O(1)
        public void AddFinalState(string state) {
            finalStates.Add(state);
        }
        // O(1)
        public void AddState(string state) {
            allStates.Add(state);
        }
        //metoda koja vrsi tranzicije od zadate rijeci simbol po simbol pocevsi od pocetnog stanja
        //i vraca bool kao indikator pripadnosti date rijeci jeziku koji automat opisuje
        // O(n)
        public bool Accepts(string input) {
            var currentState = startState;
            foreach(var symbol in input) {
                if(delta.ContainsKey((currentState, symbol))) // dodano
                    currentState = delta[(currentState, symbol)];
            }
            return finalStates.Contains(currentState);
        }


        // O(n^4)
        public Dfa Minimize() {
            HashSet<string> unreachableStates = new();
            HashSet<string> reachableStates = new();
            reachableStates.Add(startState);
            Queue<string> queue = new();
            queue.Enqueue(startState);
            String temp;

            // odredjivanje stanja koja su dostizna
            // BFS obilazak pomocu reda
            // O(v + e) -> v - broj stanja, e - broj entryja u delti
            while(queue.Count > 0) {
                temp = queue.Dequeue();
                foreach(char symbol in alphabet) {
                    if(delta.ContainsKey((temp, symbol)) && !reachableStates.Contains(delta[(temp, symbol)])) {
                        queue.Enqueue(delta[(temp, symbol)]);
                        reachableStates.Add(delta[(temp, symbol)]);
                    }
                }
            }
            
            // nedostizna su ona koja jesu u allStates a nisu dostizna
            unreachableStates = allStates;
            unreachableStates.ExceptWith(reachableStates);

            // uklanjanje tranzicija iz nedostiznih stanja ka drugim stanjima
            // O(n*a) -> n - broj stanja, a - broj simbola
            foreach(string unrState in unreachableStates) {
                foreach(char symbol in alphabet) {
                    delta.Remove((unrState, symbol));
                }
            }

            allStates = reachableStates;

            // formiranje skupova ekvivalentnih stanja
            HashSet<(string, string)> nonEquivStates = new();
            HashSet<(string, string)> potentiallyEquivStates = new();
            // punjenje skupa neekv. stanja
            // O(n^2)
            foreach(string finalState in finalStates) {
                foreach(string state in allStates) {
                    if(finalState != state && !finalStates.Contains(state)) {
                        nonEquivStates.Add((state, finalState));
                    }
                }
            }
            // punjenje skupa parova finalnih stanja ciju ekvivalenciju provjeravamo, nema duplikata
            // O(n^2)
            foreach(string state1 in finalStates) {
                foreach(string state2 in finalStates) {
                    if(state1 != state2) {
                        if(!potentiallyEquivStates.Contains((state1, state2)) && !potentiallyEquivStates.Contains((state2, state1)))
                            potentiallyEquivStates.Add((state1, state2));
                    }
                }
            }
            HashSet<string> nonFinalStates = new(allStates);
            nonFinalStates.ExceptWith(finalStates);
            // punjenje skupa parova nefinalnih stanja ciju ekvivalenciju provjeravamo, nema duplikata
            // O(n^2)
            foreach(string state1 in nonFinalStates) {
                foreach(string state2 in nonFinalStates) {
                    if(state1 != state2) {
                        if(!potentiallyEquivStates.Contains((state1, state2)) && !potentiallyEquivStates.Contains((state2, state1)))
                            potentiallyEquivStates.Add((state1, state2));
                    }
                }
            }

            // algoritam za minimizaciju - staje se kada broj onih parova ekvivalentnih stanja ostane isti u dvije uzastopne iteracije
            // imamo Sm - skup parova koji nisu sigurno ekvivalentni
            // provjeravamo za neki potencijalno ekvivalentan par da li se od njega za neki simbol ide u Sm, ako da, dodajemo njega u Sm
            int sizeCurr, sizePrev;
            // O(n^4)
            do {
                sizePrev = nonEquivStates.Count;
                // O(n^3)
                foreach(var pair in potentiallyEquivStates) {
                    foreach(char symbol in alphabet) {
                        if(delta.ContainsKey((pair.Item1, symbol)) && delta.ContainsKey((pair.Item2, symbol)) && nonEquivStates.Contains((delta[(pair.Item1, symbol)], delta[(pair.Item2, symbol)]))) {
                            nonEquivStates.Add(pair);
                            potentiallyEquivStates.Remove(pair);
                        }
                        else if(delta.ContainsKey((pair.Item2, symbol)) && delta.ContainsKey((pair.Item1, symbol)) && nonEquivStates.Contains((delta[(pair.Item2, symbol)], delta[(pair.Item1, symbol)]))) {
                            nonEquivStates.Add(pair);
                            potentiallyEquivStates.Remove(pair);
                        }
                    }
                }

                sizeCurr = nonEquivStates.Count;
            } while(sizeCurr != sizePrev);

            // grupisanje parova ekvivalentnih stanja u jedan skup u kom su sva stanja ekvivalentna jedna s drugima
            HashSet<HashSet<string>> equivStates = new();
            // O(n^4)
            foreach(var pair1 in potentiallyEquivStates) { // provjeravamo svaki par sa svakim
                HashSet<string> equivState = new();
                equivState.Add(pair1.Item1);
                equivState.Add(pair1.Item2);
                foreach(var pair2 in potentiallyEquivStates) {
                    if(pair1 != pair2) {
                        if (potentiallyEquivStates.Contains((pair1.Item1, pair2.Item1)) ||
                            potentiallyEquivStates.Contains((pair1.Item1, pair2.Item2)) ||
                            potentiallyEquivStates.Contains((pair1.Item2, pair2.Item1)) ||
                            potentiallyEquivStates.Contains((pair1.Item2, pair2.Item2))) {
                                equivState.Add(pair2.Item1);
                                equivState.Add(pair2.Item2);
                         }
                    }                        
                }

                equivStates.Add(equivState);
            }
            
            Dictionary<(string, char), string> tempDelta = new();
            // punjenje nove delta funkcije prelazima koja ostaju nepromijenjena iz originalne delta funkcije
            // O(n^2)
            foreach(var entry in delta) {
                bool isUnchanged = true;
                foreach(var set in equivStates) {
                    if(set.Contains(entry.Key.Item1))
                    isUnchanged = false;
                }
                if(isUnchanged) {
                    tempDelta[entry.Key] = entry.Value;
                }
            }
            // dodavanje novih delta prelaza (jer sada imamo ekvivalenta stanja koja postaju jedno stanje)
            HashSet<string> tempFinalStates = new(finalStates);
            HashSet<string> namedEquivStates = new();
            String temp1 = "";
            // O(n^2)
            foreach(var states in equivStates) {
                temp = "";
                foreach(string state in states) {
                    temp += state;
                    temp1 = state;
                    finalStates.Remove(state);
                }
                namedEquivStates.Add(temp);
                foreach(char symbol in alphabet) {
                    if(delta.ContainsKey((temp1, symbol)))
                        tempDelta[(temp, symbol)] = delta[(temp1, symbol)];
                }
            }

            // O(n^2)
            foreach(var state in tempFinalStates) { // dodavanje ekv. stanja u skup finalnih stanja ako je neko od tih novoformiranih finalno
                                                    //(sadrzi neko stanje koje je prvobitno bilo finalno)
                foreach(var equivState in namedEquivStates) {
                    if(equivState.Contains(state)) {
                        finalStates.Add(equivState);
                    }
                }
            }

            // O(n^2)
            foreach(string state in namedEquivStates) {
                foreach(var entry in tempDelta) {
                    if(entry.Value != "" && state.Contains(entry.Value)) {
                        tempDelta[entry.Key] = state;
                    }
                }
                if(state.Contains(startState)) // promjena start state-a (po potrebi)
                    startState = state;
            }
            delta = tempDelta;

            // izbacivanje suvisnih prelaza (duplikati) (isti prelazi su iz q3q6q7 i q7q3q6)
            // O(n)
            Dictionary<(string, char), string> tempDelta2 = new();
            foreach(var entry1 in delta) {
                if(!AlreadyInDelta(entry1.Key.Item1, entry1.Key.Item2, tempDelta2)) {
                    tempDelta2[entry1.Key] = entry1.Value;
                }
            }
            delta = tempDelta2;

            allStates.Clear();
            // O(n)
            foreach(var entry in delta) {
                if(!allStates.Contains(entry.Key.Item1))
                    allStates.Add(entry.Key.Item1);
            }
            return this;
        }

        private bool SameState(string str1, string str2) { // provjeravanje da li su skupovi stanja isti (isti su q3q6q7 i q7q3q6)
            char[] first = str1.ToArray();
            char[] second = str2.ToArray();
            Array.Sort(first);
            Array.Sort(second);
            string newStr1 = new string(first);
            string newStr2 = new string(second);
            return newStr1 == newStr2;
        }

        // provjeravanje da li se neki prelaz vec nalazi u delti
        // O(n)
        private bool AlreadyInDelta(string str, char symbol, Dictionary<(string, char), string> tempDelta2) {
            bool flag = false;
            foreach(var entry in tempDelta2) {
                if(SameState(str, entry.Key.Item1) && symbol == entry.Key.Item2)
                    flag = true;
            }
            return flag;
        }

        // konverzija DKA u ENKA
        // isto sve samo sada ne idemo u jedno stanje vec u skup stanja
        // O(n)
        public ENfa ConvertToENfa() {
            ENfa eNfa = new();
            foreach(var entry in delta) {
                eNfa.AddTransition(entry.Key.Item1, entry.Key.Item2, new HashSet<string>{entry.Value});
            }
            foreach(var finalState in finalStates) {
                eNfa.AddFinalState(finalState);
            }
            eNfa.SetStartState(startState);
            return eNfa;
        }

        // Unija dva DKA
        // krenemo od para (s0, s1) gdje su s0 i s1 pocetna stanja prvog i drugog automata i onda dodajemo parove u red kako dolazimo do njih (algoritam sa vjezbi)
        // ovako se odmah odbace nedostizna stanja
        // O(n^2)
        public static Dfa Union(Dfa m1, Dfa m2) {
            Dfa result = new();
            (string, string) temp = new();
            Queue<(string, string)> queue = new();
            HashSet<(string, string)> visitedPairs = new();
            queue.Enqueue((m1.startState, m2.startState));
            result.SetStartState(m1.startState + m2.startState);
            while(queue.Count > 0) {
                temp = queue.Dequeue();
                visitedPairs.Add((temp.Item1, temp.Item2));
                if(m1.finalStates.Contains(temp.Item1) || m2.finalStates.Contains(temp.Item2))
                    result.AddFinalState(temp.Item1 + temp.Item2);
                foreach(char symbol in m1.alphabet) {
                    result.AddTransition(temp.Item1 + temp.Item2, symbol, m1.delta[(temp.Item1, symbol)] + m2.delta[(temp.Item2, symbol)]);
                    if(!visitedPairs.Contains((m1.delta[(temp.Item1, symbol)], m2.delta[(temp.Item2, symbol)])))
                        queue.Enqueue((m1.delta[(temp.Item1, symbol)], m2.delta[(temp.Item2, symbol)]));
                }
            }
            return result;
        }

        // O(n^2)
        public static Dfa Intersection(Dfa m1, Dfa m2) {
            return Dfa.Complement(Dfa.Union(Dfa.Complement(m1), Dfa.Complement(m2))); // De-Morganova formula
        }

        // komplement - invertovanje finalnih stanja
        // O(1)
        public static Dfa Complement(Dfa m1) {
            Dfa result = new(m1);
            result.finalStates = new(result.allStates);
            result.finalStates.ExceptWith(m1.finalStates);
            return result;
        }

        // O(n^2)
        public static Dfa Difference(Dfa m1, Dfa m2) { 
            return Dfa.Intersection(m1, Dfa.Complement(m2)); // formula: A/B = A(Bc)
        }

        // O(v + e)
        public int ShortestWordLength() {
            Queue<string> queue = new();
            queue.Enqueue(startState);
            return ShortestWord(queue);
        }

        // O(v + e)
        public int LongestWordLength() {
            if(!IsLanguageFinite())
                return -1;
            else {
                Stack<string> stack = new();
                HashSet<string> visited = new();
                int rf = 0;
                return LongestWord(startState, stack, visited, ref rf);
            }
        }

        // O(v + e)
        public bool IsLanguageFinite() {
            //Dfa temp = new(this);
            //Minimize();
            bool result = true;
            foreach(var entry in delta) { // prvo provjera da neko fin. stanje ima autotranziciju (ako ima jezik je beskonacan)
                if(entry.Key.Item1 == entry.Value) {
                    if(finalStates.Contains(entry.Key.Item1))
                        return false;
                    else {
                        Queue<string> queue = new();
                        queue.Enqueue(entry.Key.Item1);
                        if(ShortestWord(queue) > 0)
                            return false;
                    }
                }  
            }
            result = Dfs();
            return result;
        }
        
        // O(v + e) -> v - broj stanja, e - broj prelaza
        private bool Dfs() {
            bool result;
            HashSet<string> visited = new();
            string stateInCycle = DfsVisit(startState, visited);
            if(stateInCycle == "")
                return true;
            Queue<string> queue = new();
            queue.Enqueue(stateInCycle);
            if(ShortestWord(queue) > 0)
                result = false;
            else
                result = true;
            return result;
        }

        // O(v + e) -> v - broj stanja, e - broj prelaza
        private string DfsVisit(string state, HashSet<string> visited) {
            if(!visited.SetEquals(allStates)) {
                visited.Add(state);
                foreach(char symbol in alphabet) {
                    if(visited.Contains(delta[(state, symbol)])) {
                        return delta[(state, symbol)];
                    }
                    else {
                        return DfsVisit(delta[(state, symbol)], visited);
                    }
                }
                return "";
            }
            else
                return "";
        }


        // radi se bfs i prvi put kad se dodje do konacnog stanja vraca se ta duzina
        // O(v + e) -> v - broj stanja, e - broj prelaza
        private int ShortestWord(Queue<string> queue, int length = 0) {
            if(length > allStates.Count) // uslov ce biti ispunjen ukoliko nema finalnog stanja dostiznog od elemenata reda
                return -1;
            String temp = queue.Dequeue();
            if(finalStates.Contains(temp))
                return length;
            foreach(char symbol in alphabet) {
                if(!finalStates.Contains(delta[(temp, symbol)]))
                    queue.Enqueue(delta[(temp, symbol)]);
                else
                    return length + 1;
            }
            return ShortestWord(queue, length + 1);
        }

        // O(v + e) -> v - broj stanja, e - broj prelaza
        private int LongestWord(string state, Stack<string> stack, HashSet<string> visited, ref int count, int length = 0) {
            visited.Add(state);
            stack.Push(state);
            foreach(char symbol in alphabet) {
                if(finalStates.Contains(state) && length > count)
                    count = length;
                if(!stack.Contains(delta[(state, symbol)]))
                    LongestWord(delta[(state, symbol)], stack, visited, ref count, length + 1);
            }
            stack.Pop();
            return count;
        }

        // pravljenje stringa od skupa stanja
        // O(n)
        private static String FromSetToString(HashSet<string> set) {
            String result = "";
            foreach(var str in set)
                result += str;
            return result;
        }
    }
}
