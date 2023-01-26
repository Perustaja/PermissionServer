namespace PermissionServer.Common.Exceptions
{
    public class AttributeArgumentException : ArgumentException
    {
        public readonly string _customMessage;
        public override string Message => _customMessage;
        /// <summary>
        /// Represents an exception where an enum other than what was registered in DI with PermissionServer is used in an authorization attribute.
        /// </summary>
        public AttributeArgumentException(Type registeredEnumType, Type argEnumType)
        {
            _customMessage = $"Improper permission enum used as an argument in an authorization attribute. Registered type: {registeredEnumType}, type used in attribute: {argEnumType}. Ensure all attributes use only the enum used when registering PermissionServer in DI.";
        }
    }
}