namespace SqlImport
{
    public class MappingField
    {
        public static MappingField Id { get {  return new MappingField("ID");} }
        public static MappingField Username { get {  return new MappingField("username");} }
        public static MappingField TransactionDate { get { return new MappingField("transaction date"); } }
        public static MappingField EntryTime { get { return new MappingField("transaction entry time"); } }
        public static MappingField NominalCode { get { return new MappingField("nominal code"); } }
        public static MappingField Amount { get { return new MappingField("amount"); } }
        public static MappingField Description { get { return new MappingField("description"); } }
        public static MappingField NominalName { get { return new MappingField("nominal code name"); } }
        public static MappingField Type { get { return new MappingField("transaction type"); } }

        private MappingField(string userFriendlyName)
        {
            UserFriendlyName = userFriendlyName;
        }

        public string UserFriendlyName { get; private set; }        

        public override string ToString()
        {
            return UserFriendlyName;
        }
    }
}