using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Image fader;
    float fading = 1f;
    bool loaded = false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (fading < 0f)
        {
            fading += Time.deltaTime;
        }
        else if (fading < 1f)
        {
            if (loaded)
            {
                fading += Time.deltaTime;
            }
            else
            {
                fading = 0f;
            }
        }
        else
        {
            fading = 1f;
        }
        fader.color = new Color(0f, 0f, 0f, 1f - Mathf.Abs(fading));
    }

    public void ToGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        fading = -1f;
        loaded = false;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Game");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if ((asyncOperation.progress >= 0.9f) && (fading == 0f))
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
        loaded = true;
    }
}
