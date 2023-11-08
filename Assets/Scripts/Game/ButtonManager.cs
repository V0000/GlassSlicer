using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ButtonManager : MonoBehaviour
    {
        public static event Action NextLevelPressed;
    
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
            {
                HandleBackButtonPress();
            }
        }
    
        public void GoToLevel()
        {
            SceneManager.LoadScene("Level");
        }
        public void GoToMap()
        {
            SceneManager.LoadScene("Map");
        }
        public void GoExit()
        {
            Application.Quit();
        }
        // Удаляет сохраненный локально файл с данными игры
        public void DropProgress()
        {
            DataLoaderSaver dataLoaderSaver = new DataLoaderSaver();
            dataLoaderSaver.DeletePersistentDataFile();
        }
    
        public void ReloadLevel()
        {
            SceneManager.LoadScene("Level");
        
        }
        public void GoMenu()
        {
            SceneManager.LoadScene("Menu");
        
        }
    
        public void GoMarket()
        {
            SceneManager.LoadScene("Market");
        
        }
    
        public void NextLevel()
        {
            NextLevelPressed?.Invoke();
            SceneManager.LoadScene("Level");
            Debug.Log("Go Next level");
        
        }

        //обработка нажатия кнопки "Назад" на девайсе
        void HandleBackButtonPress()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName == "Menu")
            {
                GoExit();
            }
            if (currentSceneName == "Level" || currentSceneName == "Map")
            {
                GoMenu();
            }
            if (currentSceneName == "Market")
            {
                GoToLevel();
            }

        }

    }
}
