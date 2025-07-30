using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager Instance;

    public AudioSource sfxSource;
    public AudioClip bounceClip;
    public AudioClip goalClip;
    public AudioClip buttonClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayBounce() => PlaySFX(bounceClip);
    public void PlayGoal() => PlaySFX(goalClip);
    public void PlayButton() => PlaySFX(buttonClip);
    public void PlayWin() => PlaySFX(winClip);
    public void PlayLose() => PlaySFX(loseClip);
}
