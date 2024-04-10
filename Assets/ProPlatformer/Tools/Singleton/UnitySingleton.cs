using UnityEngine;

//单例模板工具类，管理类都用一下，要求必须是Component或其子类
public class UnitySingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;//FindObjectOfType方法返回的是Object类型
                //_instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    //obj.hideFlags=HideFlags.HideAndDontSave;//隐藏实例化的obj，其他代码就扫不到它。一般没必要
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }




}
