using FMSILibrary;

ENfa testENfa = new();

testENfa.SetStartState();

testENfa.AddTransition("q0", '$');
testENfa.AddTransition("q0", 'a');
testENfa.AddTransition("q1", '$');

testENfa.AddTransition("q2", 'b');
testENfa.AddTransition("q4", 'b');
testENfa.AddTransition("q3", '$');
testENfa.AddTransition("q6", '$');
testENfa.AddTransition("q5", '$');

testENfa.AddFinalState("q1");
testENfa.AddFinalState("q6");
// test 123
Console.WriteLine(testENfa.Accepts("b"));
Console.WriteLine(testENfa.Accepts("a"));
Console.WriteLine(testENfa.Accepts("ab"));