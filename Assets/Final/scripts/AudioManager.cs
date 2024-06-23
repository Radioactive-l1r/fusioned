using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
   
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

   
    private AudioSource audioSource;

   
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        LoadAudioClips();

        PlayAudio("btn_hover");
        setbtnAudio();
    }

    
    private void LoadAudioClips()
    {
       
        AudioClip[] clips = Resources.LoadAll<AudioClip>("musics");
        foreach (AudioClip clip in clips)
        {
            audioClips[clip.name] = clip;
        }
    }

    // Play an audio clip by name
    public void PlayAudio(string clipName)
    {
        if (audioClips.ContainsKey(clipName))
        {
            audioSource.clip = audioClips[clipName];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clipName);
        }
    }
    //set audio for all buttons
    public void setbtnAudio()
    {
        Button[] buttons = FindObjectsOfType<Button>();

        foreach (Button btn in buttons)
        {
            // Add listeners for OnClick and OnHover events
            btn.onClick.AddListener(() => PlayAudio("btn_click"));

            // Add EventTrigger for hover
            EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = btn.gameObject.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            entry.callback.AddListener((eventData) => { PlayAudio("btn_hover"); });
            trigger.triggers.Add(entry);
        }
    }
}
