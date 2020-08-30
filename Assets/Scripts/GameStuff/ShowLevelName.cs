using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLevelName : MonoBehaviour
{

    
    public string placeName;
    public GameObject text;
    public Text placeText;
    private bool alreadyTexted;
    // Start is called before the first frame update
    void Start()
    {
        alreadyTexted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf && alreadyTexted == false)
        {
            StartCoroutine(placeNameCo());
        }
    }
    private IEnumerator placeNameCo()
    {
        text.SetActive(true);
        placeText.text = placeName;
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
        alreadyTexted = true;
    }
}
