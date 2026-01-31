using UnityEngine;

/// <summary>
/// Unity 씬에 존재하는 컴포넌트형 싱글톤
/// 예: MonoSingleton<GameManager>.Instance
/// </summary>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool isQuitting = false;
    private static readonly object lockObj = new object();

    public static T Instance
    {
        get
        {
            if (isQuitting)
            {
                Debug.LogWarning($"[MonoSingleton] {typeof(T)} 인스턴스는 앱 종료 중입니다.");
                return null;
            }

            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = Object.FindFirstObjectByType<T>();

                    if (instance == null)
                    {
                        GameObject obj = new GameObject($"(MonoSingleton) {typeof(T)}");
                        instance = obj.AddComponent<T>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return instance;
            }
        }
    }

    public static bool HasInstance => instance != null;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
