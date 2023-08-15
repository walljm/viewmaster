using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ViewMaster.Core.Models.Export;

namespace ViewMaster.Core.Models.Common;

/// <summary>
/// A set of coordinates, expressed in degrees with a range of 0 - T.MaxValue
/// </summary>
public class Degrees<T> where T : INumber<T>, IMinMaxValue<T>, IConvertible
{
    public Degrees(T pan, T tilt)
    {
        this.x = pan % T.MaxValue;
        this.y = tilt % T.MaxValue;
    }

    public bool IsEmpty => x.Equals(T.Zero) && y.Equals(T.Zero);

    private T x; // Pan
    public T X => this.x;
    public T PanValue => this.x;

    private T y; // Tilt
    public T Y => this.y;
    public T TiltValue => this.y;

    public static Degrees<T> operator +(Degrees<T> pt, Degrees<T> sz) => Add(pt, sz);

    public static Degrees<T> operator -(Degrees<T> pt, Degrees<T> sz) => Subtract(pt, sz);

    public static bool operator ==(Degrees<T> left, Degrees<T> right) => left.X == right.X && left.Y == right.Y;

    public static bool operator !=(Degrees<T> left, Degrees<T> right) => !(left == right);

    public static Degrees<T> Add(Degrees<T> left, Degrees<T> right) => new(left.X + right.X, left.Y + right.Y);

    public static Degrees<T> Subtract(Degrees<T> left, Degrees<T> right) => new(left.X - right.X, left.Y - right.Y);

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Degrees<T> coordinate && Equals(coordinate);

    public bool Equals(Degrees<T>? other)
    {
        if (other is null && this is null)
        {
            return true;
        }

        if (other is null || this is null)
        {
            return false;
        }

        return this == other;
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public void Offset(T dx, T dy)
    {
        unchecked
        {
            this.x += dx;
            this.y += dy;
        }
    }

    public void Offset(Degrees<T> p) => Offset(p.X, p.Y);

    public override string ToString() => $"{{X={X},Y={Y}}}";
}

/// <summary>
/// A set of coordinates, expressed in degrees with a range of 0 - UInt16.MaxValue
/// </summary>
public class UDegrees16 : Degrees<ushort>
{
    public UDegrees16(ushort pan, ushort tilt) : base(pan, tilt)
    {
    }

    public static implicit operator UDegrees16(Degrees<double> val)
    {
        return new UDegrees16(
            Convert.ToUInt16(val.X * (ushort.MaxValue / 360.0)), // convert steps from 360 degree decimals to ushort.MaxValue integer
            Convert.ToUInt16(val.Y * (ushort.MaxValue / 360.0))
        );
    }

    public static implicit operator UDegrees16(DegreeData p)
    {
        return new Degrees<double>(p.Pan, p.Tilt);
    }
}

/// <summary>
/// A set of coordinates, expressed in degrees with a range of 0 - 360
/// where pan:0 is forward and tilt:0 is forward.
/// </summary>
public class Degrees360 : Degrees<double>
{
    public Degrees360(double pan, double tilt) : base(
        // make north (0 degrees) straight forward for pan and tilt.
        (pan + 180) % 360,
        (tilt + 180) % 360
    )
    {
    }

    public static implicit operator Degrees360(Degrees<ushort> val)
    {
        return new Degrees360(
            Convert.ToUInt16(val.X * (ushort.MaxValue / 360.0)), // convert steps from 360 degree decimals to ushort.MaxValue integer
            Convert.ToUInt16(val.Y * (ushort.MaxValue / 360.0))
        );
    }

    public static implicit operator Degrees360(DegreeData p)
    {
        return new Degrees360(p.Pan, p.Tilt);
    }
}
