using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Burst;

public static class MazeUtil
{
    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FlattenIndex(int x, int y, int width)
    {
        return x + y * width;
    }

    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int FlattenIndex(int2 cell, int length)
    {
        return FlattenIndex(cell.x, cell.y, length);
    }

    public const uint PRIME0 = 73856093u;
    public const uint PRIME1 = 19349663u;
    public const uint PRIME2 = 83492791u;

    // hash functions from: https://matthias-research.github.io/pages/publications/tetraederCollision.pdf
    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Hash3D(uint x, uint y, uint z, uint tableSize)
    {
        return ((x * PRIME0) ^ (y * PRIME1) ^ (z * PRIME2)) % tableSize;
    }

    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Hash2D(uint x, uint y, uint tableSize)
    {
        return ((x * PRIME0) ^ (y * PRIME1)) % tableSize;
    }

    /// <summary>Pop the last element from the list.</summary>
    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Pop<T>(this NativeList<T> na_list)
    where T : unmanaged
    {
        na_list.RemoveAt(na_list.Length - 1);
    }

    /// <summary>Pop the last element from the list.</summary>
    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T LastElement<T>(this NativeList<T> na_list)
    where T : unmanaged
    {
        return na_list[na_list.Length - 1];
    }
}
