namespace The_ATM_Machine.Queries {
    /// <summary>
    /// For access to both tables (the whole database)
    /// </summary>
    public class QueryAccess {
        public AccountQueries accountTable = new AccountQueries();
        public UserQueries user = new UserQueries();
    }
}
