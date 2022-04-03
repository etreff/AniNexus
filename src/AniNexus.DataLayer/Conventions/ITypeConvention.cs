namespace AniNexus.Data.Conventions
{
    /// <summary>
    /// Conventions for a specific type.
    /// </summary>
    public interface ITypeConvention
    {
        /// <summary>
        /// Configures the conventions for the property type.
        /// </summary>
        /// <param name="builder">The model configuration builder.</param>
        void Configure(ModelConfigurationBuilder builder);
    }

    /// <summary>
    /// Conventions for a specific type.
    /// </summary>
    /// <typeparam name="T">The type to apply the convention to.</typeparam>
    public interface ITypeConvention<T> : ITypeConvention
    {
        void ITypeConvention.Configure(ModelConfigurationBuilder builder)
            => Configure(builder, builder.Properties<T>());

        /// <summary>
        /// Configures the conventions for the property type.
        /// </summary>
        /// <param name="builder">The model configuration builder.</param>
        /// <param name="properties">The properties builder for type <typeparamref name="T"/>.</param>
        void Configure(ModelConfigurationBuilder builder, PropertiesConfigurationBuilder<T> properties);
    }
}
