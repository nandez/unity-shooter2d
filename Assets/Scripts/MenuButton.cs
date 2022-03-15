using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMPro.TMP_Text text;

    public Color MainColor;
    public Color HoverColor;

    public AudioClip hoverClip;
    public AudioClip clickAudio;

    public AudioSource audioSrc;


    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = HoverColor;
        PlayAudio(hoverClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = MainColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAudio(clickAudio);
    }

    protected void PlayAudio(AudioClip audio)
    {
        audioSrc.PlayOneShot(audio);
    }
}
