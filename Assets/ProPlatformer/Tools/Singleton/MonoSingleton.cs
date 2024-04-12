using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : new()
{//TODO：这个单例还要完善，继承MonoBehaviour的单例不能用普通单例实现方法，否则启动游戏时未激活的脚本执行awake会报错，比如背包
    private static object _lock = new();

    public string meaning = string.Empty;

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                        
                    }
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
        Debug.Log($"<color=Color.yellow>{gameObject.name}</color> \n Meaning : {meaning}");
    }

    public void Print(string str)
    {
        Debug.Log($"<color=Color.yellow>{gameObject.name}</color> \n Meaning : {str}");
    }
}
