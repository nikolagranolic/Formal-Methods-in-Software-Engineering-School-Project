namespace FMSILibrary {
    public class Dfa {
        private Dictionary<(string, char), string> delta = new();
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
            delta[(currentState, symbol)] = nextState;
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
    }
}
