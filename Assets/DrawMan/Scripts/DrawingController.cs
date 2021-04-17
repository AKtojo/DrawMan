using UnityEngine;
using DrawMan.Core.ActionSystem;
using DrawMan.Core.Variables;
using DrawMan.Core.Utility;

namespace DrawMan.Core
{
    public class DrawingController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private DrawAction m_drawAction;
        [SerializeField] private ObjectPool m_linesPool;

        [Header("Variables")]
        [Tooltip("Actual Ink remaining variable asset.")]
        [SerializeField] private FloatVariable m_inkActual;
        [Tooltip("Ink's visible consumption variable")]
        [SerializeField] private FloatVariable m_inkCurrent;

        [Header("Line Drawing Rules")]
        [SerializeField] [Range(0.0f, 1.0f)] private float m_timeScale;
        [Tooltip("Line drain in percentage per second (%/s).\n[e.g.: 12.34% = 0.1234]")]
        [SerializeField] [Min(0.0f)] private float m_lineDrain;
#if UNITY_EDITOR
        [Tooltip("Minimum length the line must have to be drawn.")]
        [SerializeField] [Min(0.0f)] private float m_minLineLength;
        [Tooltip("Maximum distance the player can touch the line from the point it's been drawn.")]
        [SerializeField] [Min(0.0f)] private float m_maxDistToEnd;

        [Space]
        [SerializeField] private bool DEBUG;

        private void OnValidate()
        {
            m_sqrMinLineLength = m_minLineLength * m_minLineLength;
            m_sqrMaxDistToEnd = m_maxDistToEnd * m_maxDistToEnd;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.25f); // transparent red

            if (m_currentLine)
            {
                // the zone where you touch that must have nothing inside
                Gizmos.DrawSphere(m_drawAction.Point, m_maxDistToEnd);
            }
        }

        private void OnGUI()
        {
            if (!DEBUG) return;

            GUI.TextArea(new Rect(0, 0, 256, 256),
                string.Format(
                    "Press: {0}\n" +
                    "Release: {1}\n" +
                    "Touch: {2}\n" +
                    "Delta: {3}\n" +
                    "Point: {4}\n" +
                    "Distance: {5}\n" +
                    "Ink: {6}/{7}",
                    m_drawAction.Press, m_drawAction.Release, m_drawAction.Touch,
                    m_drawAction.Delta, m_drawAction.Point, Mathf.Sqrt(m_sqrTravelledDist),
                    m_inkCurrent.Value, m_inkActual.Value));
        }

        private void Awake()
        {
            m_linesPool.Clear();
        }
#endif
        [SerializeField] [HideInInspector] private float m_sqrMinLineLength;
        [SerializeField] [HideInInspector] private float m_sqrMaxDistToEnd;

        private LineBuilder m_currentLine = null;

        private float m_sqrTravelledDist;

        private bool CurrentLineIsLongEnough => m_sqrTravelledDist >= m_sqrMinLineLength;
        private bool LineIsValid => m_currentLine.IsValid &&
                                    m_currentLine.PointIsFarEnough(m_drawAction.Point, m_sqrMaxDistToEnd);
        private bool InkDrained => m_inkCurrent.Value <= 0.0f || m_inkActual.Value <= 0.0f;
        private bool StartDrawing =>    //m_drawAction.Press &&
            Input.GetMouseButtonDown(0) &&
                                        m_inkCurrent.Value > 0.0f &&
                                        !m_currentLine;
        private bool EndDrawing =>  //(m_drawAction.Release ||
            (Input.GetMouseButtonUp(0) ||
                                    m_inkCurrent.Value <= 0.0f) &&
                                    m_currentLine != null;

        private void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    var obj = m_linesPool.GetObject();
            //    obj.SetActive(true);
            //    obj.name = "culo";
            //}
            //return;

            if (StartDrawing)
            {
                InstantiateLine();
            }
            else if (EndDrawing)
            {
                ReleaseLine();
                return;
            }

            if (m_currentLine != null)
            {
                UpdateLine();
            }
        }

        private void UpdateLine()
        {
            m_sqrTravelledDist += m_drawAction.Delta.sqrMagnitude;
            if (CurrentLineIsLongEnough)
            {
                m_inkCurrent.Value -= m_lineDrain;
                m_sqrTravelledDist -= m_sqrMinLineLength;
                if (LineIsValid)
                {
                    Time.timeScale = m_timeScale;
                    Vector3 point = m_drawAction.Point;
                    point.z = m_currentLine.LineClipZ;
                    // TODO: change Camera.main
                    var camPoint = Camera.main.ScreenToWorldPoint(point);
                    camPoint.z = m_currentLine.LineDepthZ;
                    m_currentLine.AddPointToLine(camPoint);
                }
                else
                {
                    DisableLine();
                    return;
                }

                if (InkDrained)
                {
                    ReleaseLine();
                }
            }
        }

        private void InstantiateLine()
        {
            var obj = m_linesPool.GetObject();
            obj.SetActive(true);
            m_currentLine = (LineBuilder)obj.GetComponent(typeof(LineBuilder));
            m_currentLine.transform.SetParent(transform);
            m_currentLine.EnablePhysics();
        }

        private void ReleaseLine()
        {
            Time.timeScale = 1.0f;
            m_currentLine.StartTimer();
            m_inkCurrent.Value = Mathf.Clamp01(m_inkCurrent.Value);
            m_sqrTravelledDist = 0.0f;
            m_inkActual.Value = m_inkCurrent.Value;
            m_currentLine = null;
        }

        private void DisableLine()
        {
            m_currentLine.DisableLine();
            m_inkCurrent.Value = 1.0f;
            m_sqrTravelledDist = 0.0f;
            m_inkActual.Value = m_inkCurrent.Value;
            m_currentLine = null;
        }
    }
}
