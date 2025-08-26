using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private AudioClip menuSelectClip;

    public void PlayButton()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayMenuSelectClip()
    {
        AudioSource.PlayClipAtPoint(menuSelectClip, gameObject.transform.position);
    }
}
