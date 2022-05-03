﻿using FMSILibrary;

Dfa testDfaBig = new();
testDfaBig.SetStartState("q0");

testDfaBig.AddTransition("q0", 'a', "q6");
testDfaBig.AddTransition("q0", 'b', "q1");
testDfaBig.AddTransition("q1", 'a', "q2");
testDfaBig.AddTransition("q1", 'b', "q4");
testDfaBig.AddTransition("q2", 'a', "q5");
testDfaBig.AddTransition("q2", 'b', "q3");
testDfaBig.AddTransition("q3", 'a', "q8");
testDfaBig.AddTransition("q3", 'b', "q3");
testDfaBig.AddTransition("q4", 'a', "q6");
testDfaBig.AddTransition("q4", 'b', "q8");
testDfaBig.AddTransition("q5", 'a', "q2");
testDfaBig.AddTransition("q5", 'b', "q3");
testDfaBig.AddTransition("q6", 'a', "q4");
testDfaBig.AddTransition("q6", 'b', "q7");
testDfaBig.AddTransition("q7", 'a', "q8");
testDfaBig.AddTransition("q7", 'b', "q7");
testDfaBig.AddTransition("q8", 'a', "q7");
testDfaBig.AddTransition("q8", 'b', "q4");
testDfaBig.AddTransition("q9", 'a', "q10");
testDfaBig.AddTransition("q9", 'b', "q8");
testDfaBig.AddTransition("q10", 'a', "q3");
testDfaBig.AddTransition("q10", 'b', "q9");

testDfaBig.AddFinalState("q1");
testDfaBig.AddFinalState("q4");
testDfaBig.AddFinalState("q8");

Console.WriteLine(testDfaBig.Accepts("baaba"));
Console.WriteLine(testDfaBig.Accepts("baabab"));
Console.WriteLine(testDfaBig.Accepts("baaaa"));
Console.WriteLine(testDfaBig.Accepts("babaabb"));

Console.WriteLine();

testDfaBig.Minimize();

Console.WriteLine(testDfaBig.Accepts("baaba"));
Console.WriteLine(testDfaBig.Accepts("baabab"));
Console.WriteLine(testDfaBig.Accepts("baaaa"));
Console.WriteLine(testDfaBig.Accepts("babaabb"));


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

// testENfa.SetStartState();

// testENfa.AddTransition("q0", '$');
// testENfa.AddTransition("q0", 'a');
// testENfa.AddTransition("q1", '$');

// testENfa.AddTransition("q2", 'b');
// testENfa.AddTransition("q4", 'b');
// testENfa.AddTransition("q3", '$');
// testENfa.AddTransition("q6", '$');
// testENfa.AddTransition("q5", '$');

// testENfa.AddFinalState("q1");
// testENfa.AddFinalState("q6");
// // test 123
// Console.WriteLine(testENfa.Accepts("b"));
// Console.WriteLine(testENfa.Accepts("a"));
// Console.WriteLine(testENfa.Accepts("ab"));