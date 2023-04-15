using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct PopulateMazeWallJob : IJobFor
{
    public int StartIndex;
    public int ColumnCount;

    public NativeArray<MazeWall> na_Walls;

    public void Execute(int rowIdx)
    {
        for (int c = 0; c < this.ColumnCount; c++)
        {
            int rowStartIdx = this.StartIndex + rowIdx * this.ColumnCount;
            this.na_Walls[rowStartIdx + c] =
                new MazeWall(
                    new int2(rowIdx, c),
                    new int2(rowIdx, c + 1)
                );
        }
    }
}
