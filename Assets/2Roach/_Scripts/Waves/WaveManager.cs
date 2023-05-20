using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] List<Wave> _waves;
    [SerializeField] List<Roach> _roaches;
    private IEnumerator<Wave> _waveIterator;
    private IEnumerator<RoachOrder> _roachOrderIterator;
    private float _minTimeBtwOrders = .2f;
    private Wave _currentWave;

    public Wave CurrentWave { get => _currentWave; }
    public List<Roach> Roaches { get => _roaches;  }

    public void StartFirstWave()
    {
        _waveIterator = _waves.GetEnumerator();
        StartNextWave();
    }

    [ContextMenu("Start Next Wave")]
    private void StartNextWave()
    {
        StopAllCoroutines();

        if (_waveIterator.MoveNext())
        {
            _currentWave = _waveIterator.Current;
        }
        else
        {
            Debug.Log("No more waves Available.");
           // GameManager.Instance.Win(); TODO
            return;
        }

        if(_currentWave)
            StartCoroutine(COR_Wave(_currentWave));
    }


    private IEnumerator COR_Wave(Wave waveData)
    {
        yield return Yielders.Get(waveData.RestDurationBeforeWave);
        //GameManager.Instance.ChangeGamePhase(GameManager.GamePhase.Wave); TODO

        Debug.Log("Wave Started!" );

        while (_roachOrderIterator.MoveNext())
        {
            yield return Yielders.Get(_minTimeBtwOrders);
            var roach = GetRoachByID(_roachOrderIterator.Current.RoachId);

            roach.gameObject.SetActive(true);
            roach.InitOrder(_roachOrderIterator.Current.Order);
        }
    
        yield return new WaitUntil(()=> AllRoachesCompletedOrFailed());
        
        Debug.Log("Wave Completed!");
        //GameManager.Instance.WavesConquered++; TODO
        StartNextWave();
    }

    /// <summary>
    /// THIS FOREACH IS TROUBLE
    /// </summary>
    /// <returns></returns>
    public bool AllRoachesCompletedOrFailed()
    {
        foreach (var roach in _roaches)
        {
            if(roach.State != RoachState.Completed || roach.State != RoachState.Failed)
                return false;
        }

        return true;
    }

    public Roach GetRoachByID(RoachID id)
    {
        foreach (var roach in _roaches)
        {
            if(roach.Id == id) return roach;
        }
        Debug.LogError("No ROACH FOUND!");
        return null;
    }

}
