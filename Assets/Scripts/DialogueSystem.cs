using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{

    [SerializeField] private KeyBindingsManager keyBindingsManager;
    [SerializeField] private Collider2D playerCollider;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject isCollidingWithTalk = IsCollidingWithTalk(playerCollider);
        if (Input.GetKeyDown(keyBindingsManager.talk) && isCollidingWithTalk != null)
        { 
            foreach(string line in isCollidingWithTalk.GetComponent<DialogueEntity>().speech)
			{
                Debug.Log(line);
			}
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
}
