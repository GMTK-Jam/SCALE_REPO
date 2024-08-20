using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    //tutorial

    public List<CanvasGroup> tutorials = new List<CanvasGroup>();
    private bool isPlayingTutorial;

    void OnEnable(){
        if (!isPlayingTutorial)
        StartCoroutine(callTutorial());
    }

    IEnumerator callTutorial(){
        isPlayingTutorial = true;
        Time.timeScale = 0;
        foreach (var s in tutorials) {
        s.alpha = 1;
        yield return new WaitForSecondsRealtime(5);
        s.alpha = 0;
        }
        Time.timeScale = 1;
    }
}
