using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SessionSelectController : MonoBehaviour
{
    public Text text;
    public GameObject panel;
    public GameObject networkError;
    public int scene;

    public void SetToShared() {
        StartCoroutine(ConnectionMonitor.checkInternetConnection((isConnected) => {
            if (isConnected) {
                SessionControl.isShared = true;
                SessionControl.isLocal = false;
                SceneManager.LoadScene(scene);
            } else {
                text.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                networkError.gameObject.SetActive(true);
            }
        }));
    }

    public void SetToLocal() {
        SessionControl.isShared = false;
        SessionControl.isLocal = true;
        SceneManager.LoadScene(scene);
    }
}
