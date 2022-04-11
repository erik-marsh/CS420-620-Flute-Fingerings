using UnityEngine;
//namespace Hi5_Interaction_Core
//{
    // Make a subclass of this class with T as the subclass to make a singleton
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        public static bool DoesExist()
        {
            return instance != null;
        }

        // Makes this object a persistent singleton unless the singleton already exists in which case
        // this object is destroyed
        protected void DontDestroy()
        {
            if (this == Instance)
            {
                MonoBehaviour.DontDestroyOnLoad(Instance.gameObject);
            }
            else
            {
                MonoBehaviour.Destroy(this);
            }
        }
    }
//}