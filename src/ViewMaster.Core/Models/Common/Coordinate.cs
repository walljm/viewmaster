using System.Diagnostics.CodeAnalysis;
using ViewMaster.Core.Models.Export;

namespace ViewMaster.Core.Models.Common;

public class Coordinate : IEquatable<Coordinate>
{
    public Coordinate(ushort pan, ushort tilt)
    {
        this.x = pan;
        this.y = tilt;
    }

    public bool IsEmpty => x == 0 && y == 0;

    private ushort x; // Pan
    public ushort X => this.x;
    public ushort PanCoordinate => this.x;

    private ushort y; // Tilt
    public ushort Y => this.y;
    public ushort TiltCoordinate => this.y;

    public static implicit operator Coordinate(Degrees p)
    {
        return new Coordinate(Convert.ToUInt16(p.X * (ushort.MaxValue / 360.0)), Convert.ToUInt16(p.Y * (ushort.MaxValue / 360.0)));
    }

    public static implicit operator Coordinate(DegreeData p)
    {
        return new Degrees(p.Pan, p.Tilt);
    }

    public static Coordinate operator +(Coordinate pt, Coordinate sz) => Add(pt, sz);

    public static Coordinate operator -(Coordinate pt, Coordinate sz) => Subtract(pt, sz);

    public static bool operator ==(Coordinate left, Coordinate right) => left.X == right.X && left.Y == right.Y;

    public static bool operator !=(Coordinate left, Coordinate right) => !(left == right);

    public static Coordinate Add(Coordinate left, Coordinate right) => new((ushort)(left.X + right.X), (ushort)(left.Y + right.Y));

    public static Coordinate Subtract(Coordinate left, Coordinate right) => new((ushort)(left.X - right.X), (ushort)(left.Y - right.Y));

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Coordinate coordinate && Equals(coordinate);

    public bool Equals(Coordinate? other)
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

    public void Offset(ushort dx, ushort dy)
    {
        unchecked
        {
            this.x += dx;
            this.y += dy;
        }
    }

    public void Offset(Coordinate p) => Offset(p.X, p.Y);

    public override string ToString() => $"{{X={X},Y={Y}}}";
}
