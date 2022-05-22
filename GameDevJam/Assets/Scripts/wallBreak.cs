using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallBreak : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject crackImage;
    [SerializeField] GameObject crackImage2;
    public void breakWall()
    {
        this.gameObject.SetActive(false);
        crackImage.SetActive(false);
        crackImage2.SetActive(false);
    }
}
