using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TimerStudy : MonoBehaviour
{
    public Text t;

    private void Start()
    {
        float value = 123;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("DATA : 123");
        sb.Append("FORMAT : value.ToString(\"00000\") ---------> ");
        sb.AppendLine(value.ToString("00000"));
        sb.Append("FORMAT : value.ToString(\"#####\") ---------> ");
        sb.AppendLine(value.ToString("#####"));
        sb.AppendLine();

        value = 1.2f;
        sb.AppendLine("DATA : 1.2");
        sb.Append("FORMAT : value.ToString(\"000.00\") ---------> ");
        sb.AppendLine(value.ToString("0.00"));
        sb.Append("FORMAT : value.ToString(\"###.##\") ---------> ");
        sb.AppendLine(value.ToString("#.##"));
        sb.AppendLine();

        value = 1234567890;
        sb.AppendLine("DATA : 1234567890");
        sb.Append("FORMAT : value.ToString(\"0,0\") ---------> ");
        sb.AppendLine(value.ToString("0,0"));
        sb.Append("FORMAT : value.ToString(\"#,#\") ---------> ");
        sb.AppendLine(value.ToString("#,#"));
        sb.AppendLine();


        value = 0.74f;
        sb.AppendLine("DATA : 0.74");
        sb.Append("FORMAT : value.ToString(\"##.00%\") ---------> ");
        sb.AppendLine(value.ToString("##.00%"));

        sb.Append("FORMAT : value.ToString(\"##.00¢¶\") ---------> ");
        sb.AppendLine(value.ToString("###.00¢¶"));
        sb.AppendLine();



        value = 36.5f;
        sb.AppendLine("DATA : 36.5");
        sb.Append("FORMAT : value.ToString(\"#¡ÆC\") ---------> ");
        sb.AppendLine(value.ToString("##.##¡ÆC"));
        sb.AppendLine();

        t.text = sb.ToString();
    }
}
