using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemText : MonoBehaviour
{
   [Header("Item Text")] 
   [SerializeField] private TextMeshProUGUI text;

   public void SetText(string message, Color color)
   {
      this.text.text = message;
      this.text.color = color;
   }
}
