using System.Diagnostics.CodeAnalysis;

namespace ViewMaster.Core.Models.Common;

public class Degrees : IEquatable<Degrees>
{
    public Degrees(double pan, double tilt)
    {
        if (pan > 360)
        {
            throw new ArgumentOutOfRangeException(nameof(pan), "Pan must be a value less than 360");
        }
        if (tilt > 360)
        {
            throw new ArgumentOutOfRangeException(nameof(tilt), "Tilt must be a value less than 360");
        }

        this.x = pan;
        this.y = tilt;
    }

    public bool IsEmpty => x == 0 && y == 0;

    private double x; // Pan
    public double X => this.x;
    public double PanDegrees => this.x;

    private double y; // Tilt
    public double Y => this.y;
    public double TiltDegrees => this.y;

    public static implicit operator Degrees(Coordinate p)
    {
        return new Degrees(Math.Floor(p.X * ushort.MaxValue / 360.0), Math.Floor(p.Y * ushort.MaxValue / 360.0));
    }

    public static Degrees operator +(Degrees pt, Degrees sz) => Add(pt, sz);

    public static Degrees operator -(Degrees pt, Degrees sz) => Subtract(pt, sz);

    public static bool operator ==(Degrees left, Degrees right) => left.X == right.X && left.Y == right.Y;

    public static bool operator !=(Degrees left, Degrees right) => !(left == right);

    public static Degrees Add(Degrees left, Degrees right) => new((ushort)(left.X + right.X), (ushort)(left.Y + right.Y));

    public static Degrees Subtract(Degrees left, Degrees right) => new((ushort)(left.X - right.X), (ushort)(left.Y - right.Y));

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Degrees Degrees && Equals(Degrees);

    public bool Equals(Degrees? other)
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

    public void Offset(double dx, double dy)
    {
        unchecked
        {
            this.x += dx;
            this.y += dy;
        }
    }

    public void Offset(Degrees p) => Offset(p.X, p.Y);

    public override string ToString() => $"{{X={X},Y={Y}}}";
}