using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DebrisSpawnController : MonoBehaviour
{
    [SerializeField] private GameObject debris;
    [SerializeField] private GameObject debrisSpawnPoint;
    [SerializeField] private float waitForSeconds;
    public Coroutine debrisSpawnRef;
    private bool firstTimeCalled;

    // Start is called before the first frame update
    void Start()
    {
        firstTimeCalled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (debrisSpawnRef == null)
        {
            debrisSpawnRef = StartCoroutine(DebrisSpawnTimer());
        }
    }

    


    private void SpawnDebris()
    {
        
        Vector2 playerPause = debrisSpawnPoint.transform.position;
        Instantiate(debris, playerPause, Quaternion.identity);
        
    }

    public IEnumerator DebrisSpawnTimer()
    {

        if(!firstTimeCalled)
        {
            yield return new WaitForSeconds(waitForSeconds);
            firstTimeCalled = true;
        }
        else
        {
            SpawnDebris();
            yield return new WaitForSeconds(waitForSeconds);
        }
        
        debrisSpawnRef = null;
    }
}
