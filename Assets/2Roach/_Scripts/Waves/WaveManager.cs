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
    private int _waveNumber = 0;

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
        _waveNumber++;
        ProcWaveStartActions(_waveNumber);
        StopAllCoroutines();

        if (_waveIterator.MoveNext())
        {
            _currentWave = _waveIterator.Current;
        }
        else
        {
            Debug.Log("No more waves Available.");
            GameManager.instance.Win();
            return;
        }

        if(_currentWave)
            StartCoroutine(COR_Wave(_currentWave));
    }

    private void ProcWaveStartActions(int waveNumber)
    {
        if(waveNumber != 4)
            UIManager.instance.DisplayAlertMsg("Wave: " + waveNumber + "/" + _waves.Count, 2.5f);
        if(waveNumber == 4)
            UIManager.instance.DisplayAlertMsg("Tip: use Plates to hold food", 2.5f);
    }


    private IEnumerator COR_Wave(Wave waveData)
    {
        yield return Yielders.Get(waveData.RestDurationBeforeWave);
        _roachOrderIterator = waveData.RoachOrders.GetEnumerator();

        Debug.Log("Wave Started!" );

        while (_roachOrderIterator.MoveNext())
        {
            yield return Yielders.Get(_minTimeBtwOrders);
            var roach = GetRoachByID(_roachOrderIterator.Current.RoachId);

            roach.gameObject.SetActive(true);
            roach.InitOrder(_roachOrderIterator.Current.Order);
        }
        Debug.Log("Wave Started!" );
        yield return StartCoroutine(COR_WaitForRoaches());
        
        Debug.Log("Wave Completed!");
        //GameManager.Instance.WavesConquered++; TODO
        StartNextWave();
    }
    
    private IEnumerator COR_WaitForRoaches()
    {
        while(AllRoachesCompletedOrFailed() == false)
        {
            //Debug.Log("Check!");
            yield return Yielders.Get(1.5f);
        }
    }

    public bool AllRoachesCompletedOrFailed()
    {
        foreach (var roach in _roaches)
        {
            if(roach.State != RoachState.Dormant)
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
