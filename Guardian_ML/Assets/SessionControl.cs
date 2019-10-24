using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionControl : MonoBehaviour
{
    public static SessionControl control;
    public static bool isShared = false;
    public static bool isLocal = false;

    public void Awake() {
        if (control == null) {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this) {
            Destroy(gameObject);
        }
    }
}
