using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 执念涌现的脚本 : BasePanel
{
  [SerializeField] TMPro.TextMeshProUGUI m_TextMeshPro;
    public void 改字(string text)
    {
        m_TextMeshPro.text = text;
    }
}
