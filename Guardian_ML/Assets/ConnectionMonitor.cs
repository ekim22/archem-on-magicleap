using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionMonitor {
    public static IEnumerator checkInternetConnection(Action<bool> action) {

        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            action(false);
        }
        else {
            action(true);
        }
    } 
}
