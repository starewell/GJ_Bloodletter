using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEditor;
using UnityEngine;

public class TransfusionSite : HoldInteractable {

    
    public float infectionHeal, infectionDilution;    

    public override void Interact() {
        base.Interact();
        StartCoroutine(TransfuseBlood(bloodletter));
    }

    public IEnumerator TransfuseBlood(Bloodletter bloodletter) {
        interacting = true;
        DebugUI.instance.StartCoroutine(DebugUI.instance.DisplayHoldInteract(this));
        float timer = 0;
        while (Input.GetMouseButton(0) && interacting && inRange &&
        content > 0  && bloodletter.bloodLevel < 100) {
            while (!bloodletter.tick) {
                yield return null;
                if (!Input.GetMouseButton(0)) {
                    interacting = false;
                    break;
                }
                if (timer >= loopDelay && !audioSource.isPlaying) {
                    audioSource.loop = true;
                    audioSource.clip = loopSFX.Get();
                    audioSource.Play();
                } else timer += Time.deltaTime;
            }
            if (!Input.GetMouseButton(0)) {
                    interacting = false;
                    break;
            }
            
            if (bloodletter.bloodLevel + consumptionRate < 100)
                bloodletter.bloodLevel += consumptionRate;
            else bloodletter.bloodLevel = 100;
            if (bloodletter.infectionPotency - infectionDilution > bloodletter.potencyRange.x)
                bloodletter.infectionPotency -= infectionDilution;
            else bloodletter.infectionPotency = 0;
            if (bloodletter.infectionLevel - infectionHeal > 0)
                bloodletter.infectionLevel -= infectionHeal;
            else bloodletter.infectionLevel = 0;
            content -= consumptionRate;
            if (!inRange) {
                interacting = false;
                break;
            }
            yield return null;
        }
        if (audioSource.loop == true) {
            audioSource.loop = false;
            audioSource.Stop();
        }
// USED ALL BLOOD
        if (content <= 0) {
            ExhaustSite();
        } 
        interacting = false;    
    }



}
