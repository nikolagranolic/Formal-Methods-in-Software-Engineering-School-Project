using FMSILibrary;
namespace UnitTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void ParanBrJedinicaDfaTest()
    {   
        Dfa mod2true = new();
        mod2true.SetStartState("q0");
        mod2true.AddTransition("q0", '1', "q1");
        mod2true.AddTransition("q0", '0', "q0");
        mod2true.AddTransition("q1", '1', "q0");
        mod2true.AddTransition("q1", '0', "q1");
        mod2true.AddFinalState("q0");
        Assert.IsTrue(mod2true.Accepts("00101011"));
        Assert.IsFalse(mod2true.Accepts("10000"));
    }

    [TestMethod]
    public void BrojJedinicaDjeljivSa3EnfaTest()
    {
        ENfa brojJedinicaDjeljivSa3 = new();
        brojJedinicaDjeljivSa3.AddTransition("p0", '1', new HashSet<string>{"p1"});
        brojJedinicaDjeljivSa3.AddTransition("p0", '0', new HashSet<string>{"p0"});
        brojJedinicaDjeljivSa3.AddTransition("p1", '1', new HashSet<string>{"p2"});
        brojJedinicaDjeljivSa3.AddTransition("p1", '0', new HashSet<string>{"p1"});
        brojJedinicaDjeljivSa3.AddTransition("p2", '1', new HashSet<string>{"p0"});
        brojJedinicaDjeljivSa3.AddTransition("p2", '0', new HashSet<string>{"p2"});
        brojJedinicaDjeljivSa3.SetStartState("p0");
        brojJedinicaDjeljivSa3.AddFinalState("p0");
        Assert.IsTrue(brojJedinicaDjeljivSa3.Accepts("00000000"));
        Assert.IsTrue(brojJedinicaDjeljivSa3.Accepts("10010010111"));
        Assert.IsFalse(brojJedinicaDjeljivSa3.Accepts("001"));
    }

    [TestMethod]
    public void EnfaUDfaTest()
    {
        ENfa brojJedinicaDjeljivSa3 = new();
        brojJedinicaDjeljivSa3.SetStartState("p0");
        brojJedinicaDjeljivSa3.AddTransition("p0", '1', new HashSet<string>{"p1"});
        brojJedinicaDjeljivSa3.AddTransition("p0", '0', new HashSet<string>{"p0"});
        brojJedinicaDjeljivSa3.AddTransition("p1", '1', new HashSet<string>{"p2"});
        brojJedinicaDjeljivSa3.AddTransition("p1", '0', new HashSet<string>{"p1"});
        brojJedinicaDjeljivSa3.AddTransition("p2", '1', new HashSet<string>{"p0"});
        brojJedinicaDjeljivSa3.AddTransition("p2", '0', new HashSet<string>{"p2"});
        brojJedinicaDjeljivSa3.AddFinalState("p0");

        Dfa dfa = brojJedinicaDjeljivSa3.ConvertToDfa();
        Assert.IsTrue(dfa.Accepts("0000000"));
        Assert.IsTrue(dfa.Accepts("1000100000000001"));
        Assert.IsFalse(dfa.Accepts("11000011"));
    }

    [TestMethod]
    public void DfaUENfaTest()
    {
        Dfa mod2true = new(); // paran broj jedinica
        mod2true.SetStartState("q0");
        mod2true.AddTransition("q0", '1', "q1");
        mod2true.AddTransition("q0", '0', "q0");
        mod2true.AddTransition("q1", '1', "q0");
        mod2true.AddTransition("q1", '0', "q1");
        mod2true.AddFinalState("q0");

        ENfa enfa = mod2true.ConvertToENfa();
        Assert.IsTrue(enfa.Accepts("0011000"));
        Assert.IsFalse(enfa.Accepts("1"));
    }

    [TestMethod]
    public void DfaOperacijeTest()
    {
        Dfa mod2true = new(); // paran broj jedinica
        mod2true.SetStartState("q0");
        mod2true.AddTransition("q0", '1', "q1");
        mod2true.AddTransition("q0", '0', "q0");
        mod2true.AddTransition("q1", '1', "q0");
        mod2true.AddTransition("q1", '0', "q1");
        mod2true.AddFinalState("q0");
        Dfa mod3true = new(); // broj jedinica djeljiv sa tri
        mod3true.SetStartState("p0");
        mod3true.AddTransition("p0", '1', "p1");
        mod3true.AddTransition("p0", '0', "p0");
        mod3true.AddTransition("p1", '1', "p2");
        mod3true.AddTransition("p1", '0', "p1");
        mod3true.AddTransition("p2", '1', "p0");
        mod3true.AddTransition("p2", '0', "p2");
        mod3true.AddFinalState("p0");

        Dfa union = Dfa.Union(mod2true, mod3true); // broj jedinica djeljiv ili sa 2 ili sa 3
        Assert.IsTrue(union.Accepts("0110000000"));
        Assert.IsTrue(union.Accepts("0111000000"));
        Assert.IsTrue(union.Accepts("01100001111"));
        Assert.IsFalse(union.Accepts("11111"));

        Dfa complement = Dfa.Complement(mod2true); // neparan broj jedinica
        Assert.IsTrue(complement.Accepts("10000011000011"));
        Assert.IsFalse(complement.Accepts("11"));

        Dfa difference = Dfa.Difference(mod2true, mod3true); // broj jedinica djeljiv sa 2 ali ne i sa 3
        Assert.IsTrue(difference.Accepts("11000"));
        Assert.IsFalse(difference.Accepts("111111"));

        Dfa intersection = Dfa.Intersection(mod2true, mod3true); // broj jedinica djeljiv i sa 2 i sa 3 (djeljiv sa 6)
        Assert.IsTrue(intersection.Accepts("000111100101"));
        Assert.IsFalse(intersection.Accepts("10111"));
        Assert.IsFalse(intersection.Accepts("00111"));
    }

    [TestMethod]
    public void ENfaOperacijeTest()
    {
        ENfa mod3true = new(); // br. jedinica djeljiv sa 3
        mod3true.AddTransition("p0", '1', new HashSet<string>{"p1"});
        mod3true.AddTransition("p0", '0', new HashSet<string>{"p0"});
        mod3true.AddTransition("p1", '1', new HashSet<string>{"p2"});
        mod3true.AddTransition("p1", '0', new HashSet<string>{"p1"});
        mod3true.AddTransition("p2", '1', new HashSet<string>{"p0"});
        mod3true.AddTransition("p2", '0', new HashSet<string>{"p2"});
        mod3true.SetStartState("p0");
        mod3true.AddFinalState("p0");

        ENfa mod2true = new(); // paran br. jedinica
        mod2true.AddTransition("q0", '1', new HashSet<string>{"q1"});
        mod2true.AddTransition("q0", '0', new HashSet<string>{"q0"});
        mod2true.AddTransition("q1", '1', new HashSet<string>{"q0"});
        mod2true.AddTransition("q1", '0', new HashSet<string>{"q1"});
        mod2true.SetStartState("q0");
        mod2true.AddFinalState("q0");

        ENfa union = ENfa.Union(mod3true, mod2true); // broj jedinica djeljiv ili sa 2 ili sa 3
        Assert.IsTrue(union.Accepts("0110000000"));
        Assert.IsTrue(union.Accepts("0111000000"));
        Assert.IsTrue(union.Accepts("01100001111"));
        Assert.IsFalse(union.Accepts("11111"));

        ENfa concatenation = ENfa.Concatenation(mod2true, mod3true);
        Assert.IsTrue(concatenation.Accepts(""));
        Assert.IsTrue(concatenation.Accepts("1100000111"));
        Assert.IsFalse(concatenation.Accepts("0000000001"));

        ENfa intersection = ENfa.Intersection(mod2true, mod3true);
        Assert.IsTrue(intersection.Accepts("000111100101"));
        Assert.IsFalse(intersection.Accepts("10111"));
        Assert.IsFalse(intersection.Accepts("00111"));

        ENfa difference = ENfa.Difference(mod2true, mod3true);
        Assert.IsTrue(difference.Accepts("11000"));
        Assert.IsFalse(difference.Accepts("111111"));

        ENfa complement = ENfa.Complement(mod3true);
        Assert.IsTrue(complement.Accepts("011000"));
        Assert.IsFalse(complement.Accepts("0111000"));

        ENfa length2 = new(); // prihvata samo rijec koja prima dvije jedinice i u alfabetu ima samo jedinice
        length2.SetStartState("m0");
        length2.AddTransition("m0", '1', new HashSet<string>{"m1"});
        length2.AddTransition("m1", '1', new HashSet<string>{"m2"});
        length2.AddFinalState("m2");

        ENfa mod2trueStar = ENfa.Star(length2);
        Assert.IsTrue(mod2trueStar.Accepts("1111"));
        Assert.IsFalse(mod2trueStar.Accepts("11111"));
    }

    [TestMethod]
    public void ShortestWordTest()
    {
        Dfa mod3true = new(); // broj jedinica djeljiv sa tri
        mod3true.SetStartState("p0");
        mod3true.AddTransition("p0", '1', "p1");
        mod3true.AddTransition("p0", '0', "p0");
        mod3true.AddTransition("p1", '1', "p2");
        mod3true.AddTransition("p1", '0', "p1");
        mod3true.AddTransition("p2", '1', "p0");
        mod3true.AddTransition("p2", '0', "p2");
        mod3true.AddFinalState("p0");
        Assert.AreEqual(mod3true.ShortestWordLength(), 0);

        ENfa length2 = new(); // prihvata samo rijec koja prima dvije jedinice i u alfabetu ima samo jedinice
        length2.SetStartState("m0");
        length2.AddTransition("m0", '1', new HashSet<string>{"m1"});
        length2.AddTransition("m1", '1', new HashSet<string>{"m2"});
        length2.AddFinalState("m2");
        Assert.AreEqual(length2.ShortestWordLength(), 2);

        ENfa regex = Regex.Evaluate("111+00*+1");
        Assert.AreEqual(regex.ShortestWordLength(), 1);
    }

    [TestMethod]
    public void FiniteLanguageTest()
    {
        ENfa length2 = new(); // prihvata samo rijec koja prima dvije jedinice i u alfabetu ima samo jedinice
        length2.SetStartState("m0");
        length2.AddTransition("m0", '1', new HashSet<string>{"m1"});
        length2.AddTransition("m1", '1', new HashSet<string>{"m2"});
        length2.AddFinalState("m2");
        Assert.IsTrue(length2.IsLanguageFinite());

        ENfa regex = Regex.Evaluate("111+00*+1");
        Assert.IsFalse(regex.IsLanguageFinite());
    }

    [TestMethod]
    public void LongestWordTest()
    {
        // longest word length method returns -1 if the language is infinite
        ENfa length2 = new(); // prihvata samo rijec koja prima dvije jedinice i u alfabetu ima samo jedinice
        length2.SetStartState("m0");
        length2.AddTransition("m0", '1', new HashSet<string>{"m1"});
        length2.AddTransition("m1", '1', new HashSet<string>{"m2"});
        length2.AddFinalState("m2");
        Assert.AreEqual(length2.LongestWordLength(), 2);

        ENfa regex = Regex.Evaluate("111+00*+1");
        Assert.AreEqual(regex.LongestWordLength(), -1);

        ENfa regex2 = Regex.Evaluate("111+00+1");
        Assert.AreEqual(regex2.LongestWordLength(), 3);
    }

    [TestMethod]
    public void RegexToENfa()
    {
        ENfa regex = Regex.Evaluate("((1(0+1)01)*+(10+0)*)"); // paran broj jedinica ili 010
        Assert.IsTrue(regex.Accepts(""));
        Assert.IsFalse(regex.Accepts("111"));
        Assert.IsTrue(regex.Accepts("10011101"));
    }

    [TestMethod]
    public void MinimizeDfaTest() // nacrtati ovakav automat neminimizovan i minimizovan
    {
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

        Assert.IsTrue(testDfaBig.Accepts("baaba"));
        Assert.IsTrue(testDfaBig.Accepts("baabab"));
        Assert.IsFalse(testDfaBig.Accepts("baaaa"));
        Assert.IsFalse(testDfaBig.Accepts("babaabb"));

        testDfaBig.Minimize();

        Assert.IsTrue(testDfaBig.Accepts("baaba"));
        Assert.IsTrue(testDfaBig.Accepts("baabab"));
        Assert.IsFalse(testDfaBig.Accepts("baaaa"));
        Assert.IsFalse(testDfaBig.Accepts("babaabb"));
    }

    [TestMethod]
    public void EquivalenceTest()
    {
        Dfa finiteLang = new();
        finiteLang.SetStartState("p0");
        finiteLang.AddTransition("p0", '1', "p1");
        finiteLang.AddTransition("p1", '1', "p2");
        finiteLang.AddTransition("p2", '1', "p3");
        finiteLang.AddTransition("p3", '1', "p3");
        finiteLang.AddFinalState("p2");
        Dfa finiteLang2 = Regex.Evaluate("11").ConvertToDfa();
        Assert.IsTrue(Equivalence.AreEquivalent(finiteLang, finiteLang2)); // oba imaju jezik koji se sastoji od samo jedne rijeci = "11"

        ENfa test2 = new();
        test2.SetStartState("k0");
        test2.AddTransition("k0", '1', new HashSet<string>{"k1"});
        test2.AddTransition("k1", '1', new HashSet<string>{"k2"});
        test2.AddTransition("k2", '1', new HashSet<string>{"k3"});
        test2.AddFinalState("k2");
        Assert.IsTrue(Equivalence.AreEquivalent("11", finiteLang)); // isto

        Assert.IsTrue(Equivalence.AreEquivalent("11", "$1$1$")); // $ je epsilon
        Assert.IsTrue(Equivalence.AreEquivalent("11", "11+O")); // O je prazan skup
        Assert.IsFalse(Equivalence.AreEquivalent("1101", "0010"));
    }
}