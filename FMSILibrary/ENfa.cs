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
            string dfaStartState = "";
            foreach(var state in EpsilonClosure(startState)) {
                dfaStartState += state;
            }
            dfa.SetStartState(dfaStartState);
            int size = (int)Math.Pow(2, allStates.Count);
            HashSet<HashSet<string>> powerSet = new();
            HashSet<string> temp = new();
            List<string> tempList = allStates.ToList();
            for(int i = 0; i < size; i++) {
                temp.Clear();
                for(int j = 0; j < tempList.Count; j++) {
                    if((i & (1 << j)) > 0) {
                        temp.Add(tempList[j]);
                    }
                }
                powerSet.Add(new HashSet<string>(temp));
            }
            // dodavanje elemenata partitivnog skupa u skup stanja DKA
            // string tempStr = "";
            // foreach(var set in powerSet) {
            //     foreach(var st in set) {
            //         tempStr += st;
            //     }
            //     dfa.AddState(tempStr);
            //     tempStr = "";
            // }


            return null; // PROMIJENITI NA KRAJU KAD ZAVRSIM FUNKCIJU
        }
        private HashSet<string> FromSetForSymbolToSet(HashSet<string> stateSet, char symbol) {
            HashSet<string> result = new();
            foreach(string state in stateSet) {
                foreach(string stateInClosure in EpsilonClosure(delta[(state, symbol)])) {
                    result.Add(stateInClosure);    
                }
            }
            return result;
        }
    }
}