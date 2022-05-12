namespace FMSILibrary {
    public class Dfa {
        public Dfa() {}
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
        public string StartState {
            get {
                return startState;
            }
            set {
                startState = value;
            }
        }
        public string CurrentState {
            get {
                return currentState;
            }
            set {
                currentState = value;
            }
        }
        //dodavanje prelaza iz stanja u stanje za odredjeni simbol (sve ovo se cuva u dictionary-ju)
        public void AddTransition(string currentState, char symbol, string nextState) {
            alphabet.Add(symbol);
            delta[(currentState, symbol)] = nextState;
            allStates.Add(currentState);
            allStates.Add(nextState);
        }
        public void SetStartState(string state) {
            startState = state;
        }
        public void AddFinalState(string state) {
            finalStates.Add(state);
        }
        public void AddState(string state) {
            allStates.Add(state);
        }
        //metoda koja vrsi tranzicije od zadate rijeci simbol po simbol pocevsi od pocetnog stanja
        //i vraca bool kao indikator pripadnosti date rijeci jeziku koji automat opisuje
        public bool Accepts(string input) {
            var currentState = startState;
            foreach(var symbol in input) {
                if(delta.ContainsKey((currentState, symbol))) // dodano
                    currentState = delta[(currentState, symbol)];
            }
            return finalStates.Contains(currentState);
        }

        public void Minimize() {
            // uklanjanje nedostiznih stanja
            HashSet<string> unreachableStates = new();
            HashSet<string> reachableStates = new();
            reachableStates.Add(startState);
            Queue<string> queue = new();
            queue.Enqueue(startState);
            String temp;

            while(queue.Count > 0) {
                temp = queue.Dequeue();
                foreach(char symbol in alphabet) {
                    if(delta.ContainsKey((temp, symbol)) && !reachableStates.Contains(delta[(temp, symbol)])) {
                        queue.Enqueue(delta[(temp, symbol)]);
                        reachableStates.Add(delta[(temp, symbol)]);
                    }
                }
            }
            
            unreachableStates = allStates;
            unreachableStates.ExceptWith(reachableStates);

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
            foreach(string finalState in finalStates) {
                foreach(string state in allStates) {
                    if(finalState != state && !finalStates.Contains(state)) {
                        nonEquivStates.Add((state, finalState));
                        //nonEquivStates.Add((finalState, state));
                    }
                }
            }
            // punjenje skupa parova ciju ekvivalenciju provjeravamo
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
            foreach(string state1 in nonFinalStates) {
                foreach(string state2 in nonFinalStates) {
                    if(state1 != state2) {
                        if(!potentiallyEquivStates.Contains((state1, state2)) && !potentiallyEquivStates.Contains((state2, state1)))
                            potentiallyEquivStates.Add((state1, state2));
                    }
                }
            }

            // foreach(string state1 in allStates) {
            //     foreach(string state2 in allStates) {
            //         if(state1 != state2 && !nonEquivStates.Contains((state1, state2)) && !potentiallyEquivStates.Contains((state2, state1))) {
            //             potentiallyEquivStates.Add((state1, state2));
            //         }
            //     }
            // }

            int sizeCurr, sizePrev;
            do {
                sizePrev = nonEquivStates.Count;

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

            HashSet<HashSet<string>> equivStates = new();
            foreach(var pair1 in potentiallyEquivStates) {
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
            HashSet<string> tempFinalStates = new(finalStates);
            HashSet<string> namedEquivStates = new();
            String temp1 = "";
            foreach(var states in equivStates) {
                temp = "";
                foreach(string state in states) {
                    temp += state;
                    temp1 = state;
                    finalStates.Remove(state);
                }
                namedEquivStates.Add(temp);
                foreach(char symbol in alphabet) {
                    if(delta.ContainsKey((temp1, symbol))) // dodano
                        tempDelta[(temp, symbol)] = delta[(temp1, symbol)];
                }
            }

            foreach(var state in tempFinalStates) {
                foreach(var equivState in namedEquivStates) {
                    if(equivState.Contains(state)) {
                        finalStates.Add(equivState);
                    }
                }
            }
            //finalStates = tempFinalStates;

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
            Dictionary<(string, char), string> tempDelta2 = new();
            foreach(var entry1 in delta) {
                if(!AlreadyInDelta(entry1.Key.Item1, entry1.Key.Item2, tempDelta2)) {
                    tempDelta2[entry1.Key] = entry1.Value;
                }
            }
            delta = tempDelta2;

            allStates.Clear();
            foreach(var entry in delta) {
                if(!allStates.Contains(entry.Key.Item1))
                    allStates.Add(entry.Key.Item1);
            }
        }
        private bool SameState(string str1, string str2) {
            char[] first = str1.ToArray();
            char[] second = str2.ToArray();
            Array.Sort(first);
            Array.Sort(second);
            string newStr1 = new string(first);
            string newStr2 = new string(second);
            return newStr1 == newStr2;
        }
        private bool AlreadyInDelta(string str, char symbol, Dictionary<(string, char), string> tempDelta2) {
            bool flag = false;
            foreach(var entry in tempDelta2) {
                if(SameState(str, entry.Key.Item1) && symbol == entry.Key.Item2)
                    flag = true;
            }
            return flag;
        }

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
        public static Dfa Intersection(Dfa m1, Dfa m2) {
            return Dfa.Complement(Dfa.Union(Dfa.Complement(m1), Dfa.Complement(m2))); // De-Morganova formula
        }
        public static Dfa Complement(Dfa m1) {
            Dfa result = new(m1);
            result.finalStates = new(result.allStates);
            result.finalStates.ExceptWith(m1.finalStates);
            return result;
        }
        public static Dfa Difference(Dfa m1, Dfa m2) { 
            return Dfa.Intersection(m1, Dfa.Complement(m2)); // formula: A/B = A(Bc)
        }
        public int ShortestWordLength() {
            Queue<string> queue = new();
            queue.Enqueue(startState);
            return ShortestWord(queue);
        }
        // public bool IsLanguageFinite() {
        //     Minimize();                         // <--- da bi ukoliko ciklus postoji i ne sadrzi finalno stanje a da se iz ciklusa ne mozemo vratiti nazad
        //     Queue<string> queue = new();        // do finalnog stanja da taj ciklus zamijenimo jednim neekvivalentnim stanjem pa da onda sigurno znamo da
        //     HashSet<string> visited = new();    // ciklus koji smo detektovali sigurno sadrzi ciklus
        //     queue.Enqueue(startState);
        //     while(queue.Count > 0) {
        //         String temp = queue.Dequeue();
        //         visited.Add(temp);
        //         foreach(char symbol in alphabet) {
        //             if(!visited.Contains(delta[(temp, symbol)]))
        //                 queue.Enqueue(delta[(temp, symbol)]));
        //             else
        //                 return true;
        //         }

        //     }
        // }
        private int ShortestWord(Queue<string> queue, int length = 0) {
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
        private static String FromSetToString(HashSet<string> set) {
            String result = "";
            foreach(var str in set)
                result += str;
            return result;
        }
    }
}
