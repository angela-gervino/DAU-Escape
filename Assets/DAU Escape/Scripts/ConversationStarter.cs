using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField]
    private NPCConversation myConversation;

    private void OnTriggerEnter(Collider other)
    {
        ConversationManager.Instance.StartConversation(myConversation);

        Destroy(gameObject);
    }
}
