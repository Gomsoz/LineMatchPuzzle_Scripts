using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager GameMgr { get { return _instance; } }

    public Action<int> TimeEvent = null;
    public Action ZeroTimeEvent = null;
    public Action<int> MatchingEvent = null;

    int m_leftTime = 0;

    private static bool paused = false;

    public static bool Paused
    {
        get { return paused; }
        set
        { 
            paused = value;
            Time.timeScale = value ? 0 : 1;
        }
    }

    private void Awake()
    {
        GameManagerInit();
    }

    void GameManagerInit()
    {
        Singleton();
    }

    void Singleton()
    {
        if (_instance == null)
        {
            GameObject _MgrOb = GameObject.Find("@GameManager");
            if (_MgrOb == null)
            {
                _MgrOb = new GameObject { name = "@GameManager" };
                _MgrOb.AddComponent<Managers>();
            }
            _instance = _MgrOb.GetComponent<GameManager>();
            DontDestroyOnLoad(_MgrOb);
        }
    }

    public void GamePause()
    {
        Paused = !Paused;
        if (Paused)
        {
            Managers.Sound.Pause(Defines.SoundType.Bgm);
            return;
        }
        Managers.Sound.UnPause(Defines.SoundType.Bgm);
    }

    public void SetTiemer(int time)
    {
        m_leftTime = time;
        StartCoroutine(OnTimer());
    }

    IEnumerator OnTimer()
    {
        while(m_leftTime > 0)
        {
            if (StageManager._isPlayingPuzzle == false)
                m_leftTime = 0;
            yield return new WaitForSeconds(1f);
            m_leftTime--;
            TimeEvent.Invoke(m_leftTime);
        }
        
        if(StageManager._isPlayingPuzzle == true)
            ZeroTimeEvent.Invoke();
    }
}
