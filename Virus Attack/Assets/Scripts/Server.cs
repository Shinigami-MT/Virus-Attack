using UnityEngine;

public class Server : MonoBehaviour
{
    private int _health = 10;
    public int Health { get { return _health; } 
        set
        {
            _health = value;
            if(value <= 0)
            {
                UIManager.Instance.OnGameOver();                
            }
            UIManager.Instance.UpdateHealth(value);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent<DataObject>(out DataObject data);
        if(data)
        {
            if (!data.isGood && GameManager.Instance.serverIsOnline) Health--;
            
            data.DestroyMe();
        }

    }
}
