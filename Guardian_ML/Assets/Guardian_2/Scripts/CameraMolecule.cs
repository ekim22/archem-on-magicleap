using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Richard Smith
public class CameraMolecule : MonoBehaviour
{
    public static string moleculeCID;
    public static CameraMolecule instance = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}
