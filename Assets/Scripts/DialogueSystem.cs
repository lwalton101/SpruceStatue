using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    private Queue<string> currentDialogue;
    [SerializeField] private KeyBindingsManager keyBindingsManager;
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private TextMeshProUGUI npcNameText;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI messageText;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerCollider == null)
        {
            playerCollider = NetworkManager.Singleton.GetPlayerInfo().playerObject.GetComponent<Collider2D>();
        }
        
        GameObject isCollidingWithTalk = IsCollidingWithTalk(playerCollider);
        if (Input.GetKeyDown(keyBindingsManager.talk) && isCollidingWithTalk != null)
        {
            currentDialogue = new Queue<string>();
            DialogueEntity dialogueEntity = isCollidingWithTalk.GetComponent<DialogueEntity>();

            StartDialogue(dialogueEntity);
        }
    }

    private GameObject IsCollidingWithTalk(Collider2D coll)
    {
        Collider2D[] colliders = new Collider2D[5];
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        Physics2D.OverlapCollider(coll, filter, colliders);
        foreach (Collider2D collider2D in colliders)
        {
            if (collider2D == null) continue;

            if (collider2D.gameObject.CompareTag("CanTalkTo"))
            {
                return collider2D.gameObject;
            }
        }
        return null;
    }

    public void StartDialogue(DialogueEntity dialogueEntity)
	{
        dialogueBox.SetActive(true);
        npcNameText.text = dialogueEntity.npcName;
        currentDialogue.Clear();

        foreach(string line in dialogueEntity.speech)
		{
            currentDialogue.Enqueue(line);
		}

        DisplayNextLine();
	}

    public void DisplayNextLine()
    {
        if (currentDialogue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = currentDialogue.Dequeue();
        StopAllCoroutines();
        messageText.text = sentence;
    }

	private void EndDialogue()
	{
        dialogueBox.SetActive(false);
	}
}
