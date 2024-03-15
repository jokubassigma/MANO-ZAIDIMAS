using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    public string sceneName;
    private bool isSceneLoading = false;
    private void Start()
    {
        fadeImage.color = Color.black;
        fadeImage.CrossFadeColor(Color.clear, 1f, true, true);
    }

    void OnTriggerEnter2D(Collider2D other) // Idek "Press {kazkoks mygtukas} to go the forest. Forest "{kazkoks mygtukas"}
    {
        if (other.CompareTag("Player"))
        {
            if (!isSceneLoading)
            {
                StartCoroutine(LoadScene());
            }
        }
    }

    public IEnumerator LoadScene()
    {
        isSceneLoading = true;
        fadeImage.CrossFadeColor(Color.black, 1f, true, true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
    }
    
    
}