using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Globals : MonoBehaviour
{
    public const string NUMBER_FORMAT = "+0.00;-0.00;0.00";

    [SerializeField] private static TMP_Text txtBlueTeam;
    [SerializeField] private static TMP_Text txtRedTeam;
    // Start is called before the first frame update
    public static float winsBlueTeam = 0;
    public static float loosesBlueTeam = 0;
    public static float winsRedTeam = 0;
    public static float loosesRedTeam = 0;
    public static float generation = 0;

    

    public static void PrintStats()
    {
        txtBlueTeam = GameObject.Find("TextBlueTeam").GetComponent<TMP_Text>();
        float bluePercent = (winsBlueTeam / loosesBlueTeam) * 100;
        txtBlueTeam.text = string.Format("Generacion={0}, Success={1}, Fail={2}, %{3}", generation, winsBlueTeam, loosesBlueTeam, bluePercent.ToString("0"));

        txtRedTeam = GameObject.Find("TextRedTeam").GetComponent<TMP_Text>();

        float redPercent = (winsRedTeam / loosesRedTeam) * 100;
        txtRedTeam.text = string.Format("Generacion={0}, Success={1}, Fail={2}, %{3}", generation, winsRedTeam, loosesRedTeam, redPercent.ToString("0"));

    }
}
