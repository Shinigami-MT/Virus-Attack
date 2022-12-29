using System.Collections;
using UnityEngine;

public class DataObject : MonoBehaviour
{
    public static int ChanceForRed = 33;

    public bool isGood { get; set; }
    private SpriteRenderer m_sprite;
    private Coroutine movement;

    [SerializeField] float Speed = 1;
    [SerializeField] float SpeedForGreen = 1;
    [SerializeField] float SpeedForRed = 2;
    private void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        GetComponent<Collider2D>().enabled = true;
        transform.localScale = Vector3.one;
        int chanceForGreen = Random.Range(0, 100);
        isGood = chanceForGreen > ChanceForRed;
        if (isGood)
        {
            m_sprite.color = Color.green;
            m_sprite.sortingOrder = 1;
            Speed = SpeedForGreen;
        }
        else
        {
            m_sprite.color = Color.red;
            m_sprite.sortingOrder = 2;
            Speed = SpeedForRed;
        }
        /*EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventdata) => onClick());
        trigger.triggers.Add(entry);*/
    }

    public void onClick()
    {
        if(!isGood)
        {
            if (movement != null)
            {
                StopCoroutine(movement);
            }
            GameManager.Instance.Score+=2;
            //StartCoroutine(DestroyMe());
            DestroyMe();
        }
        else
        {
            if(GameManager.Instance.FirewallMode)
            {
                if (movement != null)
                {
                    StopCoroutine(movement);
                }
                GameManager.Server.Health--;
                //StartCoroutine(DestroyMe());
                DestroyMe();
            }
        }
    }

    public void DestroyMe()
    {
        GetComponent<Collider2D>().enabled = false;
        LeanTween.scale(this.gameObject, Vector3.zero, 0.5f).setEaseInSine().setOnComplete(() => GameManager.DataPool.Despawn(this.gameObject));
        /*while(transform.localScale.x > 0.1f)
        {
            transform.localScale = transform.localScale * Mathf.Lerp(1, 0, Time.deltaTime *3);
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);*/
    }

    private void OnDisable()
    {
        if(movement != null)
        {
            StopCoroutine(movement);
        }
    }


    public void StartJourney(Vector3[] path)
    {
        movement = StartCoroutine(StartJourneyCO(path));
    }
    private IEnumerator StartJourneyCO(Vector3[] path)
    {
        for (int i = 0; i < path.Length; i++)
        {
            while(transform.localPosition != path[i])
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, path[i],  Speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}

