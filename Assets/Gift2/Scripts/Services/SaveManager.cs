using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

namespace Gift2
{
    public static class SaveManager
    {
        private static string SavePath => Application.persistentDataPath;

        private static string GetFullPath(string fileName) => Path.Combine(SavePath, fileName);

        public static bool Save<T>(string fileName, T data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, prettyPrint: true);
                string path = GetFullPath(fileName);

                File.WriteAllText(path, json, Encoding.UTF8);
                Debug.Log($"Данные сохранены в {path}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка сохранения в файл {fileName}: {e.Message}");
                return false;
            }
        }

        public static bool Load<T>(string fileName, out T data)
        {
            data = default(T);
            string path = GetFullPath(fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"Файл {fileName} не найден по пути {path}");
                return false;
            }

            try
            {
                string json = File.ReadAllText(path, Encoding.UTF8);

                data = JsonUtility.FromJson<T>(json);
                Debug.Log($"Данные загружены из {path}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки из файла {fileName}: {e.Message}");
                return false;
            }
        }

        public static IEnumerator SaveAsync<T>(string fileName, T data)
        {
            bool done = false;
            System.Threading.Tasks.Task.Run(() =>
            {
                done = Save(fileName, data);
            });

            while (!done)
                yield return null;
        }

        public static IEnumerator LoadAsync<T>(string fileName)
        {
            T result = default(T);
            bool done = false;
            System.Threading.Tasks.Task.Run(() =>
            {
                Load(fileName, out result);
                done = true;
            });

            while (!done)
                yield return null;

            yield return result;
        }
        
        public static void Clear(string fileName)
        {
            try
            {
                string path = GetFullPath(fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    Debug.Log($"Файл {fileName} удалён");
                }
                else
                {
                    Debug.LogWarning($"Файл {fileName} не найден, удаление не требуется");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка удаления файла {fileName}: {e.Message}");
            }
        }
    }
}
