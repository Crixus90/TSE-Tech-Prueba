using UnityEngine;
using UnityEngine.Android;
using TMPro;

public class LocationPermissionsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;

    private void Start()
    {
        CheckPermissions();
    }

    private void CheckPermissions()
    {
        if (Application.platform != RuntimePlatform.Android) return;

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            UpdateUI("Permission granted.", Color.green);
            Debug.Log("[LocationPermissions] Location permission granted.");
        }
        else
        {
            UpdateUI("Permissions pending... Requesting access.", Color.yellow);
            Debug.Log("[LocationPermissions] Requesting location permission...");
            Permission.RequestUserPermission(Permission.FineLocation);

            // check again after a short delay
            Invoke(nameof(VerifyPermissionAfterRequest), 1.0f);
        }
    }


    private void VerifyPermissionAfterRequest()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            UpdateUI("Permission granted.", Color.green);
            Debug.Log("[LocationPermissions] Permission granted.");
        }
        else
        {
            UpdateUI("Permission denied. Enable in settings.", Color.red);
            Debug.Log("[LocationPermissions] Permission denied.");
        }

        
    }

    // Would use callbacks instead but it's unsupported in Unity 2019.

    private void UpdateUI(string message, Color color)
    {
        if (statusText == null)
        {
            Debug.LogWarning("[LocationPermissions] Status text UI not assigned!");
            return;
        }

        statusText.text = message;
        statusText.color = color;
    }
}