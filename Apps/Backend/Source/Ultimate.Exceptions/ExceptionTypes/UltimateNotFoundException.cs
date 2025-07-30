namespace Ultimate.Exceptions.ExceptionTypes
{
    public class UltimateNotFoundException : Exception
    {
        public string Entity { get; set; }
        public UltimateNotFoundException(string entity)
        {
            Entity = entity;
        }
    }
}
