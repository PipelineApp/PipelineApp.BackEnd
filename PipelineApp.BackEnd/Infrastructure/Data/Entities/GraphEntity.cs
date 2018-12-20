namespace PipelineApp.BackEnd.Interfaces
{
    using Neo4j.Driver.V1;

    /// <summary>
    /// Base class for all data-layer entity objects.
    /// </summary>
    public abstract class GraphEntity
    {
        /// <summary>
        /// Loads the properties of an <see cref="IRecord"/> into the data model.
        /// </summary>
        /// <param name="record">Graph DB record.</param>
        public abstract void LoadRecord(IRecord record);
    }
}
