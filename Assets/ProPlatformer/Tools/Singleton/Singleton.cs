using System;
public class Singleton<T> where T : new()
{
    //�߳���
    private static object _lock = new object();

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
                        Init();
                    }
                }
            }
            return _instance;
        }
    }

    private static void Init()
    {
        
    }
}
