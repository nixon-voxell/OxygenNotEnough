using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float m_SmoothSpeed;
    [SerializeField] private Vector3 m_Offset;

    private void Update()
    {
        Player player = GameManager.Instance.Player;
        if (player == null) return;

        Vector3 desiredPosition = player.transform.position + m_Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_SmoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }
}
