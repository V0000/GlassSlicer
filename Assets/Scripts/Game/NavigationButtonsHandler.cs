using System;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Game
{
    public class NavigationButtonsHandler : MonoBehaviour
    {
        public static event Action OnNextLevelPressed;

        private DataLoaderSaver _dataLoaderSaver;
        
        [Inject]
        private void Initialize(DataLoaderSaver dataLoaderSaver)
        {
            _dataLoaderSaver = dataLoaderSaver;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
            {
                HandleBackButtonPress();
            }
        }
        
        #region Public methods

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
            _dataLoaderSaver.DeletePersistentDataFile();
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
            OnNextLevelPressed?.Invoke();
            SceneManager.LoadScene("Level");
            Debug.Log("Go Next level");
        
        }
        
        #endregion
        
        //обработка нажатия кнопки "Назад" на девайсе
        private void HandleBackButtonPress()
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
