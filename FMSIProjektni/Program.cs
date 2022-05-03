using FMSILibrary;

Dfa testDfa = new();

testDfa.SetStartState("q0");
testDfa.AddTransition("q0", 'a', "q1");
testDfa.AddTransition("q0", 'b', "q3");
testDfa.AddTransition("q1", 'a', "q0");
testDfa.AddTransition("q1", 'b', "q2");
testDfa.AddTransition("q2", 'a', "q2");
testDfa.AddTransition("q2", 'b', "q0");
testDfa.AddTransition("q3", 'a', "q2");
testDfa.AddTransition("q3", 'b', "q0");
testDfa.AddFinalState("q2");
testDfa.AddFinalState("q3");

Console.WriteLine(testDfa.Accepts("ababb"));
Console.WriteLine(testDfa.Accepts("ababbb"));
Console.WriteLine();

testDfa.Minimize();

Console.WriteLine(testDfa.Accepts("ababb"));
Console.WriteLine(testDfa.Accepts("ababbb"));
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