using System;
class GeneratedDfa {
    public bool StartSimulation(String testString, Reaction beginning, Reaction end, Reaction transitionFor1,Reaction transitionFor0) {
        string currentState = "q0";
        foreach(char symbol in testString) {
            beginning.ExecuteReactions(currentState);
            switch(currentState) {
                case "q0": {
                    if(symbol == '1') {
                        currentState = "q1";
                        transitionFor1.ExecuteReactions(currentState);
                    }
                    if(symbol == '0') {
                        currentState = "q0";
                        transitionFor0.ExecuteReactions(currentState);
                    }
                }
                break;
                case "q1": {
                    if(symbol == '1') {
                        currentState = "q0";
                        transitionFor1.ExecuteReactions(currentState);
                    }
                    if(symbol == '0') {
                        currentState = "q1";
                        transitionFor0.ExecuteReactions(currentState);
                    }
                }
                break;
                case "q2": {
                    if(symbol == '1') {
                        currentState = "q0";
                        transitionFor1.ExecuteReactions(currentState);
                    }
                    if(symbol == '0') {
                        currentState = "q2";
                        transitionFor0.ExecuteReactions(currentState);
                    }
                }
                break;
             }
             end.ExecuteReactions(currentState);
         }
         bool isStringInLanguage;
         switch(currentState) {
             case "q1":
                 isStringInLanguage = true;
                 break;
             case "q2":
                 isStringInLanguage = true;
                 break;
             default:
                 isStringInLanguage = false;
                 break;
         }
         return isStringInLanguage;
    }

}

class Reaction {
    private List<Action<string>> reactions = new();
    public void AddReaction(Action<string> reaction) {
        reactions.Add(reaction);
    }
    public void ExecuteReactions(string state) {
        foreach(Action<string> reaction in reactions) {
            reaction(state);
        }
    }
}
