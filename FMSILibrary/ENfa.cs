namespace FMSILibrary {
    public class ENfa {
        private Dictionary<(string, char), HashSet<string>> delta = new();
        private HashSet<string> finalStates = new();
        private HashSet<string> startState = new();
        private HashSet<string> currentState = new();
        public void AddTransition(string currentState, char symbol) {
            HashSet<string> nextState = new();
            int nextStateNum;
            string temp;
            Console.WriteLine(currentState + " for symbol " + symbol + ":");
            Console.WriteLine("How many states are in the next state?");
            nextStateNum = int.Parse(Console.ReadLine());
            Console.WriteLine("Adding states...");
            for(int i = 1; i <= nextStateNum; i++) {
                Console.Write("State " + 1 + ":");
                temp = Console.ReadLine();
                nextState.Add(temp);
            }

            delta[(currentState, symbol)] = nextState;
            Console.WriteLine();
        }
        public void SetStartState() {
            HashSet<string> state = new();
            int stateNum;
            string temp;

            Console.WriteLine("How many states are in the start state?");
            stateNum = int.Parse(Console.ReadLine());
            Console.WriteLine("Adding states...");
            for(int i = 1; i <= stateNum; i++) {
                Console.Write("State " + i + ":");
                temp = Console.ReadLine();
                state.Add(temp);
            }

            startState = state;
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
    }
}