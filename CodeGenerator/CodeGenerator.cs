using FMSILibrary;
using System;

class CodeGenerator {
    string generatedCode = "";
    Dfa dfaFromSpecification = new();

    public CodeGenerator(string sourceFile) {
        Dfa dfa = new();
        int counter = 0;
        int irregularLinesCounter = 0;
        string[] lines = System.IO.File.ReadAllLines(sourceFile);
        if(lines.Length < 2)
            throw new Exception("Fajl prazan ili ne sadrzi dovoljno linija! Obavezne linije:\n1. linija = \"[vrsta reprezentacije jezika],[stringovi],...\"\n2. linija = \"[pocetno stanje];[finalna stanja],...\" u slucaju automata ili string u slucaju regexa");
        
        string[] firstLine = lines[counter++].Split(',');
        
        if(firstLine[0] == "DFA") {
            if(lines.Length < 3) {
                irregularLinesCounter++;
            }
            else {
                
                if(lines[counter].Contains(';')) {
                    string[] startAndFinalStates = lines[counter++].Split(';');
                    if (startAndFinalStates[1] == "")
                        irregularLinesCounter++;
                    string[] finalStates = startAndFinalStates[1].Split(',');
                    
                    string startState = startAndFinalStates[0];
                    dfaFromSpecification.SetStartState(startState);
                    
                    
                    
                    for(int i = 0; i < finalStates.Length; i++) {
                        if(finalStates[i] != "")
                            dfaFromSpecification.AddFinalState(finalStates[i]);
                    }
                }
                else {
                    dfaFromSpecification.SetStartState(lines[counter++]);
                }
                for(int i = counter; i < lines.Length; i++) {
                    if(lines[counter].Contains('=')) {
                        string[] tranzicija = lines[counter++].Split('=');
                        if(tranzicija.Length == 2) {
                            string destination = tranzicija[1];
                            if(tranzicija[0].Contains(',')) {
                                string[] stanjeISimbol = tranzicija[0].Split(',');
                                if(stanjeISimbol.Length == 2) {
                                    string source = stanjeISimbol[0];
                                    if(stanjeISimbol[1].Length == 1) {
                                        char symbol = stanjeISimbol[1].ToCharArray()[0];
                                        try {
                                            dfaFromSpecification.AddTransition(source, symbol, destination);
                                        }
                                        catch(Exception e) {
                                            e.ToString();
                                            irregularLinesCounter++;
                                        } 
                                    }
                                    else irregularLinesCounter++;
                                }
                                else irregularLinesCounter++;
                            }
                            else irregularLinesCounter++;
                        }
                        else irregularLinesCounter++;
                    }
                    else {
                        irregularLinesCounter++;
                        counter++;
                    }
                }
                if(irregularLinesCounter != 0) {
                    throw new Exception("Specifikacija sadrzi linije koje nisu ispravne!");
                }

                
            } 
        }
        else throw new Exception("Neispravna specifikacija!");

        generatedCode += "using System;\n" +
                             "class GeneratedDfa {\n";
        generatedCode += "    public bool StartSimulation(String testString, Reaction beginning, Reaction end, " + TransitionReactionsArgument() + ") {\n";
        generatedCode += "        string currentState = \"" + dfaFromSpecification.getStartState() + "\";\n";
        generatedCode += "        foreach(char symbol in testString) {\n";
        generatedCode += "            beginning.ExecuteReactions(currentState);\n";
        generatedCode += "            switch(currentState) {\n";
        foreach(string state in dfaFromSpecification.getAllStates()) {
            generatedCode += "                case \"" + state + "\": {\n";
            foreach(char symbol in dfaFromSpecification.getAlphabet()) {
                generatedCode += "                    if(symbol == \'" + symbol + "\') {\n";
                generatedCode += "                        currentState = \"" + dfaFromSpecification.getDelta()[(state, symbol)] + "\";\n";
                generatedCode += "                        transitionFor" + symbol + ".ExecuteReactions(currentState);\n";
                generatedCode += "                    }\n";
            }
            generatedCode += "                }\n";
            generatedCode += "                break;\n";
        }
        generatedCode += "             }\n";
        generatedCode += "             end.ExecuteReactions(currentState);\n";
        generatedCode += "         }\n";
        generatedCode += "         bool isStringInLanguage;\n";
        generatedCode += "         switch(currentState) {\n";
        foreach(string finalState in dfaFromSpecification.getFinalStates()) {
            generatedCode += "             case \"" + finalState + "\":\n";
            generatedCode += "                 isStringInLanguage = true;\n";
            generatedCode += "                 break;\n";
        }
        generatedCode += "             default:\n";
        generatedCode += "                 isStringInLanguage = false;\n";
        generatedCode += "                 break;\n";
        generatedCode += "         }\n";
        generatedCode += "         return isStringInLanguage;\n";
        generatedCode += "    }\n\n";
        generatedCode += "    static public void Main(string[] args) {\n";
        generatedCode += "        \n";
        generatedCode += "    }\n";
        generatedCode += "}\n\n";
        generatedCode += "class Reaction {\n";
        generatedCode += "    private List<Action<string>> reactions = new();\n";
        generatedCode += "    public void AddReaction(Action<string> reaction) {\n";
        generatedCode += "        reactions.Add(reaction);\n";
        generatedCode += "    }\n";
        generatedCode += "    public void ExecuteReactions(string state) {\n";
        generatedCode += "        foreach(Action<string> reaction in reactions) {\n";
        generatedCode += "            reaction(state);\n";
        generatedCode += "        }\n";
        generatedCode += "    }\n";
        generatedCode += "}\n";

        Console.WriteLine(generatedCode);
    }


    private string TransitionReactionsArgument() {
        string result = "";
        char[] alphabet = new char[dfaFromSpecification.getAlphabet().Count];
        dfaFromSpecification.getAlphabet().CopyTo(alphabet);
        for(int i = 0; i < alphabet.Length; i++) {
            result += "Reaction transitionFor" + alphabet[i];
            if(i != alphabet.Length - 1)
                result += ",";
        }
        return result;
    }
    
    static public void Main(string[] args) {
        CodeGenerator codeGenerator = new CodeGenerator("specification.txt");
        //Console.WriteLine("Hello");

        // Reaction start = new();
        // start.AddReaction((string a) => Console.WriteLine(a + "START"));
        // Reaction end = new();
        // end.AddReaction((string a) => Console.WriteLine(a + "END"));
        // Reaction transition1 = new();
        // transition1.AddReaction((string a) => Console.WriteLine(a + "transition1"));
        // Reaction transition0 = new();
        // transition0.AddReaction((string a) => Console.WriteLine(a + "transition0"));
        // GeneratedDfa test = new();
        // Console.WriteLine(test.StartSimulation("1011", start, end, transition1, transition0));
    }
}