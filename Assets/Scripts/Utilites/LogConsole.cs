using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogConsole : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logTextMeshPro;
    [SerializeField] private GameObject logPanel;
    
    private bool isPanelVisible = false;
    void Awake()
    {
        logTextMeshPro.text = "";

        //событие логирования
        Application.logMessageReceived += HandleLog;
        logPanel.SetActive(isPanelVisible);
        Debug.Log("LogConsole is started");
    }

    void HandleLog(string logText, string stackTrace, LogType type)
    {
        logTextMeshPro.text += logText + "\n";
    }
    
    public void ToggleLogPanel()
    {
        isPanelVisible = !isPanelVisible;
        logPanel.SetActive(isPanelVisible);
    }
}