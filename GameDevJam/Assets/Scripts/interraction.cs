using UnityEngine.Events;
using UnityEngine;

public class interraction : MonoBehaviour
{
    public GameObject textPlace;
    [SerializeField] UnityEvent OnInterraction;
    bool interract = false;
    bool interracted = false;
    public bool shouldShow = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerTracker")
        {
            if (interracted == false)
            {
                textPlace.SetActive(true);
                interract = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerTracker")
        {
            if (interracted == false)
            {
                textPlace.SetActive(false);
                interract = false;
            }
        }
    }

    private void Update()
    {
        if(interract == true && shouldShow)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInterraction.Invoke();
                interracted = true;
                interract = false;
                textPlace.SetActive(false);
            }
        }
    }

    public void shouldShowNow()
    {
        shouldShow = true;
    }

}
