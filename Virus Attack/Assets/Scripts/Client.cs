using System.Collections;
using UnityEngine;
[DefaultExecutionOrder(1)]

public class Client : MonoBehaviour
{
    [SerializeField] LineRenderer m_lineRenderer;

    private Vector3[] path = new Vector3[3];
    private Coroutine spawner;
    private void OnEnable()
    {
        GameManager.Instance.onServerOnline += ToggleSpawn;
        SetPath();
    }

    
    private void OnDisable()
    {
        GameManager.Instance.onServerOnline -= ToggleSpawn;
    }
    private void SetPath()
    {
        path[0] = Vector3.zero;
        path[1] = Vector3.up * (GameManager.Server.transform.position.y - this.transform.position.y);
        path[2] = GameManager.Server.transform.position - this.transform.position;
        m_lineRenderer.SetPositions(path);
    }

    #region spawn control

    private void ToggleSpawn()
    {
        if(spawner != null)
        {
            StopCoroutine(spawner);
        }
        spawner = StartCoroutine(SpawnDataCO());
    }

    private IEnumerator SpawnDataCO()
    {
        while(GameManager.Instance.serverIsOnline)
        {
            Debug.Log("Server is online " + GameManager.Instance.serverIsOnline);
            float RandomDelay = Random.Range(2.0f, 5.0f);
            Spawn();
            yield return new WaitForSeconds(RandomDelay);
        }
    }

    private void Spawn()
    {
        if(GameManager.DataPool)
        {
             DataObject current =  GameManager.DataPool.Spawn(this.transform).GetComponent<DataObject>();
            //DataObject current = Instantiate(DataPrefab, transform.position, Quaternion.identity, this.transform); 
            current.StartJourney(path);
        }
        else
        {
            Debug.LogError("no datapool found");
        }
    }
    #endregion
}