using System;

namespace The_ATM_Machine {
    /// <summary>
    /// Verifies if the number that has been typed by the user, is correct
    /// </summary>
    public class InputHandling { 
        public static int CheckNumber() {
            var givenNumber = 0;
            while (true) {
                var answer = Int32.TryParse(Console.ReadLine(), out givenNumber);
                if (answer && givenNumber > 0){
                    return givenNumber;
                }
                Console.WriteLine("Error: enter a valid number above the 0");
            }
        }
    }
}
