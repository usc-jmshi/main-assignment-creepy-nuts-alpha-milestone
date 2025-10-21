using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public enum DeathCause
{
    DeathWall,
    FellOffPlatform,
    Abandoned,
    Quit
}

public class DeathAnalytics : MonoBehaviour
{
    public static DeathAnalytics Instance { get; private set; }
    
    private float survivalStartTime;
    private int colorSwitches;
    private int dashCount;
    private const string FORM_URL = "https://docs.google.com/forms/d/e/1FAIpQLScyYWkslH_p_cVRZpg3mebNviUg3KvmGGPonxCAAhElUuav4g/formResponse";
    
    private void Awake()
    {
        Instance = this;
        StartSurvivalTimer();
    }
    
    public void StartSurvivalTimer()
    {
        survivalStartTime = Time.time;
        colorSwitches = 0;
        dashCount = 0;
    }
    
    public void RecordColorSwitch()
    {
        colorSwitches++;
    }
    
    public void RecordDash()
    {
        dashCount++;
    }
    
    public void RecordDeath(DeathCause cause)
    {
        float survivalTime = Time.time - survivalStartTime;
        StartCoroutine(SendToGoogleForm(survivalTime, cause));
        StartSurvivalTimer();
    }
    
    private IEnumerator SendToGoogleForm(float timeSpent, DeathCause cause)
    {
        Debug.Log($"Sending to form: Time={timeSpent:F1}s, Cause={GetCauseString(cause)}, Colors={colorSwitches}, Dashes={dashCount}");
        
        WWWForm form = new WWWForm();
        
        form.AddField("entry.1616513856", "Death Event");
        form.AddField("entry.441659625", Mathf.RoundToInt(timeSpent).ToString());
        form.AddField("entry.1626598288", GetCauseString(cause));
        form.AddField("entry.694169683", colorSwitches.ToString());
        form.AddField("entry.969517712", dashCount.ToString());
        
        UnityWebRequest www = UnityWebRequest.Post(FORM_URL, form);
        yield return www.SendWebRequest();
        
        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log("Analytics failed: " + www.error);
        else
            Debug.Log("Analytics sent successfully!");
    }
    
    private string GetCauseString(DeathCause cause)
    {
        return cause switch
        {
            DeathCause.DeathWall => "Caught by Chasing Object",
            DeathCause.FellOffPlatform => "Block Landing Missed",
            DeathCause.Abandoned => "User Abandonment",
            DeathCause.Quit => "User Quit",
            _ => "Unknown"
        };
    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) RecordDeath(DeathCause.Abandoned);
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) RecordDeath(DeathCause.Abandoned);
    }
}