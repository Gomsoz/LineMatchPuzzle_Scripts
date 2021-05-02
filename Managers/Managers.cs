using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    #region Managers Property
    static Managers _instance;
    static InputManager _input = new InputManager();
    static ResourcesManager _resources = new ResourcesManager();
    static UIManager _ui = new UIManager();
    static SceneManagerEx _scene = new SceneManagerEx();
    static BoardManager _board = new BoardManager();
    static StageManager _stage = new StageManager();
    static JsonManager _json = new JsonManager();
    static SoundManager _sound = new SoundManager();

    public static Managers Manager { get { return _instance; } }
    public static InputManager Input { get { return _input; } }
    public static ResourcesManager Resources { get { return _resources; } }
    public static UIManager UI { get { return _ui; } }
    public static SceneManagerEx Scene { get { return _scene; } }
    public static BoardManager Board { get { return _board; } }
    public static StageManager Stage { get { return _stage; } }
    public static JsonManager Json { get { return _json; } }
    public static SoundManager Sound { get { return _sound; } }
    #endregion

    private void Awake()
    {
        AwakeInit();
    }

    private void Start()
    {
        StartInit();
    }

    private void Update()
    {
        Input.InputUpdate();
    }

    void AwakeInit()
    {
        Singleton();
        OtherManagersAwakeInit();      
    }

    void StartInit()
    {
        OtherMAnagersStartInit();
    }

    void Singleton()
    {
        if (_instance == null)
        {
            GameObject _MgrOb = GameObject.Find("@Managers");
            if (_MgrOb == null)
            {
                _MgrOb = new GameObject { name = "@Managers" };
                _MgrOb.AddComponent<Managers>();
            }
            _instance = _MgrOb.GetComponent<Managers>();
            DontDestroyOnLoad(_MgrOb);
        }
    }

    void OtherManagersAwakeInit()
    {
        UI.UIMnangerInit();
    }

    void OtherMAnagersStartInit()
    {
        Stage.StageMgrInit();
    }
}
