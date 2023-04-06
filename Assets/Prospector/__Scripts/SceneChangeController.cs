using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChangeController : MonoBehaviour
{
    public GameObject a;
    public GameObject aa;

    public Button btnCardgame;
    public Button btnProspector;
    private int number;

   // public Animator anmator;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.a);
        GameObject.DontDestroyOnLoad(this.gameObject);

        btnProspector.onClick.AddListener(LoadSceneProspector);
        btnCardgame.onClick.AddListener(LoadScenebtnCardgame);

    }
    public void LoadSceneProspector()
    {
        StartCoroutine(LoadScene(1));
    }

    public void LoadScenebtnCardgame()
    {
        StartCoroutine(LoadScene(2));
    }

    IEnumerator LoadScene(int index)
    {
        //anmator.SetBool("FadeIn", true);
        //anmator.SetBool("FadeOut", false);
        yield return new WaitForSeconds(1);
        AsyncOperation a = SceneManager.LoadSceneAsync(index);
        a.completed += OnLoadedScene;
        aa.SetActive(false);

    }


    private void OnLoadedScene(AsyncOperation obj)
    {
        //anmator.SetBool("FadeIn", false);
        //anmator.SetBool("FadeOut", true);
    }
    // Update is called once per frame
    void Update()
    {
    }
}

