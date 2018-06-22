using System;
using System.Linq;

namespace The_ATM_Machine.Queries {
    /// <summary>
    /// Handles queries related to the account table
    /// </summary>
    public class AccountQueries {
        TheDatabase database = new TheDatabase();

        public bool CheckIfBankNumberExists(int bankNumber) {
            var res = database.database.Accounts.Where(a => a.BankNumber == bankNumber).FirstOrDefault();
            if(res == null) {
                return false;
            }
            return true;
        }
        public void AddAccount(Account account) {
            database.database.Accounts.Add(account);
            database.database.SaveChanges();
        }
        public int GetAccountId(int bankNumber) {
            var res = database.database.Accounts.Where(a => a.BankNumber == bankNumber).FirstOrDefault();
            return res.AccountId;
        }
        public bool IsPincodeCorrect(int bankNumber, int pincode) {
            var res = database.database.Accounts.Where(a => a.BankNumber == bankNumber).FirstOrDefault();
            if(res.Pincode == pincode) {
                return true;
            }
            return false;
        }
        public void ChangeBalance(Func<Account,bool> wherePredicate, int ammount, Action<Account> predicate) {
            var account = database.database.Accounts.Where(wherePredicate).FirstOrDefault();
            predicate(account);
            database.database.SaveChanges();
        }
        public void ChangePincode(int accountId, int pincode) {
            database.database.Accounts.Where(a => a.AccountId == accountId).FirstOrDefault().Pincode = pincode;
            database.database.SaveChanges();
        }
        public int GetAccountInformation(int accountId, Func<Account, dynamic> predicate) {
            var res = database.database.Accounts.Where((a) => a.AccountId == accountId).FirstOrDefault();
            return predicate(res);
        }
    }
}
