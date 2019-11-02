using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static publicMethods.PublicMethods;

public class energyDetector : MonoBehaviour
{
    
    public GameObject energyField;
    public List<GameObject> list;
    TMPro.TextMeshProUGUI energyText;
    public KeyCode hold;
    bool check = false;
    float energyLevel = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        energyText = energyField.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        energyLevel = 0;
        foreach(GameObject item in list){
            float num = Vector3.Distance(this.transform.position, item.transform.position);
            energyLevel += 100/Mathf.Sqrt(num);
        }
        // if(energyLevel >= 1.0f){
        //     energyLevel = 1.0f;
        // }
        int temp = (int)energyLevel;
        energyLevel = Mathf.Round(energyLevel * 10.0f) / 10.0f;
        string text = energyLevel.ToString();
        if (Input.GetKeyDown(hold)){
            check = true;
        }
        if (Input.GetKeyUp(hold)){
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
