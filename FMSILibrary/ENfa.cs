using System;

namespace FMSILibrary {
    public class ENfa {
        private Dictionary<(string, char), HashSet<string>> delta = new();
        private HashSet<string> finalStates = new();
        private HashSet<string> startState = new();
        private HashSet<string> currentState = new();
        private HashSet<string> allStates = new();
        private HashSet<char> alphabet = new();
        public void AddTransition(string currentState, char symbol, HashSet<string> nextState) {
            if(symbol != '$')
                    alphabet.Add(symbol);
            delta[(currentState, symbol)] = nextState;
            allStates.Add(currentState);
            foreach(string state in nextState)
                allStates.Add(state);
        }
        public void SetStartState(string state) {
            startState.Add(state);
        }
        public void AddFinalState(string state) {
            finalStates.Add(state);
        }
        public bool Accepts(string input) {
            currentState = EpsilonClosure(startState);    
            foreach(var symbol in input) {
                HashSet<string> temp = new();

                foreach(string state in currentState) {
                    if(delta.ContainsKey((state, symbol))) {
                        foreach(string state1 in delta[(state, symbol)]) {
                            temp.Add(state1);
                        }
                    }
                }
                currentState = EpsilonClosure(temp);
            }

            currentState.IntersectWith(finalStates);
            
            return currentState.Count > 0 ? true : false;
        }

        public HashSet<string> EpsilonClosure(HashSet<string> startState) {
            HashSet<string> eClosure = new();
            Queue<string> queue = new();
            
            foreach(string state in startState) {
                eClosure.Add(state);
                queue.Enqueue(state);

                while(queue.Count > 0) {
                    string temp = queue.Dequeue();

                    if(delta.ContainsKey((temp, '$'))) { 
                        foreach(string tempState in delta[(temp, '$')]) {
                            if(!eClosure.Contains(tempState)) {
                                queue.Enqueue(tempState);
                                eClosure.Add(tempState);
                            }
                        }
                    }
                }
            }
            return eClosure;
        }
        public Dfa ConvertToDfa() {
            Dfa dfa = new();
            Queue<HashSet<string>> queue = new();
            HashSet<string> temp = new();
            temp = EpsilonClosure(startState);
            // postavljanje pocetnog stanja
            dfa.SetStartState(FromSetToString(temp));
            dfa.AddState(FromSetToString(temp));
            temp.Clear();
            queue.Enqueue(EpsilonClosure(startState));
            HashSet<string> visited = new();
            while(queue.Count > 0) {
                temp = queue.Dequeue();
                visited.Add(FromSetToString(temp));
                foreach(char symbol in alphabet) {
                    dfa.AddTransition(FromSetToString(temp), symbol, FromSetToString(FromSetForSymbolToSet(temp, symbol)));
                    dfa.AddState(FromSetToString(temp));
                    if(!visited.Contains(FromSetToString(FromSetForSymbolToSet(temp, symbol)))) {
                        queue.Enqueue(FromSetForSymbolToSet(temp, symbol));
                    }
                }
                foreach(var str in temp) {
                    if(finalStates.Contains(str))
                        dfa.AddFinalState(FromSetToString(temp));
                }
                temp.Clear();
            }
            dfa.Minimize();
            return dfa;
        }
        private HashSet<string> FromSetForSymbolToSet(HashSet<string> stateSet, char symbol) {
            HashSet<string> result = new();
            foreach(string state in stateSet) {
                if(delta.ContainsKey((state, symbol))) {
                    foreach(string stateInClosure in EpsilonClosure(delta[(state, symbol)])) {
                        result.Add(stateInClosure);    
                    }
                }
            }
            return result;
        }
        private String FromSetToString(HashSet<string> set) {
            String result = "";
            foreach(var str in set)
                result += str;
            return result;
        }
    }
}