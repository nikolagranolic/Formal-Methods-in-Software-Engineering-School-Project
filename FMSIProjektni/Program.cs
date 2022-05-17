using FMSILibrary;

//Dfa test = new();
// test.SetStartState("m0");
// test.AddTransition("m0", '1', "m1");
// test.AddTransition("m1", '1', "m2");
// test.AddTransition("m2", '1', "m3");
// test.AddTransition("m3", '1', "m4");
// test.AddTransition("m4", '1', "m5");
// test.AddTransition("m5", '1', "m5");
// test.AddFinalState("m0");
// test.AddFinalState("m1");
// test.AddFinalState("m2");
// test.AddFinalState("m3");
// test.AddFinalState("m4");
// Console.WriteLine(test.Accepts("1"));
// Console.WriteLine(test.Accepts("11"));
// Console.WriteLine(test.Accepts("111"));
// Console.WriteLine(test.Accepts("1111"));
// Console.WriteLine(test.Accepts("11111"));
// Console.WriteLine(test.LongestWordLength());
ENfa test2 = new();
test2.SetStartState("k0");
test2.AddTransition("k0", '1', new HashSet<string>{"k1"});
test2.AddTransition("k1", '1', new HashSet<string>{"k2"});
test2.AddTransition("k2", '1', new HashSet<string>{"k3"});
test2.AddFinalState("k1");
Dfa test4 = test2.ConvertToDfa();
Console.WriteLine(test4.Accepts("1"));
Console.WriteLine(test4.ShortestWordLength());
Console.WriteLine(test4.LongestWordLength());

// Dfa test3 = Regex.Evaluate("01+11(0+1)+0(11+01+00+10)*").ConvertToDfa(); 
// Console.WriteLine(test3.LongestWordLength());
// Console.WriteLine(test3.ShortestWordLength());


// Dfa mod2true = new();
// mod2true.SetStartState("q0");
// mod2true.AddTransition("q0", '1', "q1");
// mod2true.AddTransition("q0", '0', "q0");
// mod2true.AddTransition("q1", '1', "q0");
// mod2true.AddTransition("q1", '0', "q1");
// mod2true.AddFinalState("q0");
// Console.WriteLine(mod2true.IsLanguageFinite());


// Dfa finiteLang = new();
// finiteLang.SetStartState("p0");
// finiteLang.AddTransition("p0", '1', "p1");
// finiteLang.AddTransition("p1", '1', "p2");
// finiteLang.AddTransition("p2", '1', "p3");
// finiteLang.AddTransition("p3", '1', "p3");
// finiteLang.AddFinalState("p2");
// Console.WriteLine(finiteLang.Accepts("11"));

// Dfa finiteLang2 = Regex.Evaluate("11").ConvertToDfa();
// Console.WriteLine(Equivalence.AreEquivalent(finiteLang, finiteLang2));

// ENfa test2 = new();
// test2.SetStartState("k0");
// test2.AddTransition("k0", '1', new HashSet<string>{"k1"});
// test2.AddTransition("k1", '1', new HashSet<string>{"k2"});
// test2.AddTransition("k2", '1', new HashSet<string>{"k3"});
// test2.AddFinalState("k2");
// Console.WriteLine("\n" + Equivalence.AreEquivalent("11", finiteLang));




//ENfa enfa2 = Regex.Evaluate()
// ENfa regex = Regex.Evaluate("a-b*+b+a"); 
// Dfa dfaRegex = regex.ConvertToDfa();
// Console.WriteLine(regex.Accepts("aba"));
// Console.WriteLine(dfaRegex.Accepts("aba"));

// ENfa test1 = new();
// test1.SetStartState("q0");
// test1.AddTransition("q0", '1', new HashSet<string>{"q1"});
// test1.AddTransition("q0", '0', new HashSet<string>{"q0"});
// test1.AddTransition("q1", '1', new HashSet<string>{"q0"});
// test1.AddTransition("q1", '0', new HashSet<string>{"q1"});
// test1.AddFinalState("q1");

// Dfa test2 = new();
// test2.SetStartState("p0");
// test2.AddTransition("p0", '1', "p0");
// test2.AddTransition("p0", '0', "p1");
// test2.AddTransition("p1", '1', "p1");
// test2.AddTransition("p1", '0', "p0");
// test2.AddFinalState("p0");

// Console.WriteLine(test1.Accepts("10011"));

// Console.WriteLine(test2.Accepts("10011"));

// Dfa testDfa1 = test1.ConvertToDfa();

// Dfa intersection = Dfa.Intersection(testDfa1, test2);
// Dfa union = Dfa.Union(testDfa1, test2);
// Dfa difference = Dfa.Difference(testDfa1, test2);

// Console.WriteLine("PRESJEK: " + intersection.Accepts("10011"));
// Console.WriteLine("UNIJA: " + union.Accepts("10011"));
// Console.WriteLine("RAZLIKA: " + difference.Accepts("10011"));

// Console.WriteLine(test1.ConvertToDfa().ShortestWordLength());

// Dfa rijeciDuzine2 = new();
// rijeciDuzine2.SetStartState("m0");
// rijeciDuzine2.AddTransition("m0", 'a', "m1");
// rijeciDuzine2.AddTransition("m0", 'b', "m1");
// rijeciDuzine2.AddTransition("m1", 'a', "m2");
// rijeciDuzine2.AddTransition("m1", 'b', "m2");
// rijeciDuzine2.AddTransition("m2", 'a', "m3");
// rijeciDuzine2.AddTransition("m2", 'b', "m3");
// rijeciDuzine2.AddTransition("m3", 'a', "m3");
// rijeciDuzine2.AddTransition("m3", 'b', "m3");
// rijeciDuzine2.AddFinalState("m2");
// Console.WriteLine(rijeciDuzine2.Accepts("ababababbbaabbabbabab"));
// Console.WriteLine(rijeciDuzine2.ShortestWordLength());




// Dfa mod2true = new();
// mod2true.SetStartState("q0");
// mod2true.AddTransition("q0", '1', "q1");
// mod2true.AddTransition("q0", '0', "q0");
// mod2true.AddTransition("q1", '1', "q0");
// mod2true.AddTransition("q1", '0', "q1");
// mod2true.AddFinalState("q0");
// Dfa mod3true = new();
// mod3true.SetStartState("q0");
// mod3true.AddTransition("q0", '1', "q1");
// mod3true.AddTransition("q0", '0', "q0");
// mod3true.AddTransition("q1", '1', "q2");
// mod3true.AddTransition("q1", '0', "q1");
// mod3true.AddTransition("q2", '1', "q0");
// mod3true.AddTransition("q2", '0', "q2");
// mod3true.AddFinalState("q0");
// Dfa mod2notmod3 = Dfa.Difference(mod2true, mod3true);
// Console.WriteLine(mod2notmod3.Accepts("00100110000011000110010110000"));



// ENfa paranBrJedinica = new();
// paranBrJedinica.AddTransition("q0", '1', new HashSet<string>{"q1"});
// paranBrJedinica.AddTransition("q0", '0', new HashSet<string>{"q0"});
// paranBrJedinica.AddTransition("q1", '1', new HashSet<string>{"q0"});
// paranBrJedinica.AddTransition("q1", '0', new HashSet<string>{"q1"});
// paranBrJedinica.SetStartState("q0");
// paranBrJedinica.AddFinalState("q0");
// ENfa nepBr1 = ENfa.Complement(paranBrJedinica);
//  Console.WriteLine(nepBr1.Accepts("1000000"));
// ENfa neparanBrJedinica = ENfa.Complement(paranBrJedinica);
// Console.WriteLine(neparanBrJedinica.Accepts("111"));
// ENfa brojJedinicaDjeljivSa3 = new();
// brojJedinicaDjeljivSa3.AddTransition("p0", '1', new HashSet<string>{"p1"});
// brojJedinicaDjeljivSa3.AddTransition("p0", '0', new HashSet<string>{"p0"});
// brojJedinicaDjeljivSa3.AddTransition("p1", '1', new HashSet<string>{"p2"});
// brojJedinicaDjeljivSa3.AddTransition("p1", '0', new HashSet<string>{"p1"});
// brojJedinicaDjeljivSa3.AddTransition("p2", '1', new HashSet<string>{"p0"});
// brojJedinicaDjeljivSa3.AddTransition("p2", '0', new HashSet<string>{"p2"});
// brojJedinicaDjeljivSa3.SetStartState("p0");
// brojJedinicaDjeljivSa3.AddFinalState("p0");

// ENfa mod2notmod3 = ENfa.Difference(paranBrJedinica, brojJedinicaDjeljivSa3);
// Console.WriteLine(mod2notmod3.Accepts("1111111111"));


//Console.WriteLine(brojJedinicaDjeljivSa3.Accepts("000000000100010000000"));
//ENfa brJedinicaDjeljivSa2ili3 = ENfa.Union(paranBrJedinica, brojJedinicaDjeljivSa3);
//ENfa brJedinicaDjeljivSa6 = ENfa.Concatenation(paranBrJedinica, brojJedinicaDjeljivSa3);
//ENfa paranBr1Star = ENfa.Star(paranBrJedinica);
//Console.WriteLine(paranBr1Star.Accepts("1"));
// ENfa test = new();

// test.SetStartState("q0");
// test.AddTransition("q0", 'a', new HashSet<string>{"q1"});
// test.AddTransition("q1", 'a', new HashSet<string>{"q2"});
// test.AddTransition("q2", 'b', new HashSet<string>{"q3"});
// test.AddTransition("q3", 'b', new HashSet<string>{"q0"});

// Console.WriteLine(test.Accepts("abababbaaabab"));
// Dfa testDfa = test.ConvertToDfa();
// Console.WriteLine(testDfa.Accepts("abababbaaabab"));


// ENfa eNfa = new();
// eNfa.SetStartState("q0");
// eNfa.AddTransition("q0", 'a', new HashSet<string>{"q1"});
// eNfa.AddTransition("q0", 'b', new HashSet<string>{"q0"});
// eNfa.AddTransition("q1", 'b', new HashSet<string>{"q1"});
// eNfa.AddTransition("q1", '$', new HashSet<string>{"q2", "q4"});
// eNfa.AddTransition("q2", 'a', new HashSet<string>{"q2"});
// eNfa.AddTransition("q2", 'b', new HashSet<string>{"q5"});
// eNfa.AddTransition("q4", 'a', new HashSet<string>{"q5"});
// eNfa.AddTransition("q4", 'b', new HashSet<string>{"q3"});
// eNfa.AddTransition("q3", 'a', new HashSet<string>{"q4"});
// eNfa.AddFinalState("q5");
// Console.WriteLine(eNfa.Accepts("aaabaaba"));
// Console.WriteLine(eNfa.Accepts("bbabbbababaa"));
// Console.WriteLine(eNfa.Accepts("babaabababbaabbaaaabbb"));
// Console.WriteLine();
// Dfa dfa = eNfa.ConvertToDfa();
// Console.WriteLine(dfa.Accepts("aaabaaba"));
// Console.WriteLine(dfa.Accepts("bbabbbababaa"));
// Console.WriteLine(eNfa.Accepts("babaabababbaabbaaaabbb"));

// Dfa testDfaBig = new();
// testDfaBig.SetStartState("q0");

// testDfaBig.AddTransition("q0", 'a', "q6");
// testDfaBig.AddTransition("q0", 'b', "q1");
// testDfaBig.AddTransition("q1", 'a', "q2");
// testDfaBig.AddTransition("q1", 'b', "q4");
// testDfaBig.AddTransition("q2", 'a', "q5");
// testDfaBig.AddTransition("q2", 'b', "q3");
// testDfaBig.AddTransition("q3", 'a', "q8");
// testDfaBig.AddTransition("q3", 'b', "q3");
// testDfaBig.AddTransition("q4", 'a', "q6");
// testDfaBig.AddTransition("q4", 'b', "q8");
// testDfaBig.AddTransition("q5", 'a', "q2");
// testDfaBig.AddTransition("q5", 'b', "q3");
// testDfaBig.AddTransition("q6", 'a', "q4");
// testDfaBig.AddTransition("q6", 'b', "q7");
// testDfaBig.AddTransition("q7", 'a', "q8");
// testDfaBig.AddTransition("q7", 'b', "q7");
// testDfaBig.AddTransition("q8", 'a', "q7");
// testDfaBig.AddTransition("q8", 'b', "q4");
// testDfaBig.AddTransition("q9", 'a', "q10");
// testDfaBig.AddTransition("q9", 'b', "q8");
// testDfaBig.AddTransition("q10", 'a', "q3");
// testDfaBig.AddTransition("q10", 'b', "q9");

// testDfaBig.AddFinalState("q1");
// testDfaBig.AddFinalState("q4");
// testDfaBig.AddFinalState("q8");

// Console.WriteLine(testDfaBig.Accepts("baaba"));
// Console.WriteLine(testDfaBig.Accepts("baabab"));
// Console.WriteLine(testDfaBig.Accepts("baaaa"));
// Console.WriteLine(testDfaBig.Accepts("babaabb"));

// Console.WriteLine();

// testDfaBig.Minimize();

// Console.WriteLine(testDfaBig.Accepts("baaba"));
// Console.WriteLine(testDfaBig.Accepts("baabab"));
// Console.WriteLine(testDfaBig.Accepts("baaaa"));
// Console.WriteLine(testDfaBig.Accepts("babaabb"));




// Dfa testDfa = new();

// testDfa.SetStartState("q0");
// testDfa.AddTransition("q0", 'a', "q1");
// testDfa.AddTransition("q0", 'b', "q3");
// testDfa.AddTransition("q1", 'a', "q0");
// testDfa.AddTransition("q1", 'b', "q2");
// testDfa.AddTransition("q2", 'a', "q2");
// testDfa.AddTransition("q2", 'b', "q0");
// testDfa.AddTransition("q3", 'a', "q2");
// testDfa.AddTransition("q3", 'b', "q0");
// testDfa.AddFinalState("q2");
// testDfa.AddFinalState("q3");

// Console.WriteLine(testDfa.Accepts("ababb"));
// Console.WriteLine(testDfa.Accepts("ababbb"));
// Console.WriteLine();

// testDfa.Minimize();

// Console.WriteLine(testDfa.Accepts("ababb"));
// Console.WriteLine(testDfa.Accepts("ababbb"));


// ENfa testENfa = new();

// testENfa.SetStartState("q0");

// testENfa.AddTransition("q0", '$', new HashSet<string>{"q3", "q4", "q5"});
// testENfa.AddTransition("q0", 'a', new HashSet<string>{"q1", "q2"});
// testENfa.AddTransition("q1", '$', new HashSet<string>{"q6"});
// testENfa.AddTransition("q2", 'b', new HashSet<string>{"q6"});
// testENfa.AddTransition("q4", 'b', new HashSet<string>{"q7"});
// testENfa.AddTransition("q3", '$', new HashSet<string>{"q6"});
// testENfa.AddTransition("q6", '$', new HashSet<string>{"q5"});
// testENfa.AddTransition("q5", '$', new HashSet<string>{"q0"});

// testENfa.AddFinalState("q1");
// testENfa.AddFinalState("q6");

// Console.WriteLine(testENfa.Accepts("b"));
// Console.WriteLine(testENfa.Accepts("a"));
// Console.WriteLine(testENfa.Accepts("ab"));
// Console.WriteLine(testENfa.Accepts("aaa"));
// Console.WriteLine(testENfa.Accepts("abab"));
// Console.WriteLine(testENfa.Accepts("aabb"));