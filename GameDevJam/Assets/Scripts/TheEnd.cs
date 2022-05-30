using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class TheEnd : MonoBehaviour
{
    public TextMeshProUGUI endText;
    public TextMeshProUGUI resultText;
    public PlayerTracker thePlayerTracker;
    public Animator endAnimator;
    public Persistent forVolume;
    string type;
    float resetVolume;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerTracker")
        {
            toTheEnd(false);
        }
    }
    public void toTheEnd(bool lost)
    {
        resetVolume = forVolume.volume;
        forVolume.changeVolume(0.00001f);
        
        if(thePlayerTracker.playerChar.unitType == 1)
        {
            type = "human";
        }
        else if (thePlayerTracker.playerChar.unitType == 2)
        {
            type = "goblin";
        }
        else if (thePlayerTracker.playerChar.unitType == 3)
        {
            type = "big foot";
        }
        else if (thePlayerTracker.playerChar.unitType == 4)
        {
            type = "bat";
        }

        if (lost == true)
        {
            resultText.text = "You Lose";
            endText.text = "You lost your sanity. You have lived through many traumas, many deaths within a short period of time. Although your curse kept you alive, you lost your mind and become a monster. You haunted the prison for an eternity before finding the peace of true death.";
        }
        else
        {
            resultText.text = "You Win";
            endText.text = "You have managed to escape the prison as a " + type + ". The world you faced on the other side of the wall was full of familiar madness and temporarily death bodies. It was a nightmare designed with necromancy by 13 damned souls and you were one of them. A world with constant resurrection and cruel enemies… You laughed at your good fortune and continued your adventure.";
        }
        endAnimator.SetTrigger("ToTheEnd");
    }

    public void ResetVoulme()
    {
        forVolume.changeVolume(resetVolume);
    }
}
