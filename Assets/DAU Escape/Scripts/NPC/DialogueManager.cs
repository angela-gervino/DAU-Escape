using UnityEngine;
using DialogueEditor;

namespace DAUEscape
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private NPCConversation myConversation;
        private float detectionRadius = 3.0f;
        private bool inProgress = false; // is the dialogue currently on the screen?

        void Update()
        {
            inProgress = ConversationManager.Instance.IsConversationActive;
            PlayerController.UpdateDialogueStatus(inProgress);

            if (!inProgress)
            {
                Vector3 toPlayer = PlayerController.Instance.transform.position - transform.position;
                toPlayer.y = 0;

                if (toPlayer.magnitude <= detectionRadius && Input.GetKeyDown(KeyCode.X))
                {
                    StartDialogue();
                }
            }
        }// Update


        void StartDialogue()
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }// StartDialogue
    }
}

