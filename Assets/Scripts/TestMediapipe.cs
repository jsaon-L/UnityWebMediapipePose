using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;
using Edge = System.Tuple<int, int>;

public class TestMediapipe : MonoBehaviour
{

    // copy from file:///C:/Program%20Files/Unity/Hub/Editor/2019.4.17f1c1/Editor/Data/Documentation/en/Manual/webgl-interactingwithbrowserscripting.html
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);

    [DllImport("__Internal")]
    private static extern void PrintFloatArray(float[] array, int size);

    [DllImport("__Internal")]
    private static extern int AddNumbers(int x, int y);

    [DllImport("__Internal")]
    private static extern string StringReturnValueFunction();

    [DllImport("__Internal")]


    private static extern void BindWebGLTexture(int texture);


    public const string PRE_JSON_STR = "{\"Datas\":";
    public const string END_JSON_STR = "}";


    [SerializeField]
    private GameObject _boneLinePrefab;
    private List<LineRenderer> _boneLines;
    private StringBuilder _jsonBuffer = new StringBuilder();
    private WebCamTexture _camTexture;


    protected static readonly IList<Edge> _Connections = new List<Edge> {
      // Right Arm
      new Edge(11, 13),
      new Edge(13, 15),
      // Left Arm
      new Edge(12, 14),
      new Edge(14, 16),
      // Torso
      new Edge(11, 12),
      new Edge(12, 24),
      new Edge(24, 23),
      new Edge(23, 11),
      // Right Leg
      new Edge(23, 25),
      new Edge(25, 27),
      new Edge(27, 29),
      new Edge(29, 31),
      new Edge(31, 27),
      // Left Leg
      new Edge(24, 26),
      new Edge(26, 28),
      new Edge(28, 30),
      new Edge(30, 32),
      new Edge(32, 28),
    };



    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("打开摄像头成功");

            _camTexture = new WebCamTexture(640, 480);
            GetComponent<Renderer>().material.mainTexture = _camTexture;
            _camTexture.Play();
        }
        else
        {
            Debug.LogError("打开摄像头失败");
        }

        _boneLines = new List<LineRenderer>(_Connections.Count);
        foreach (var item in _Connections)
        {
            var go = Instantiate<GameObject>(_boneLinePrefab);
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(-0.5f,-0.5f,0);
            go.transform.localScale = Vector3.one;
            _boneLines.Add(go.GetComponent<LineRenderer>());
        }

    }

    private bool startSend = false;
    private bool results = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            startSend = !startSend;
        }
        if (startSend&&results)
        {
            results = false;
            Hello();
        }

    }



    public void ResultsPoseValue(string landmark)
    {
        results = true;

        if ("undefined" == landmark)
        {
            Debug.Log("Unity 收到骨骼数据为 空");
        }
        else
        {
            Debug.Log("Unity 收到骨骼数据了");
            _jsonBuffer.Clear();
            _jsonBuffer.Append(PRE_JSON_STR);
            _jsonBuffer.Append(landmark);
            _jsonBuffer.Append(END_JSON_STR);
            Landmarks landmarks = JsonUtility.FromJson<Landmarks>(_jsonBuffer.ToString());

            DrawLandmark(landmarks);
        }

    }


    public void DrawLandmark(Landmarks landmarks)
    {
        for (int i = 0; i < _Connections.Count; i++)
        {
            var l1 = landmarks.Datas[_Connections[i].Item1];
            var l2 = landmarks.Datas[_Connections[i].Item2];

            Vector3 p1 = new Vector3(1-l1.x, 1 - l1.y, 0);
            Vector3 p2 = new Vector3(1-l2.x, 1 - l2.y, 0);
            _boneLines[i].SetPositions(new Vector3[] { p1, p2 });
        }

    }


}


#region Json





[Serializable]
public struct Landmarks
{


    public Landmark[] Datas;



}

[Serializable]
public struct Landmark
{
    public float x;
    public float y;
    public float visibility;
}

#endregion