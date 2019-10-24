using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Created by Richard Smith
public class ChangeScene : MonoBehaviour {

    public Molecules molecules;
    public Canvas networkMessageCanvas;
    public Dropdown dropdown;
    public Button button;

    private string guardianScene = "GuardianARScene";
    private string predictiveScene = "MainScene";
    private string sessionScene = "SessionSelect";

    //
    public void switchToPrediction() {
        StartCoroutine(ConnectionMonitor.checkInternetConnection((isConnected) => {
            if (isConnected) {
                Debug.Log("Connected to Internet");

                CameraMolecule.moleculeCID = "Glucose"; // PLACE HOLDER FIX FOR CRASHES. PERM FIX WOULD BE TO SPLIT
                                                        // CAMERAMOLECULE MOLECULECID APART IN GUARDIANARCONTROLLER

                //This code will make iterate through the Molecules gameObject and make a copy of its children 
                //to another List of Transforms. This then allows the new children to have their parent assigned
                //to the MoleculesControl MoleculeList. This "saves" the data between scene changes
                if (molecules.transform.childCount > 0) {
                    List<Transform> copy = new List<Transform>();
                    foreach (Transform child in molecules.transform) {
                        copy.Add(child.gameObject.transform);
                    }
                    foreach (Transform child in copy) {
                        child.transform.parent = MoleculesControl.control.moleculeList.transform;
                        child.GetComponent<MeshRenderer>().enabled = false;
                        Component[] renderers = child.GetComponentsInChildren(typeof(MeshRenderer));
                        foreach (MeshRenderer renderer in renderers) {
                            renderer.enabled = false;
                        }
                    }
                }
                SceneManager.LoadScene(2);
            }
            else {
                Debug.Log("Not connected to Internet");
                dropdown.interactable = false;
                button.interactable = false;
                networkMessageCanvas.gameObject.SetActive(true);
            }
        }));
    }

    //
    public void switchToGuardian() {
        Debug.Log("MoleculeCID changed to " + CameraMolecule.moleculeCID);
        SceneManager.LoadScene(1);
        Debug.Log("Changed scene to Guardian");
    }

    //
    public void cancelAndSwitchScene() {
        CameraMolecule.moleculeCID = "Glucose";
        Debug.Log("MoleculeCID changed to " + CameraMolecule.moleculeCID);
        switchToGuardian();
    }
}
