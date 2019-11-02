using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to create gems in the game and  gems are used to 
/// duplicate a part of the scene and their movements
/// </para>
/// Updated: 2/4/2019<para/>
/// Author: Yan Xiao, Fei Xiao<para/>
/// Attached object: Player <para/>
/// </summary>
public class placeGem : MonoBehaviour
{

    public GameObject GemPrefab;
    public Camera playerCamera;

    [System.Serializable]
    public class keys_for_placeGem
    {
        public KeyCode placeGemKey;
        public KeyCode putGemKey;
        public KeyCode cancelKey;
        public KeyCode largerKey;
        public KeyCode smallerKey;
        // public KeyCode          rotationKey;
    }

    public keys_for_placeGem allKeys;

    public float spawnDistance;
    public int maxGem;
    public float scaleSpeed;
    public float fartherSpeed;

    //variables used for debugging
    [Header("Debug")]
    public float minimunDistance;
    public float maximum_scale;
    public GameObject temp;
    public List<GameObject> createdGem;
    player_status status_script;
    lockSpace gem_status;
    public float fartherOrCloserFactor = 1;
    public float rotationYOffest = 0;


    void Start()
    {
        status_script = this.GetComponent<player_status>();
    }

    void Update()
    {
        maximum_scale = Mathf.Max(1, fartherOrCloserFactor * 5 * 3);

        // if the gem is currently activating 
        if (temp != null) {
            minimunDistance = Mathf.Max(0.2f, temp.transform.localScale.x / 5 / 3);
            AdjustPosition(temp);
            if (Input.GetKeyDown(allKeys.cancelKey)) {
                Destroy(temp);
                temp = null;
                while (status_script.Hands_free(Hands.gem) == false);
                rotationYOffest = 0;
                fartherOrCloserFactor = 1;
            } else if (Input.GetKeyDown(allKeys.putGemKey)) {
                while (status_script.Hands_free(Hands.gem) == false);
                int status = gem_status.Unpick();
                if (status == 0) {
                    createdGem.Add(temp);
                } else if (status == 1) {
                    Destroy(temp);
                }
                temp = null;
                rotationYOffest = 0;
                fartherOrCloserFactor = 1;
                
            }
        }


        // This part of code detects if user can put a gem and initiate the gem if
        // user pressed the place gem key
        else if (Input.GetKeyDown(allKeys.placeGemKey) && createdGem.Count < maxGem) {
            if (status_script.Hands_change(Hands.gem)){
                temp = Instantiate(GemPrefab);
                gem_status = temp.GetComponent<lockSpace>();
                temp.transform.position = CalculatePosition();
            }
        }
    }

    /// <summary>
    /// This private helper method is used to calculate the position of the holding gem
    /// </summary>
    /// <returns>the absolute position of the gem in space</returns>
    private Vector3 CalculatePosition()
    {
        Vector3 playerPos = this.transform.position + new Vector3(0, 4, 0);
        Vector3 playerDirection = playerCamera.transform.forward;
        Vector3 spawnPos = playerPos - new Vector3(0, 3, 0) + playerDirection * spawnDistance * fartherOrCloserFactor;
        return spawnPos;
    }

    /// <summary>
    /// This private helper method adjust the size, position, rotation of the gem when user is 
    /// holding that gem
    /// </summary>
    private void AdjustPosition(GameObject gem)
    {
        //getting larger
        if (Input.GetKey(allKeys.largerKey)) {
            if (gem.transform.localScale.x < maximum_scale)
                gem.transform.localScale += (new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime));
        }
        //move closer or farther
        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            fartherOrCloserFactor = Mathf.Max(fartherOrCloserFactor + fartherSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime, minimunDistance);
        }
        
        if (Input.GetKey(allKeys.smallerKey)) {
            if (gem.transform.localScale.x > 0.5)
                gem.transform.localScale -= (new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime));
        }

        temp.transform.position = CalculatePosition();
        
    }
}
