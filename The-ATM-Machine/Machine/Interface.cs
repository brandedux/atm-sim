using System;
using The_ATM_Machine.Queries;
using System.Threading;

namespace The_ATM_Machine {
    /// <summary>
    /// The ATM
    /// </summary>
    public class Machine {
        QueryAccess queryAccess = new QueryAccess();

        /// <summary>
        /// Menu when the user is not logged in, shows the login, create and exit options
        /// </summary>
        public void StartUpMenuOptions() {     
            while (true) {
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("Welcome to the ATM \n Select one of the following options:");
                Console.WriteLine("Login: 1 \n Create a bank account: 2 \n Exit: 3");
                var answer = Console.ReadLine();
                switch (answer.ToLower()) {
                    case ("1"):
                        this.Login();
                        return;
                    case ("2"):
                        this.CreateBankAccount();
                        break;
                    case ("3"):
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Error: choose a valid menu option");
                        break;
                }
            }
        }
        /// <summary>
        /// Menu when the user is logged in, shows the main options
        /// </summary>
        public void MainMenuOptions() {      
            while (true) {
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("Main Menu \n Welcome {0}", LoggedInUser.Name);
                Console.WriteLine("Draw money: 1 \n Deposit: 2 \n Transfer money: 3 \n Change Pincode: 4 \n  Current balance: 5 \n Exit: 6");
                var answer = Console.ReadLine();
                switch (answer.ToLower()) {
                    case ("1"):
                        this.Withdraw();
                        break;
                    case ("2"):
                        this.Deposit();
                        break;
                    case ("3"):
                        this.Transfer();
                        break;
                    case ("4"):
                        this.ChangePincode();
                        break;
                    case ("5"):
                        this.SeeBalance();
                        break;
                    case ("6"):
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Error: choose a valid menu option");
                        break;
                }
            }
        }
        /// <summary>
        /// This menu shows when the user enters invalid data
        /// </summary>
        /// <param name="backPredicate">the the method to call when the user chooses the back option</param>
        /// <param name="retryPredicate">the the method to call when the user chooses the retry option</param>
        private void BackRetryExit(Action backPredicate, Action retryPredicate) {
            Console.WriteLine("Retry: 1 \n Back: 2 \n Exit: 3");
            while (true) {
                var answer = Console.ReadLine();
                switch (answer.ToLower()) {
                    case ("1"):
                        retryPredicate();
                        return;
                    case ("2"):
                        backPredicate();
                        return;
                    case ("3"):
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Error: select a valid menu option");
                        break;
                }
            }
        }
        /// <summary>
        /// The login menu
        /// </summary>
        private void Login() {
            Console.WriteLine("Login");
            Console.WriteLine("Enter your bank account number");
            var givenBanknumber = InputHandling.CheckNumber();
            if (queryAccess.accountTable.CheckIfBankNumberExists(givenBanknumber)) {
                var givenPincode = this.PincodeConfirmation("Enter your pincode");
                if(queryAccess.accountTable.IsPincodeCorrect(givenBanknumber, givenPincode)) {
                    Console.WriteLine("Successfully logged in");
                    var loggedInUser = queryAccess.user.GetUserInformation(givenBanknumber);
                    LoggedInUser.LogInUser(loggedInUser.Item1, loggedInUser.Item2);
                    return;
                    
                }
                Console.WriteLine("Error: The pincode is not correct");
            }
            else {
                Console.WriteLine("Error: the bank account number has not been found");
              
            }
            this.BackRetryExit(() => this.StartUpMenuOptions(), () => this.Login());
        }
        /// <summary>
        /// Create bank acount menu
        /// </summary>
        private void CreateBankAccount() {
            Console.WriteLine("Create Bank Account");
            Console.WriteLine("Type your name");
            var name = Console.ReadLine();
            Console.WriteLine("Type your surname");
            var surname = Console.ReadLine();
            Console.WriteLine("Type the bank account number that you would like to have");
            var bankNumber = this.CheckIfBankNumberExists(InputHandling.CheckNumber());
            var pincode = PincodeConfirmation("Type the pincode you would like to have");
            queryAccess.accountTable.AddAccount(new Account() { BankNumber = bankNumber, Money = 0, Pincode = pincode });
            queryAccess.user.AddUser(new User() { Name = name, Surname = surname, AccountId = queryAccess.accountTable.GetAccountId(bankNumber) });

            Console.WriteLine("Registered \n you can login now");
        }
        /// <summary>
        /// Used for the create bank account menu
        /// </summary>
        /// <param name="bankNumber">the banknumber that the user entered</param>
        /// <returns></returns>
        private int CheckIfBankNumberExists(int bankNumber) {
            while (true) {
                var doesBankAccountExist = queryAccess.accountTable.CheckIfBankNumberExists(bankNumber);
                    if (doesBankAccountExist) {
                        Console.WriteLine("Bank number already exists, try again:");
                        bankNumber = InputHandling.CheckNumber();
                    }
                    else {
                        return bankNumber;                      
                    }
                }
            }
        /// <summary>
        /// Checks if two inserted pincodes match and are correct
        /// </summary>
        /// <param name="message">message to display when the pincode confirmation screen is shown</param>
        /// <returns></returns>
        private int PincodeConfirmation(string message) {
            while (true) {
                Console.WriteLine(message);
                var firstPincode = InputHandling.CheckNumber();
                Console.WriteLine("Type your pincode again");
                var secondPincode = InputHandling.CheckNumber();
                if(firstPincode.ToString().Length != 4) {
                    Console.WriteLine("Error: the pincode needs to be 4 digits, try it again");
                }
                else if (firstPincode.Equals(secondPincode)) {
                    return firstPincode;
                }
                else {
                    Console.WriteLine("Error: the two entered pincodes didn't match, try it again");
                }
            }
        }
        /// <summary>
        /// The deposit menu
        /// </summary>
        private void Deposit() {
            Console.WriteLine("Deposit \n How much do you like to deposit?");
            var givenAmmount = InputHandling.CheckNumber();
            queryAccess.accountTable.ChangeBalance((a) => a.AccountId == LoggedInUser.AccountId, givenAmmount, (a) => a.Money += givenAmmount);
            Console.WriteLine("The money has been deposit");
            
        }
        /// <summary>
        /// The change pincode menu
        /// </summary>
        private void ChangePincode() {
            Console.WriteLine("Change Pincode");
            var choosenPincode = this.PincodeConfirmation("What would you like to change your pincode to?");
            queryAccess.accountTable.ChangePincode(LoggedInUser.AccountId, choosenPincode);
            Console.WriteLine("Pincode has been changed");
        }
        /// <summary>
        /// The Withdraw menu
        /// </summary>
        private void Withdraw() {
            Console.WriteLine("Withdraw \n How much do you like to draw?");
            var givenAmmount = InputHandling.CheckNumber();
            if(this.IsGivenAmmountHigherThanBalance(givenAmmount)) {
                queryAccess.accountTable.ChangeBalance((a) => a.AccountId == LoggedInUser.AccountId, givenAmmount, (a) => a.Money -= givenAmmount);
                Console.WriteLine("Money has been Withdrawn");
            }
            else {
                Console.WriteLine("Error: given ammount should be higher than the current balance");
                this.BackRetryExit(() => this.MainMenuOptions(), () => this.Withdraw());
            }
        }
        /// <summary>
        /// The transfer menu
        /// </summary>
        private void Transfer() {
            Console.WriteLine("Transfer \n To which account to transfer money?");
            var givenBankNumber = InputHandling.CheckNumber();
            if (this.queryAccess.accountTable.CheckIfBankNumberExists(givenBankNumber)) {
                Console.WriteLine("How much to deposit?");
                var givenAmmount = InputHandling.CheckNumber();
                if (this.IsGivenAmmountHigherThanBalance(givenAmmount)) {
                    this.queryAccess.accountTable.ChangeBalance((a) => a.BankNumber == givenBankNumber, givenAmmount, (a) => a.Money += givenAmmount);
                    this.queryAccess.accountTable.ChangeBalance((a) => a.AccountId == LoggedInUser.AccountId, givenAmmount, (a) => a.Money -= givenAmmount);
                    Console.WriteLine("Money has been transfered");
                    return;
                }
                else {
                    Console.WriteLine("Error: given ammount is lower than balance, current balance: {0}", LoggedInUser.Balance);
                    this.BackRetryExit(() => this.MainMenuOptions(), () => this.Transfer());
                    return;
                }
            }
            Console.WriteLine("Error: Bank number does not exist");
            this.BackRetryExit(() => this.MainMenuOptions(), () => this.Transfer());

        }
        /// <summary>
        /// Checks if the given ammount is higher than user's current balance
        /// </summary>
        /// <param name="ammount">ammount of money that has been typed by the user</param>
        /// <returns></returns>
        private bool IsGivenAmmountHigherThanBalance(int ammount) {
            if(ammount <= LoggedInUser.Balance && ammount > 0) {
                return true;
            }
            return false;
        }
        /// <summary>
        /// The menu for check balance
        /// </summary>
        private void SeeBalance() {
            Console.WriteLine("The current balance is: {0}", LoggedInUser.Balance);
        }
    }
}
