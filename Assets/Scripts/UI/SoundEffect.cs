using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public static SoundEffect Instance;

    [SerializeField] public AudioSource m_Source, m_WalkSource, m_GasLeakSource;
    [SerializeField] public AudioClip m_GetOxygen;
    [SerializeField] public AudioClip m_Win, m_Lose;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogError("There is probably more than one instance.", Instance);
            Object.Destroy(this);
        }
    }


    public void Walk(bool isMoving, bool iscrouch)
    {
        bool shouldPlay = isMoving && !iscrouch;
        this.PlayStopByState(this.m_WalkSource, shouldPlay);
    }

    public void GetOxygenTank()
    {
        m_Source.PlayOneShot(m_GetOxygen);
    }

    public void ReleaseOxygen(bool isMoving)
    {
        this.PlayStopByState(this.m_GasLeakSource, isMoving);
    }

    public void Win()
    {
        m_Source.PlayOneShot(m_Win);
    }
    public void Lose()
    {
        m_Source.PlayOneShot(m_Lose);
    }
    private void PlayStopByState(AudioSource source, bool shouldPlay)
    {
        if (!source.isPlaying && shouldPlay)
        {
            source.Play();
        } else if (source.isPlaying && !shouldPlay)
        {
            source.Stop();
        }
    }
    private void Update()
    {
        if(GameManager.Instance.GameState != GameState.InProgress)
        {
            this.m_GasLeakSource.Stop();
            this.m_WalkSource.Stop();
        } 

    }
}
