namespace GoogleSearchScrape.Abstractions.Interfaces
{
    /// <summary>
    /// Represents a object persisted in the database
    /// </summary>
    /// <typeparam name="T">Type of primary key</typeparam>
    public interface IDao<T>
    {
        T Id { get; set; }
    }
}
