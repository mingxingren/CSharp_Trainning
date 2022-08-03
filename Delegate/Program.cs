using System;

namespace Program {
    class App {
        static void Main(string[] args) {
            Func<int, bool> fnPrint = (int param) => {
                Console.WriteLine("this is Func");
                return param % 2 == 0;
            };

            fnPrint(0);

            Action<int> fnAction = (int param) => {
                Console.WriteLine("this is Action");
            };
            fnAction(0);

            Predicate<int> fnPredicate = (int param) => {
                Console.WriteLine("this is Predicate");
                return param % 2 == 0;
            };
            fnPredicate(0); 
        }
    }
}