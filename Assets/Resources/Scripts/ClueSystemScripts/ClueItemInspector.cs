using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using Yarn.Unity;
using TeaspoonTools.TextboxSystem;

public class ClueItemInspector : MonoBehaviour
{
    public static ClueItemInspector S;			// Singleton of this class;
    public UnityEvent   DoneDisplayingClue = new UnityEvent();
    public ClueItem     currentClue;
    public GameObject   cloneClue;
    public ClueItem     cloneClueItem;

    public GameObject   clueUIWindow;
   
    public Text         itemNameDisplay;
    public Text         itemDescripDisplay;

    public GameObject   currentRoom;

    // components
    private Collider    cloneClueCol;
    public Vector3      cloneCluePos = new Vector3(0f, 0f, -4f); 

    //Vector3 cluePrevPos;
    //Quaternion cluePrevRot;

    Vector3 centerOfClone { get { return cloneClueCol.bounds.center; } }
    // ^ better to have this as a property, so we don't have to update a var constantly in Update.
    // Can reduce how many calls on the collider are done, too, so potential performance boost!

    public static bool inspectingItem
    {
        get { return S.clueUIWindow.activeSelf; }
    }

    [SerializeField]
    private bool touchControlled;


    // Set the LayerMask _clueLayer to the appropriate layer.

    private LayerMask _clueLayer;

    private Camera mainCam;
    public float rayCastDistance = 20;

    private Vector3 _lightOffset = new Vector3(0f, 2f, -3f);
    GameObject inspectionLight = null; // cache, so we can destroy and recreate it as needed

    // methods

    void Awake()
    {
        S = this;
        // better to set up component refs in Awake, since it executes before Start
        mainCam = Camera.main;
        _clueLayer = 1 << LayerMask.NameToLayer("Clue");

        
        //Attempt to find UI Text by name if not plugged in already
        if (itemNameDisplay == null)
        {
            itemNameDisplay = GameObject.Find("ItemName").GetComponent<Text>();
        }

        if (itemDescripDisplay == null)
        {
            itemDescripDisplay = GameObject.Find("ItemDescription").GetComponent<Text>();
        }
        
    }

    void Update()
    {
        
        if (!DialogueRunner.S.isDialogueRunning && Textbox.textboxesOnScreen == 0)
        {
            ClickItem();
            //Does not bother calling HandleRotation if the currentClue has already been set
            if (cloneClueItem != null)
                HandleRotation(ref cloneClueItem);
        }
    }

    public void DisplayClue(ClueItem clueToDisplay, bool onlyInLog = true)
    {
        /*
         * Displays the clue passed. If onlyInLog is true, it only displays it if 
         * it is in log. Otherwise, it doesn't.
         */

        bool clueInLog = false;
        ClueItem currentClueItem = null;

        if (onlyInLog)
        {
            // check if the clue item is in the log
            foreach (GameObject prefab in UIController.S.loggedCluePrefabs)
            {
                currentClueItem = prefab.GetComponent<ClueItem>();
                if (currentClueItem == clueToDisplay)
                {
                    clueInLog = true;
                    break;
                }
                
            }

            if (currentClueItem != clueToDisplay)
                throw new System.ArgumentException("Passed clue item is not in the clue log.");

            UIController.S.HideCaptainsLog(CaptainsLogMenus.clueLog);
            SetCurrentClue(ref clueToDisplay);
            HandleClueViewing(ref clueToDisplay);
            clueUIWindow.SetActive(true);
        }
        else
        {
            // check the whole clue database for the clue to display
            currentClueItem = (from GameObject prefab in ClueDatabase.S.cluePrefabs
                               where prefab.GetComponent<ClueItem>() == clueToDisplay
                               select prefab.GetComponent<ClueItem>()).First();

            if (currentClueItem != null)
            {
                UIController.S.HideCaptainsLog(CaptainsLogMenus.clueLog);
                SetCurrentClue(ref clueToDisplay);
                HandleClueViewing(ref clueToDisplay);
                clueUIWindow.SetActive(true);
            }
            else
                throw new System.ArgumentException("Passed clue item is not in the clue database.");

        }

        
    }


    /********************************************
	* HELPER FUNCTIONS							*
	*********************************************/
    void ClickItem()
    {
        //TODO Look into making this mobile compatible
        if (Input.GetMouseButtonUp(0))
        {
            print("Mouse Button Released");
            Ray pos = mainCam.ScreenPointToRay(Input.mousePosition);
            print("Position: " + pos);
            RaycastHit objectHit;
            Debug.DrawRay(pos.origin, pos.direction * rayCastDistance, Color.red, 5f);
            //TODO Put clues on a specific layer and have the Raycast search only that layer. That way our raycasting is more efficient.
            if (Physics.Raycast(pos.origin, pos.direction, out objectHit, rayCastDistance))
            {
                if ((objectHit.collider.CompareTag("Clue") || 
                    objectHit.collider.CompareTag("Prop")) && !inspectingItem)
                {
                    Debug.Log("You have selected the " + objectHit.transform.GetComponent<ClueItem>().ItemName);

                    // Open Clue UI Window before spawning the clone 
                    clueUIWindow.SetActive(true);            

                    // Set clueItem to the object that we just hit with the Raycast
                    // cache the ClueItem script for a performance boost.
                    SetCurrentClue(ref objectHit);
                    // If the object we hit is tagged as a Clue or Prop, then bring it up for inspection
                    HandleClueViewing(ref currentClue);
                }

            }
        }
    }

    public void SetCurrentClue(ref RaycastHit objectHit)
    {
        if (currentClue == null)
        {
            //Separate ClueItem script management from clone manipulation

            currentClue = objectHit.collider.GetComponent<ClueItem>();
            print("current clue:" + currentClue.ItemName);

            itemNameDisplay.text = currentClue.ItemName;
            itemDescripDisplay.text = currentClue.Description;

            //Vector3 location to spawn clone
            Vector3 desiredViewingLocation = cloneCluePos;

            cloneClue = Instantiate(    objectHit.collider.gameObject, 
                                        desiredViewingLocation, 
                                        Quaternion.Euler(0, 0, 0));
            cloneClue.transform.localScale *= currentClue.cloneScale;
            cloneClue.transform.rotation = Quaternion.Euler(currentClue.cloneRot.x, 
                                                            currentClue.cloneRot.y, 
                                                            currentClue.cloneRot.z);

            CanvasClueObject.S.SetCloneClue(ref cloneClue);


            cloneClueCol = cloneClue.GetComponent<Collider>();
            cloneClueItem = cloneClue.GetComponent<ClueItem>();
            //cloneClueItem.isCollected = true;

            cloneClueCol.enabled = false;
            cloneClueItem.enabled = false;
        }


        //Needs to be replaced by clicking out
        /*
        else if (objectHit.collider.GetComponent<ClueItem>() != currentClue)
        {
            ResetCurrentClue();
        }
        */
    }

    public void SetCurrentClue(ref ClueItem clueItem)
    {
        if (currentClue == null)
        {
            clueUIWindow.SetActive(true);
            currentClue = clueItem;
            print("current clue:" + currentClue.ItemName);

            itemNameDisplay.text = currentClue.ItemName;
            itemDescripDisplay.text = currentClue.Description;

            //Vector3 location to spawn clone
            Vector3 desiredViewingLocation = cloneCluePos;

            cloneClue = Instantiate(ClueDatabase.S.GetCluePrefab(clueItem.name),
                                    desiredViewingLocation,
                                    Quaternion.Euler(0, 0, 0));
            
            cloneClue.transform.rotation = Quaternion.Euler(currentClue.cloneRot.x,
                                                            currentClue.cloneRot.y,
                                                            currentClue.cloneRot.z);
            
            cloneClue.transform.localScale *= currentClue.cloneScale;
            
            CanvasClueObject.S.SetCloneClue(ref cloneClue);


            cloneClueCol = cloneClue.GetComponent<Collider>();
            cloneClueItem = cloneClue.GetComponent<ClueItem>();
            //cloneClueItem.isCollected = true;

            cloneClueCol.enabled = false;
            cloneClueItem.enabled = false;
        }
    }
    public void StoreCurrentClue()
    {
        if (currentClue != null)
        {
            ClueManager.S.AddClue(ref currentClue);
            UIController.S.AddToClueLog(currentClue.name);
        }
    }

    //
    public void ResetCurrentClue()
    {
        //currentClue.transform.position = cluePrevPos;
        //currentClue.transform.rotation = cluePrevRot;
        currentClue.isInspectable = false;
        if (cloneClue != null)
            Destroy(cloneClue);

        currentClue = null;
        cloneClue = null;
        cloneClueItem = null;

        //when we're not inspecting anything, we won't need the light
        if (inspectionLight != null)
            Destroy(inspectionLight);

        DoneDisplayingClue.Invoke();

    }

    GameObject CreateLight(Vector3 position)
    {
        GameObject lightObject = new GameObject("Inspection Light");
        Light lightComponent = lightObject.AddComponent<Light>();
        lightComponent.type = LightType.Spot;
        lightComponent.intensity = 15f;
        lightComponent.range = 5f;
        lightComponent.spotAngle = 50f;
        lightComponent.color = Color.white;
        lightObject.transform.position = position;
        lightObject.transform.rotation = Quaternion.Euler(35, 0, 0);

        return lightObject;
    }

    void HandleClueViewing(ref ClueItem currentClue)
    {
        Debug.Log("Displaying clue named " + currentClue.ItemName);
        // Return the ClueItem information stored in the Clue Item we just clicked on 
        // and log it to the console.
        print(currentClue.ToString()); // overrode its ToString method. 

        if (cloneClueItem != false)
            cloneClueItem.isInspectable = true;

        // Set up position to set up the light for inspecting the clueItem
        Vector3 clueLightLocation = new Vector3 (0,0,0);

        if (cloneClue != null)
        {
            clueLightLocation = cloneClue.transform.position + _lightOffset;

            // Create a light to view inspectable clueItem
            if (inspectionLight == null)
                inspectionLight = CreateLight(clueLightLocation);
        }

    }

    // helper methods
    //TODO This needs to work with mobile as well. Look into Unity's touch class to get an idea of how we can implement this with a touch screen.
    void HandleRotation(ref ClueItem currentClue)
    {

        // If the item is inspectable we handle input to rotate the object. 
        // W and S rotate it around the x-axis while A and D rotate around the y-axis
        if (currentClue.isInspectable)
        {
            // Rotate object based on the button pressed
            // TODO Objects not rotating as preferred.

            //			// The following handles code for inspecting (rotating) ClueItems on PC
            //            if (Input.GetKey(KeyCode.W))
            //            {
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.right, currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.W))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }
            //            else if (Input.GetKey(KeyCode.S))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.right, -currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.S))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }
            //            else if (Input.GetKey(KeyCode.A))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.up, currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.A))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }
            //            else if (Input.GetKey(KeyCode.D))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.up, -currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.D))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }

            //This is just to text touch drag functionality on Macs/PCs
            float x = 0;
            float y = 0;
			
            //if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            if (!touchControlled)
            {
                //VERY GHETTO, DOES NOT FULLY WORK
                if (Input.GetMouseButton(0))
                {
                    Vector3 pos = new Vector3 (0, 0, 0);
                    pos.x = cloneClueCol.bounds.center.x - Input.mousePosition.x;
                    pos.y = cloneClueCol.bounds.center.x - Input.mousePosition.y;

                    //print(pos.x);
                    //print(pos.y);

                    x = pos.x/30;
                    y = pos.y/30;
                }
            }

            //else
            if (touchControlled)
            {
                // The following handles code for inspecting (rotating) clues on Mobile
                Touch myTouch = Input.GetTouch(0);

                x = myTouch.deltaPosition.x;
                y = myTouch.deltaPosition.y;
            }

			if (Mathf.Abs(y) > Mathf.Abs(x))
			{
				// We are using the y variable in AngleAxis because moving our finger up and down would 
				// make the object rotate on it's x (right) axis

				cloneClue.transform.RotateAround (centerOfClone, Vector3.right, y * currentClue.rotateSpeed * Time.deltaTime);
//				rb.constraints = RigidbodyConstraints.FreezePosition;
//				rb.constraints = RigidbodyConstraints.FreezeRotationY;
//				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			}

			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
                // We are using the x variable in AngleAxis because moving our finger left and right would 
                // make the object rotate on it's y (world up) axis

                cloneClue.transform.RotateAround (centerOfClone, Vector3.up, x * currentClue.rotateSpeed * Time.deltaTime);
//				rb.constraints = RigidbodyConstraints.FreezePosition;
//				rb.constraints = RigidbodyConstraints.FreezeRotationX;
//				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			}
        }
    }
}
