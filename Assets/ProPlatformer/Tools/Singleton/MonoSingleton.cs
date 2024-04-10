using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : new()
{//TODO�����������Ҫ���ƣ��̳�MonoBehaviour�ĵ�����������ͨ����ʵ�ַ���������������Ϸʱδ����Ľű�ִ��awake�ᱨ�����米��
    private static object _lock = new object();

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
