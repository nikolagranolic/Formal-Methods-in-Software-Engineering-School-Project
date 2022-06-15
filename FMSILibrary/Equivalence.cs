namespace FMSILibrary {
    public class Equivalence {
        // O(n^2)
        public static bool AreEquivalent(Dfa m1, Dfa m2) {
            Dfa temp = Dfa.Union(Dfa.Intersection(m1, Dfa.Complement(m2)), Dfa.Intersection(m2, Dfa.Complement(m1)));
            //temp.Minimize();
            if(temp.FinalStates == 0)
                return true;
            else
                return false;
        }   
        // O(n^4)
        public static bool AreEquivalent(ENfa m1, ENfa m2) {
            return Equivalence.AreEquivalent(m1.ConvertToDfa(), m2.ConvertToDfa());
        }
        // O(n^2)
        public static bool AreEquivalent(string regex1, string regex2) {
            return Equivalence.AreEquivalent(Regex.Evaluate(regex1), Regex.Evaluate(regex2));
        }
        // O(n^4)
        public static bool AreEquivalent(Dfa m1, ENfa m2) {
            return Equivalence.AreEquivalent(m1, m2.ConvertToDfa());
        }
        // O(n^4)
        public static bool AreEquivalent(ENfa m1, Dfa m2) {
            return Equivalence.AreEquivalent(m1.ConvertToDfa(), m2);
        }
        // O(n^4)
        public static bool AreEquivalent(Dfa m1, string regex) {
            return Equivalence.AreEquivalent(m1.ConvertToENfa(), Regex.Evaluate(regex));
        }
        // O(n^4)
        public static bool AreEquivalent(string regex, Dfa m1) {
            return Equivalence.AreEquivalent(Regex.Evaluate(regex), m1.ConvertToENfa());
        }
        // O(n^2)
        public static bool AreEquivalent(ENfa m1, string regex) {
            return Equivalence.AreEquivalent(m1, Regex.Evaluate(regex));
        }
        // O(n^2)
        public static bool AreEquivalent(string regex, ENfa m1) {
            return Equivalence.AreEquivalent(Regex.Evaluate(regex), m1);
        }
    }
}