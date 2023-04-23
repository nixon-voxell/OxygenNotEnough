using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSoundEffect : MonoBehaviour
{
    [SerializeField] public AudioSource m_Source;
    [SerializeField] public AudioClip m_ClickButton;
    public void ButtonClicks()
    {
        m_Source.PlayOneShot(m_ClickButton);
    }
}
