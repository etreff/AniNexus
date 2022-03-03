using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Geo;

/// <summary>
/// A global coordinate.
/// </summary>
[DebuggerDisplay("{" + nameof(Latitude) + "}, {" + nameof(Longitude) + "}")]
public class Coordinate : IEquatable<Coordinate>
{
    /// <summary>
    /// The radius of the Earth in kilometers. This is the average of the
    /// authalic and approximation radii.
    /// </summary>
    private const double _earthRadius = 6371.9;
    private const double _piOver180 = Math.PI / 180;

    /// <summary>
    /// The latitude.
    /// </summary>
    public double Latitude
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _latitudeField;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _latitudeField = WrapLatitude(value);
    }

    /// <summary>
    /// The longitude.
    /// </summary>
    public double Longitude
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _longitudeField;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set => _longitudeField = WrapLongitude(value);
    }

    private double _latitudeField;
    private double _longitudeField;

    /// <summary>
    /// Creates a new <see cref="Coordinate"/> object.
    /// </summary>
    public Coordinate()
        : this(0, 0)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Coordinate"/> object, wrapping
    /// <paramref name="latitude"/> and <paramref name="longitude"/>
    /// to be valid values.
    /// </summary>
    /// <param name="latitude">The latitude.</param>
    /// <param name="longitude">The longitude.</param>
    public Coordinate(double latitude, double longitude)
        : this(latitude, longitude, false)
    {
    }

    /// <summary>
    /// Creates a new <see cref="Coordinate"/> object, specifying
    /// whether to validate the original inputs are within range.
    /// </summary>
    /// <param name="latitude">The latitude.</param>
    /// <param name="longitude">The longitude.</param>
    /// <param name="validate">
    ///     Specify <see langword="true"/> to throw an <see cref="ArgumentOutOfRangeException"/> if
    ///     <paramref name="latitude"/> and <paramref name="longitude"/> are not strictly valid
    ///     values. Specify <see langword="false"/> to wrap the inputs to make them valid values.
    /// </param>
    public Coordinate(double latitude, double longitude, bool validate)
    {
        if (validate)
        {
            VerifyLatitude(latitude);
            VerifyLongitude(longitude);
        }

        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Returns whether the provided latitude is valid.
    /// </summary>
    /// <param name="latitude">The latitude value.</param>
    public static bool IsValidLatitude(double latitude)
    {
        return latitude >= -90 && latitude <= 90;
    }

    /// <summary>
    /// Returns whether the provided longitude is valid.
    /// </summary>
    /// <param name="longitude">The longitude value.</param>
    public static bool IsValidLongitude(double longitude)
    {
        return longitude >= -180 && longitude <= 180;
    }

    /// <summary>
    /// Wraps <paramref name="latitude"/> so that the value is a valid latitude.
    /// </summary>
    /// <param name="latitude">The value to wrap.</param>
    public static double WrapLatitude(double latitude)
    {
        return Modulo(latitude, 90);
    }

    /// <summary>
    /// Wraps <paramref name="longitude"/> so that the value is a valid longitude.
    /// </summary>
    /// <param name="longitude">The value to wrap.</param>
    public static double WrapLongitude(double longitude)
    {
        return Modulo(longitude, 180);
    }

    /// <summary>
    /// Gets the distance, in kilometers, between this coordinate and another coordinate.
    /// This method is fast but less accurate.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/></exception>
    public double GetHaversineDistance([NotNull] Coordinate other)
    {
        Guard.IsNotNull(other, nameof(other));

        return GetHaversineDistance(other.Latitude, other.Longitude);
    }

    /// <summary>
    /// Gets the distance, in kilometers, between this coordinate and another coordinate.
    /// This method is fast but less accurate.
    /// </summary>
    public double GetHaversineDistance(double otherLatitude, double otherLongitude)
        => GetHaversineDistance(Latitude, Longitude, otherLatitude, otherLongitude);

    /// <summary>
    /// Gets the distance, in kilometers, between two coordinates.
    /// This method is fast but less accurate.
    /// </summary>
    public static double GetHaversineDistance(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
    {
        double latA = latitudeA * _piOver180;
        double latB = latitudeB * _piOver180;
        double deltaLat = (latitudeB - latitudeA) * _piOver180;
        double deltaLong = (longitudeB - longitudeA) * _piOver180;

        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                   Math.Cos(latA) * Math.Cos(latB) *
                   Math.Sin(deltaLong / 2) * Math.Sin(deltaLong / 2);
        double c = Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)) * 2;

        return c * _earthRadius;
    }

    /// <summary>
    /// Deconstructs the object.
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    public void Deconstruct(out double latitude, out double longitude)
    {
        latitude = Latitude;
        longitude = Longitude;
    }

    /// <summary>
    /// Returns the REAL modulo of two numbers.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double Modulo(double a, double b)
    {
        double r = a % b;
        return r.Equals(a) ? a : r < 0 ? r + b : r - b;
    }

    private static void VerifyLatitude(double latitude)
    {
        if (!IsValidLatitude(latitude))
        {
            throw new ArgumentOutOfRangeException(nameof(latitude));
        }
    }

    private static void VerifyLongitude(double longitude)
    {
        if (!IsValidLongitude(longitude))
        {
            throw new ArgumentOutOfRangeException(nameof(longitude));
        }
    }

    /// <inheritdoc />
    public bool Equals(Coordinate? other)
    {
        return other is not null && (ReferenceEquals(this, other) || (Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude)));
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is not null && (ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((Coordinate)obj)));
    }

    /// <summary>
    /// Checks two instances for equality.
    /// </summary>
    public static bool operator ==(Coordinate? left, Coordinate? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Checks two instances for inequality.
    /// </summary>
    public static bool operator !=(Coordinate? left, Coordinate? right)
    {
        return !Equals(left, right);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }
}
