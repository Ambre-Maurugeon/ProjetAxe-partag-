using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ArbreVie : MonoBehaviour
{
    private bool inTrigger=false;

//Saturation
    private ColorAdjustments _colorAdjustments;
    private GameObject _globVolume;
    private VolumeProfile _volProfile;

    // Start is called before the first frame update
    void Start()
    { 
        _globVolume = GameObject.FindGameObjectWithTag("GlobalVolume");
        _volProfile = _globVolume.GetComponent<Volume>().profile;
    }

    // Update is called once per frame
    void Update()
    {
        if(inTrigger){
            StartCoroutine(Heal());
        }
    }

    IEnumerator Heal(){

        if(_volProfile.TryGet(out _colorAdjustments)){ 
            if (_colorAdjustments.saturation.value<0){
                _colorAdjustments.saturation.value+=0.8f; // changer vitesse augmentation saturation
            }
        }

        if (Life.ActualHealth<Life.InitialHealth){
            Life.ActualHealth +=1;

            yield return new WaitForSeconds(0.15f);
        }
    }

    

    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=true;
        }
    }

    void OnTriggerExit2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=false;
        }
    }
}
