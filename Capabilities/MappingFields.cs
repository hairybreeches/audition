namespace Capabilities
{
    public class MappingFields
    {
        public static readonly IMappingField Id =  new IdField();
        public static readonly IMappingField Username = new UsernameField();
        public static readonly IMappingField Type = new TypeField();
        public static readonly IMappingField TransactionDate = new TransactionDateField();
        public static readonly IMappingField Description = new DescriptionField();
        public static readonly IMappingField NominalCode = new NominalCodeField();
        public static readonly IMappingField Amount = new AmountField();
        public static readonly IMappingField NominalName = new NominalNameField();

        private class IdField : MappingField
        {
            public IdField()
                : base("ID")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.Id;
            }
        }
        private class UsernameField : MappingField
        {
            public UsernameField()
                : base("username")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.Username;
            }
        }
        private class TransactionDateField : MappingField
        {
            public TransactionDateField()
                : base("transaction date")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.TransactionDate;
            }
        }
        private class NominalCodeField : MappingField
        {
            public NominalCodeField()
                : base("nominal code")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.AccountCode;
            }
        }
        private class AmountField : MappingField
        {
            public AmountField()
                : base("amount")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.Amount;
            }
        }
        private class DescriptionField : MappingField
        {
            public DescriptionField()
                : base("description")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.Description;
            }
        }
        private class NominalNameField : MappingField
        {
            public NominalNameField()
                : base("nominal code name")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.AccountName;
            }
        }
        private class TypeField : MappingField
        {
            public TypeField()
                : base("transaction type")
            {

            }
            public override T GetValue<T>(IMappingVisitor<T> visitor)
            {
                return visitor.Type;
            }
        }

        private abstract class MappingField : IMappingField
        {
            protected MappingField(string userFriendlyName)
            {
                UserFriendlyName = userFriendlyName;
            }

            public string UserFriendlyName { get; private set; }

            public override string ToString()
            {
                return UserFriendlyName;
            }

            public abstract T GetValue<T>(IMappingVisitor<T> visitor);
        }

    }
}