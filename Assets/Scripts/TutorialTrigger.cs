using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Tutorial tutorial;

    public void TriggerDialogue()
    {
        transform.GetChild(3).gameObject.GetComponent<TutorialManager>().StartDialogue(tutorial);
    }
}