using UnityEngine;

public class SpawnOxygen : MonoBehaviour
{
    [SerializeField] private int m_TankNumber;
    [SerializeField] private GameObject m_OxygenTankPrefab;

    private void Start()
    {
        this.SpawnOxygenTank(this.m_TankNumber);
    }

    public void SpawnOxygenTank(int times)
    {
        for (int i = 0; i < times; i++)
        {
            int x, y;
            GameObject oxygenTank = Object.Instantiate(this.m_OxygenTankPrefab, this.transform);
            GameManager.Instance.MazeGenerator.GetRandomGridPosition(out x, out y);

            GameManager.Instance.MazeGenerator.PlaceObject(
                oxygenTank.transform, x, y
            );
        }
    }
}
