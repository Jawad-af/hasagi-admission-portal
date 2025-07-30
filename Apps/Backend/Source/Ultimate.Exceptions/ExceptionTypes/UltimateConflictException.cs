namespace Ultimate.Exceptions.ExceptionTypes
{
    public class UltimateConflictException : Exception
    {
        public string Entity { get; set; }
        public UltimateConflictException(string entity) 
        {
            Entity = entity;
        }
    }
}
