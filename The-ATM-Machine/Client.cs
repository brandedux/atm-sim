namespace The_ATM_Machine {
    /// <summary>
    /// Start of the program
    /// </summary>
    public class Client {
        static void Main(string[] args) {
            var atm = new Machine();
            atm.StartUpMenuOptions(); //when the user is not logged in
            atm.MainMenuOptions(); //when the user is logged in
        }
    }
}
