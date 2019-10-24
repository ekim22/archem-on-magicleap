using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by Richard Smith
public class MoleculesControl : MonoBehaviour
{
    
    public static MoleculesControl control;
    public GameObject moleculeList;
    //public Molecules molecules;

    void Awake()
    {
        if (control == null)
        {
            moleculeList = new GameObject("MoleculeList");
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(moleculeList);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    
}
