using System;
using System.Linq;

namespace The_ATM_Machine.Queries {
    /// <summary>
    /// Queries related to the user table
    /// </summary>
    public class UserQueries {
        TheDatabase database = new TheDatabase();

        public void AddUser(User user) {
            this.database.database.Users.Add(user);
            this.database.database.SaveChanges();
        }
        public Tuple<string,int,int> GetUserInformation(int bankNumber) {
            var res = database.database.Users.Join(database.database.Accounts, (u) => u.AccountId, (a) => a.AccountId, (ures, aRes) =>
            new Tuple<string,int,int>(ures.Name, aRes.AccountId, aRes.BankNumber))
            .Where((tables) => tables.Item3 == bankNumber).FirstOrDefault();
            return res;
        }
    }
}
