using The_ATM_Machine.Queries;

namespace The_ATM_Machine {
    /// <summary>
    /// Information about the logged in user
    /// </summary>
    public class LoggedInUser {
        private static int accountId; //id of the account Table
        private static string name;
        private static bool isUserLoggedIn = false;

        public static int AccountId { get => accountId; set => accountId = value; }
        public static string Name { get => name; set => name = value; }
        public static int Balance { get => new QueryAccess().accountTable.GetAccountInformation(accountId, (a) => a.Money); }
        public static int Pincode { get => new QueryAccess().accountTable.GetAccountInformation(accountId, (a) => a.Pincode);}

        private LoggedInUser() {
        }

        public static void LogInUser(string name, int accountId) {
            if (!isUserLoggedIn) {
                LoggedInUser.name = name;
                LoggedInUser.accountId = accountId;
                LoggedInUser.isUserLoggedIn = true;
            }
        }
    }
}
