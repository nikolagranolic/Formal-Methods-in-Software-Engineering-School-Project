using FMSILibrary;
using System;

class SpecificationAnalyzer {
    static public void Main(string[] args) {
        int counter = 0;
        int irregularLinesCounter = 0;
        string[] lines = System.IO.File.ReadAllLines("specification.txt");
        if(lines.Length < 2)
            throw new Exception("Fajl prazan ili ne sadrzi dovoljno linija! Obavezne linije:\n1. linija = \"[vrsta reprezentacije jezika],[stringovi],...\"\n2. linija = \"[pocetno stanje];[finalna stanja],...\" u slucaju automata ili string u slucaju regexa");
        
        string[] firstLine = lines[counter++].Split(',');
        if(firstLine.Length == 1)
            irregularLinesCounter++;
        HashSet<string> stringovi = new HashSet<string>();
        for(int i = 1; i < firstLine.Length; i++) {
            stringovi.Add(firstLine[i]);
        }
        
        if(firstLine[0] == "DFA") {
            if(lines.Length < 3) {
                irregularLinesCounter++;
            }
            else {
                Dfa dfa = new();
                if(lines[counter].Contains(';')) {
                    string[] startAndFinalStates = lines[counter++].Split(';');
                    if (startAndFinalStates[1] == "")
                        irregularLinesCounter++;
                    string[] finalStates = startAndFinalStates[1].Split(',');
                    
                    string startState = startAndFinalStates[0];
                    dfa.SetStartState(startState);
                    
                    
                    
                    for(int i = 0; i < finalStates.Length; i++) {
                        if(finalStates[i] != "")
                            dfa.AddFinalState(finalStates[i]);
                    }
                }
                else {
                    dfa.SetStartState(lines[counter++]);
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
                                            dfa.AddTransition(source, symbol, destination);
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
                if(irregularLinesCounter == 0) {
                    foreach(string str in stringovi) {
                        Console.WriteLine("String " + str + (dfa.Accepts(str) ? "" : " ne") + " pripada reprezentovanom jeziku.");
                    }
                }
            } 
        }
        else if(firstLine[0] == "ENFA") {
            if(lines.Length < 3) {
                irregularLinesCounter++;
            }
            else {
                ENfa enfa = new();
                if(lines[counter].Contains(';')) {
                    string[] startAndFinalStates = lines[counter++].Split(';');
                    if (startAndFinalStates[1] == "")
                        irregularLinesCounter++;
                    string[] finalStates = startAndFinalStates[1].Split(',');
                    
                    string startState = startAndFinalStates[0];
                    enfa.SetStartState(startState);
                    
                    
                    
                    for(int i = 0; i < finalStates.Length; i++) {
                        if(finalStates[i] != "")
                            enfa.AddFinalState(finalStates[i]);
                    }
                }
                else {
                    enfa.SetStartState(lines[counter++]);
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
                                        string[] destinations = destination.Split(',');
                                        HashSet<string> goingTo = new HashSet<string>();
                                        foreach(string str in destinations) {
                                            goingTo.Add(str);
                                        }
                                        try {
                                            enfa.AddTransition(source, symbol, new HashSet<string>(goingTo));
                                        }
                                        catch(Exception e) {
                                            e.ToString();
                                            irregularLinesCounter++;
                                        }
                                        goingTo.Clear();
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
                if(irregularLinesCounter == 0) {
                    foreach(string str in stringovi) {
                        Console.WriteLine("String " + str + (enfa.Accepts(str) ? "" : " ne") + " pripada reprezentovanom jeziku.");
                    }
                }
            }
        }
        else if(firstLine[0] == "REGEX") {
            if(lines.Length == 2) {
                try {
                    ENfa regex = Regex.Evaluate(lines[1]); // ukoliko je regularan izraz neispravan (odnosno nije se mogao kontstruisati ENFA na osnovu njega)
                    foreach(string str in stringovi) {
                        Console.WriteLine("String \"" + str + (regex.Accepts(str) ? "\"" : "\" ne") + " pripada reprezentovanom jeziku.");
                    }
                }
                catch (Exception e) { // u blok se ulazi ukoliko regex nije leksicki ispravan
                    e.ToString();
                    irregularLinesCounter++;
                }
                
            }
            else if(lines.Length == 1) {
                irregularLinesCounter++;
            }
            else {
                for(int i = 2; i < lines.Length; i++) {
                    irregularLinesCounter++;
                }
            }
        }
        else {
            irregularLinesCounter++;
        }
        
        Console.WriteLine("Broj relevantnih linija specifikacije koje sadrže nepravilnosti: " + irregularLinesCounter);
    }
}
