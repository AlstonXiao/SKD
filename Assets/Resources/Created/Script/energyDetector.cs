using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

/// <summary>
/// <para>
/// This class is used to calculate the energy strenghth and guide the player to the areas where he can explore
/// </para>
/// Author: Yuzhe Gu<para/>
/// Attached object: Player<para/>
/// Needed object: Several gameObjects to calculate the strenght<para/>
/// </summary>
public class energyDetector : MonoBehaviour
{
    
    public GameObject energyField; // where to display the strenghth
    public List<GameObject> list; // list of objects that emit the energy
    private TMPro.TextMeshProUGUI energyText;
    public KeyCode checkEnergyKey; // user need to press which button to see the energy
    private bool check = false;
    private float energyLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        energyText = energyField.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        energyLevel = 0;

        // the energy strength is 1/X^2 propotion to the distance
        foreach(GameObject item in list){
            float num = Vector3.Distance(this.transform.position, item.transform.position);
            energyLevel += 100/Mathf.Sqrt(num);
        }
        // if(energyLevel >= 1.0f){
        //     energyLevel = 1.0f;
        // }

        // rounding 
        int temp = (int)energyLevel;
        energyLevel = Mathf.Round(energyLevel * 10.0f) / 10.0f;
        string text = energyLevel.ToString();
        if (Input.GetKeyDown(checkEnergyKey)){
            check = true;
        }
        if (Input.GetKeyUp(checkEnergyKey)){
            check = false;
        }
        if(check){
            energyText.text = text;
        }
        else{
            energyText.text = "";
        }
        

        
    }
}
