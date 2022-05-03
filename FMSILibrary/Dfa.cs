namespace FMSILibrary {
    public class Dfa {
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
        //metoda koja vrsi tranzicije od zadate rijeci simbol po simbol pocevsi od pocetnog stanja
        //i vraca bool kao indikator pripadnosti date rijeci jeziku koji automat opisuje
        public bool Accepts(string input) {
            var currentState = startState;
            foreach(var symbol in input) {
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
                        nonEquivStates.Add((finalState, state));
                    }
                }
            }
            // punjenje skupa parova ciju ekvivalenciju provjeravamo
            foreach(string state1 in allStates) {
                foreach(string state2 in allStates) {
                    if(state1 != state2 && !nonEquivStates.Contains((state1, state2)) && !potentiallyEquivStates.Contains((state2, state1))) {
                        potentiallyEquivStates.Add((state1, state2));
                    }
                }
            }

            int sizeCurr, sizePrev;
            do {
                sizePrev = nonEquivStates.Count;

                foreach(var pair in potentiallyEquivStates) {
                    foreach(char symbol in alphabet) {
                        if(nonEquivStates.Contains((delta[(pair.Item1, symbol)], delta[(pair.Item2, symbol)]))) {
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
                    if(state.Contains(entry.Value)) {
                        tempDelta[entry.Key] = state;
                    }
                }
                if(state.Contains(startState)) // promjena start state-a (po potrebi)
                    startState = state;
            }
            delta = tempDelta;
        }
    }
}
