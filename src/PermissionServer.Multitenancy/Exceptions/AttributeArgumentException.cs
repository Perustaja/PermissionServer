namespace PermissionServer.Multitenancy.Exceptions
{
    public class AttributeArgumentException : ArgumentException
    {
        public readonly string _customMessage;
        public override string Message => _customMessage;
        /// <summary>
        /// Represents an exception where the tenant information is sourced through HttpContext RouteData,
        /// and the expected key was not found.
        /// </summary>
        public AttributeArgumentException(Type registeredEnumType, Type argEnumType)
        {
            _customMessage = $"Improper permission enum used as an argument in an authorization attribute. Registered type: {registeredEnumType}, type used in attribute: {argEnumType}. Ensure all attributes use only the enum used when registering PermissionServer in DI.";
        }
    }
}