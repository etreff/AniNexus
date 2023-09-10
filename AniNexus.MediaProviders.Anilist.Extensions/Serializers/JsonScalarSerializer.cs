using StrawberryShake.Serialization;

namespace AniNexus.MediaProviders.Anilist.Serializers;

/// <summary>
/// A serializer for deserializing a JSON scalar from a GraphQL response.
/// </summary>
public sealed class JsonScalarSerializer : JsonSerializer
{
    /// <summary>
    /// Creates a new <see cref="JsonScalarSerializer"/> instance.
    /// </summary>
    public JsonScalarSerializer()
        : base("Json")
    {
    }
}
