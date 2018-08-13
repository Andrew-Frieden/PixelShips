using UnityEngine;

public class FpsCounter : MonoBehaviour
{

    public UnityEngine.UI.Text displayText;

    public double updatesPerSecond = 4.0;
    private double updateInterval { get { return 1.0 / updatesPerSecond; } }

    private double frameCount;
    private double timeSinceLastUpdate;
    private double displayFps;

    void Update()
    {
        frameCount++;
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate > updateInterval)
        {
            displayFps = frameCount / timeSinceLastUpdate;
            displayText.text = string.Format("{0:f2}", displayFps);

            frameCount = 0;
            timeSinceLastUpdate -= updateInterval;
        }
    }
}
