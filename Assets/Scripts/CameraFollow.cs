using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_SmoothSpeed = 0.125f;
    [SerializeField] private Vector3 m_Offset;

    private void Update()
    {
        Vector3 desiredPosition = m_Target.position + m_Offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_SmoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
        transform.LookAt(m_Target);
    }
}
