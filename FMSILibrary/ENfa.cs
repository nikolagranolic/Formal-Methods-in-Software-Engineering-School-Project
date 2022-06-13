using System;

namespace FMSILibrary {
    public class ENfa {
        public ENfa() {}
        public ENfa(ENfa other) {
            delta = new(other.delta);
            finalStates = new(other.finalStates);
            startState = new(other.startState);
            currentState = new(other.currentState);
            allStates = new(other.allStates);
            alphabet = new(other.alphabet);
        }
        private Dictionary<(string, char), HashSet<string>> delta = new();
        private HashSet<string> finalStates = new();
        private HashSet<string> startState = new();
        private HashSet<string> currentState = new();
        private HashSet<string> allStates = new();
        private HashSet<char> alphabet = new();
        static int helperID = 0;

        // O(n) -> n - broj stanja u skupu nextState
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
            allStates.Add(state);
        }

        // O(n^3)
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

        // metoda vraca skup koji sadrzi epsilon closure od stanja koje je proslijedjeno kao argument metode
        // O(n^2)
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

        // pocinjemo od pocetnog stanja i naredne skupove stanja dodajemo u red pa onda ih skidamo iz reda pa ponavljamo
        // O(n^4) (zbog minimizacije)
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

        // dodajemo jedno novo pocetno stanje i iz njega epsilon prelaze u pocetna stanja automata m1 i m2
        // O(n)
        public static ENfa Union(ENfa m1, ENfa m2) {
            //return Dfa.Union(m1.ConvertToDfa(), m2.ConvertToDfa()).ConvertToENfa(); // ovo bi bilo O(n^2)
            ENfa result = new();
            foreach(var entry in m1.delta) {
                result.AddTransition(entry.Key.Item1, entry.Key.Item2, entry.Value);
            }
            foreach(var entry in m2.delta) {
                result.AddTransition(entry.Key.Item1, entry.Key.Item2, entry.Value);
            }
            HashSet<string> temp = new();
            foreach(var state in m1.startState)
                temp.Add(state);
            foreach(var state in m2.startState)
                temp.Add(state);
            result.AddTransition("start" + helperID, '$', temp);
            result.SetStartState("start" + helperID++);
            foreach(var state in m1.finalStates)
                result.AddFinalState(state);
            foreach(var state in m2.finalStates)
                result.AddFinalState(state);
            return result;
        }

        // finalna stanja m1 nisu finalna vec od njih dodamo epsilon prelaze u pocetno stanje automata m2
        // O(n^2)
        public static ENfa Concatenation(ENfa m1, ENfa m2) {
            ENfa result = new();
            foreach(var entry in m1.delta) {
                result.AddTransition(entry.Key.Item1, entry.Key.Item2, entry.Value);
            }
            foreach(var entry in m2.delta) {
                result.AddTransition(entry.Key.Item1, entry.Key.Item2, entry.Value);
            }
            string temp = "";
            foreach(var state in m2.startState)
                temp = state;
            foreach(var state in m1.finalStates) {
                HashSet<string> tempSet = new();
                if(result.delta.ContainsKey((state, '$')))
                    foreach(var st in m1.delta[(state, '$')])
                        tempSet.Add(st);
                tempSet.Add(temp);
                if(result.delta.ContainsKey((state, '$')))
                    result.delta.Remove((state, '$'));
                result.AddTransition(state, '$', tempSet);
            }
                
            foreach(var state in m1.startState)
                temp = state;
            result.SetStartState(temp);
            foreach(var state in m2.finalStates)
                result.AddFinalState(state);
            return result;
        }

        // dodavanje novog pocetnog stanja i iz njega epsilon prelaz u staro pocetno stanje(koje je sada i finalno), i iz finalnih stanja starih epsilon prelaz u staro pocetno stanje
        // O(n)
        public static ENfa Star(ENfa m1) {
            ENfa result = new(m1);
            string temp = "";
            foreach(var str in m1.startState)
                temp = str;
            result.startState.Clear();
            result.startState.Add("newStart");
            result.AddFinalState("newStart");
            foreach(var state in result.finalStates)
                result.AddTransition(state, '$', new HashSet<string>{temp});
            return result; 
        }

        // O(n^2)
        public static ENfa Complement(ENfa m1) {
            return Dfa.Complement(m1.ConvertToDfa()).ConvertToENfa();
        }

        // O(n^2)
        public static ENfa Intersection(ENfa m1, ENfa m2) {
            return Dfa.Intersection(m1.ConvertToDfa(), m2.ConvertToDfa()).ConvertToENfa();
        }

        // O(n^2)
        public static ENfa Difference(ENfa m1, ENfa m2) {
            return Dfa.Difference(m1.ConvertToDfa(), m2.ConvertToDfa()).ConvertToENfa();
        }
        
        // O(n^4)
        public int ShortestWordLength() {
            Dfa temp = ConvertToDfa();
            temp.Minimize();
            return temp.ShortestWordLength();
        }

        // O(n^4)
        public int LongestWordLength() {
            Dfa temp = ConvertToDfa();
            temp.Minimize();
            return temp.LongestWordLength();
        }

        // O(n^4)
        public bool IsLanguageFinite() {
            Dfa dfa = this.ConvertToDfa();
            return dfa.IsLanguageFinite();
        }

        // imam neki skup, i vracam skup svih stanja u koja se moze otici kada se elementima pocetnog skupa da simbol neki
        // O(n^2)
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

        // pravi string od skupa stanja
        // O(n)
        private String FromSetToString(HashSet<string> set) {
            String result = "";
            foreach(var str in set)
                result += str;
            if(result != "")
                return result;
            else
                return "dS";
        }
    }
}