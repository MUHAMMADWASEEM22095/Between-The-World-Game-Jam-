using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LightmapThemeController : MonoBehaviour
{
    // Original baked lightmaps ko save karne ke liye variable
    private LightmapData[] originalLightmaps;

    public Image Key;

    private Color keyColor;

    void Start()
    {
        // Game start hotay hi baked lighting ka data save kar lein
        originalLightmaps = LightmapSettings.lightmaps;
        
        // Set Key alpha on start
        keyColor = Key.color;
        keyColor.a = 0.1f;
        Key.color = keyColor;
    }

    // Is function ko call karne se Dark Theme (Baked Lighting) apply hogi
    public void EnableDarkTheme()
    {
        keyColor = Key.color;
        keyColor.a = 1f;
        Key.color = keyColor;
        LightmapSettings.lightmaps = originalLightmaps;
        Debug.Log("Dark Theme (Baked Lighting) Enabled");
    }

    // Is function ko call karne se Bright Theme (Baked Lighting OFF) apply hogi
    public void EnableBrightTheme()
    {
        // Lightmaps ke array ko empty kar dein, is se baked data disable ho jayega
        LightmapSettings.lightmaps = new LightmapData[0];
        Debug.Log("Bright Theme (Baked Lighting OFF) Enabled");
       


        keyColor = Key.color;
        keyColor.a = 0.1f;
        Key.color = keyColor;keyColor = Key.color;
        keyColor.a = 0.1f;
        Key.color = keyColor;
    }

    // Testing ke liye: Keyboard ke buttons se control karein
    void Update()
    {
        // D dabane se Dark theme
        if (Input.GetKeyDown(KeyCode.V))
        {
            EnableDarkTheme();
        }
        
        // B dabane se Bright theme
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnableBrightTheme();
        }
    }
}