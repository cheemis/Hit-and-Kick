using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    //audio variables
    private AudioSource musicSource;
    [SerializeField]
    private AudioClip mainMusic;
    [SerializeField]
    private AudioClip gameOverMusic;



    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = mainMusic;
    }

    public void GameOverMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.clip = gameOverMusic;
            musicSource.Play();
            musicSource.loop = false;
        }
    }
}
