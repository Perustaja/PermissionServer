namespace PermissionServer.Common.Exceptions
{
    public class EntitiesNotRegisteredException : InvalidOperationException
    {
        public readonly string _customMessage;
        public override string Message => _customMessage;
        /// <summary>
        /// Represents an exception where the user attempted to add Entity Framework stores, but the 
        /// necessary entities were not found. Most likely due to the necessary extension not being called
        /// for the ModelBuilder.
        /// </summary>
        public EntitiesNotRegisteredException(params Type[] unregisteredTypes)
        {
            _customMessage = $"Provided DbContext did not have the following necessary entities added to the model: {String.Join(',', unregisteredTypes.ToList())}. Ensure that the necessary extension method is called within OnModelCreating().";
        }
    }
}