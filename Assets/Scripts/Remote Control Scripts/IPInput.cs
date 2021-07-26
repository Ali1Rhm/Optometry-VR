using UnityEngine.UI;
using UnityEngine;

public class IPInput : MonoBehaviour
{
    [SerializeField] private LocalhostData data;

    public void ChangeIP(InputField inputField)
    {
        data.IP = inputField.text;
        print(data.IP);
    }
}
