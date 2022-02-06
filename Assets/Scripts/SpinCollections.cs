using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

public class SpinCollections : MonoBehaviour
{
    /// <summary>
    /// Controls the rate at which cube spins; higher is faster.
    /// Default int32: 33
    /// </summary>
    [SerializeField, Range(0, 999)]
    private int SpinFactor = 33;

    /// <summary>
    /// Controls the rate at which cube spins on each axis; higher is faster.
    /// Default Vector3: x = 1.0, y = 1.0, z.0 = 1
    /// </summary>
    [SerializeField]
    private Vector3 RotateAmount = new Vector3(1.0f, 1.0f, 1.0f);

    /// <summary>
    /// Controls the rate at which colors change; higher is slower.
    /// Default int32: 60
    /// </summary>
    [SerializeField, Range(0, 999)]
    private int RedColorFactor = 60;

    /// <summary>
    /// Stores the name of the gameObject as the Key and a Stack of colors it has changed to as it's Value
    /// </summary>
    private Dictionary<string, Stack<Color>> _cubeColorStack;

    private Stack<Color> _colorStack;

    private ArrayList _systemColors;

    private void Start()
    {
        _cubeColorStack = new Dictionary<string, Stack<Color>>();
        _colorStack = new Stack<Color>();
        _systemColors = new ArrayList
        {
            Color.white,
            Color.black,
            Color.gray,
            Color.clear,
            Color.green,
            Color.red,
            Color.cyan,
            Color.blue,
            Color.yellow,
            Color.magenta,
            Color.grey

        };
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(SpinFactor * Time.deltaTime * RotateAmount);

        Color color = Random.ColorHSV(0, 1, 0, 1, 0, 1);

        int red = (int)(color.r * 1000);

        int determinant = red % RedColorFactor;
        bool updateColor = determinant == 0;

        if (updateColor)
        {
            gameObject.GetComponent<Renderer>().material.color = color;
            _colorStack.Push(color);
            _cubeColorStack[gameObject.name] = _colorStack;
        }
    }

    void OnApplicationQuit()
    {
        LogData();
    }

    /// <summary>
    /// Gets the Keys and Values and prints them to the Debug Console
    /// </summary>
    private void LogData()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(string.Join(',', _cubeColorStack.Keys));
        foreach (Stack<Color> stack in _cubeColorStack.Values)
        {
            Color color;
            while (stack.TryPop(out color))
            {
                sb.AppendLine($"Red {color.r} Blue {color.b} Green {color.g}");
                foreach (Color c in _systemColors)
                {
                    if (c.r == color.r && c.g == color.g && c.b == color.b)
                    {
                        sb.AppendLine($"Wow you must be lucky! {color}");
                        Application.Quit();
                    }
                }
            }
        }
        Debug.Log(sb.ToString());
    }
}