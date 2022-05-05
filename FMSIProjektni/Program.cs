using FMSILibrary;

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

// ENfa eNfatest = testDfaBig.convertToENfa();

// Console.WriteLine(eNfatest.Accepts("baaba"));
// Console.WriteLine(eNfatest.Accepts("baabab"));
// Console.WriteLine(eNfatest.Accepts("baaaa"));
// Console.WriteLine(eNfatest.Accepts("babaabb"));

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


ENfa testENfa = new();

testENfa.SetStartState("q0");

testENfa.AddTransition("q0", '$', new HashSet<string>{"q3", "q4", "q5"});
testENfa.AddTransition("q0", 'a', new HashSet<string>{"q1", "q2"});
testENfa.AddTransition("q1", '$', new HashSet<string>{"q6"});
testENfa.AddTransition("q2", 'b', new HashSet<string>{"q6"});
testENfa.AddTransition("q4", 'b', new HashSet<string>{"q7"});
testENfa.AddTransition("q3", '$', new HashSet<string>{"q6"});
testENfa.AddTransition("q6", '$', new HashSet<string>{"q5"});
testENfa.AddTransition("q5", '$', new HashSet<string>{"q0"});

testENfa.AddFinalState("q1");
testENfa.AddFinalState("q6");

Console.WriteLine(testENfa.Accepts("b"));
Console.WriteLine(testENfa.Accepts("a"));
Console.WriteLine(testENfa.Accepts("ab"));
Console.WriteLine(testENfa.Accepts("aaa"));
Console.WriteLine(testENfa.Accepts("abab"));
Console.WriteLine(testENfa.Accepts("aabb"));
Dfa convertedDfa = testENfa.ConvertToDfa();

Console.WriteLine();
Console.WriteLine(convertedDfa.Accepts("b"));
Console.WriteLine(convertedDfa.Accepts("a"));
Console.WriteLine(convertedDfa.Accepts("ab"));
Console.WriteLine(convertedDfa.Accepts("aaa"));
Console.WriteLine(convertedDfa.Accepts("abab"));
Console.WriteLine(convertedDfa.Accepts("aabb"));