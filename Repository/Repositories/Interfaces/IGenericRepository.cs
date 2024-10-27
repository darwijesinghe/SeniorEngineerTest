namespace Repository.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface for CRUD operations
    /// </summary>
    /// <typeparam name="T">The type of the entity</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Retrieves all entities from the data source.
        /// </summary>
        /// <returns>
        /// A collection of all entities in the data source.
        /// </returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Finds and returns an entity based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>
        /// The entity matching the provided identifier, or empty entity if not found.
        /// </returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new entity to the data source and saves the changes.
        /// </summary>
        /// <param name="entity">The entity to be added to the data source.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        Task<string> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity in the data source with new values.
        /// </summary>
        /// <param name="entity">The entity with updated values to be saved.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        Task<string> UpdateAsync(T entity);

        /// <summary>
        /// Removes an entity from the data source based on its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete.</param>
        /// <returns>
        /// An error message if the operation fails; otherwise, null.
        /// </returns>
        Task<string> DeleteAsync(int id);

    }
}
