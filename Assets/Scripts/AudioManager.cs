using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip GetItemClip;
    public AudioClip DieClip;
    public AudioClip HurtClip;
    public AudioClip OnButtonClip;
    public AudioSource Source;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
        Source = gameObject.GetComponent<AudioSource>();
    }
    public void PlayGetItem()
    {
        Source.clip = GetItemClip;
        Source.Play();
    }
    public void PlayDie()
    {
        Source.clip = DieClip;
        Source.Play();
    }
    public void PlayHurt()
    {
        Source.clip = HurtClip;
        Source.Play();
    }
    public void PlayOnButton()
    {
        Source.clip = OnButtonClip;
        Source.Play();
    }
}
