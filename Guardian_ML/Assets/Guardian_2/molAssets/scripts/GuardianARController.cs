using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GuardianARController : MonoBehaviour {

    public EventSystem eventSystem;
    public Dropdown dropdown;

    public Camera FirstPersonCamera;           // The first-person camera being used with AR background
    public GameObject DetectedPlanePrefab;     // A prefab for tracking and visualizing detected planes
    public GameObject SearchingForPlaneUI;     // A game object parenting UI for displaying the "searching for planes" snackbar.
    private bool m_IsQuitting = false;         //True if the app is in the process of quitting due to an ARCore connection error, otherwise false
	public Molecules molecules;

	public string MoleculeCID { get => CameraMolecule.moleculeCID; set => CameraMolecule.moleculeCID = value; }

    private bool transferedMolecules = false; // Associated with the MoleculeList from the Singleton MoleculesControl. Will change when plane
                                              // is detected and transfer executes first and only time

	public void Start()
    {
        if (dropdown != null) {
            dropdown.onValueChanged.AddListener(
            (int index) => {
                GetComponent<MoleculeDropdown>().
                Molecules.TryGetValue(dropdown.options[index].text, out int textCID);
                CameraMolecule.moleculeCID = textCID.ToString();
                // TODO: Perm fix to split moleculeCID into string and int values
            });
        }
        else {
            Debug.Log("No dropdown item selected!");
        }

        Debug.Log("Start " + CameraMolecule.moleculeCID);

    }

	//public void Spawn(LeanFinger finger) {

	//	Debug.Log("Spawn() ....");

 //       Debug.Log("Spawn " + CameraMolecule.moleculeCID);
 //   }

	public void Update()
    {
        _UpdateApplicationLifecycle();

    }

    private void _UpdateApplicationLifecycle() { // Check and update the application lifecycle.
    

        if (Input.GetKey(KeyCode.Escape)) { // Exit the app when the 'back' button is pressed.
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        

        if (m_IsQuitting) {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
       
    }

    private void _DoQuit() {
        Application.Quit();
    }


    private void _ShowAndroidToastMessage(string message) {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null) {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                    message, 0);
                toastObject.Call("show");
            }));
        }
    }

    private void _TransferMoleculeList() {
        //Similar to the code in switchToPrediction, this will iterate through the MoleculeList in MoleculeControl
        //and make a copy of its children to a new List of Transforms. This then allows for a safe transfer of the children
        //objects back to the Molecules gameObject in the scene to "reload saved" data from prior use in this session
        if (MoleculesControl.control.moleculeList.transform.childCount > 0) {
            List<Transform> copy = new List<Transform>();
            foreach (Transform child in MoleculesControl.control.moleculeList.transform) {
                copy.Add(child.gameObject.transform);
                Debug.Log("Copied from MoleculeControl to copy");
            }
            foreach (Transform child in copy) {
                child.transform.parent = molecules.transform;
                child.GetComponent<MeshRenderer>().enabled = true;
                Component[] renderers = child.GetComponentsInChildren(typeof(MeshRenderer));
                foreach (MeshRenderer renderer in renderers) {
                    renderer.enabled = true;
                }
                Debug.Log("Copied from copy to Molecules");
            }
        }
    }
    
}