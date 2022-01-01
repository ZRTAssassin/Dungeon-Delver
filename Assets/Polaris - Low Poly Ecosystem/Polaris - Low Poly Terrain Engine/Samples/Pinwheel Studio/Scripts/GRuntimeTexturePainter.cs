#if GRIFFIN
using Pinwheel.Griffin.PaintTool;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pinwheel.Griffin
{
    public class GRuntimeTexturePainter : MonoBehaviour
    {
        [SerializeField] GTerrainTexturePainter painter;
        public GTerrainTexturePainter Painter
        {
            get
            {
                return painter;
            }
            set
            {
                painter = value;
            }
        }

        [SerializeField] GameObject cursorPrefab;
        public GameObject CursorPrefab
        {
            get
            {
                return cursorPrefab;
            }
            set
            {
                cursorPrefab = value;
            }
        }

        [SerializeField] KeyCode alternativeKey;
        public KeyCode AlternativeKey
        {
            get
            {
                return alternativeKey;
            }
            set
            {
                alternativeKey = value;
            }
        }

        [SerializeField] KeyCode negativeKey;
        public KeyCode NegativeKey
        {
            get
            {
                return negativeKey;
            }
            set
            {
                negativeKey = value;
            }
        }

        [SerializeField] Dropdown modeDropdown;
        public Dropdown ModeDropdown
        {
            get
            {
                return modeDropdown;
            }
            set
            {
                modeDropdown = value;
            }
        }

        [SerializeField] Slider redSlider;
        public Slider RedSlider
        {
            get
            {
                return redSlider;
            }
            set
            {
                redSlider = value;
            }
        }

        [SerializeField] Slider greenSlider;
        public Slider GreenSlider
        {
            get
            {
                return greenSlider;
            }
            set
            {
                greenSlider = value;
            }
        }

        [SerializeField] Slider blueSlider;
        public Slider BlueSlider
        {
            get
            {
                return blueSlider;
            }
            set
            {
                blueSlider = value;
            }
        }

        [SerializeField] Slider alphaSlider;
        public Slider AlphaSlider
        {
            get
            {
                return alphaSlider;
            }
            set
            {
                alphaSlider = value;
            }
        }

        [SerializeField] Image colorImage;
        public Image ColorImage
        {
            get
            {
                return colorImage;
            }
            set
            {
                colorImage = value;
            }
        }

        [SerializeField] Slider radiusSlider;
        public Slider RadiusSlider
        {
            get
            {
                return radiusSlider;
            }
            set
            {
                radiusSlider = value;
            }
        }

        [SerializeField] Slider opacitySlider;
        public Slider OpacitySlider
        {
            get
            {
                return opacitySlider;
            }
            set
            {
                opacitySlider = value;
            }
        }

        [SerializeField] Button resetButton;
        public Button ResetButton
        {
            get
            {
                return resetButton;
            }
            set
            {
                resetButton = value;
            }
        }

        [SerializeField] Text logText;
        public Text LogText
        {
            get
            {
                return logText;
            }
            set
            {
                logText = value;
            }
        }

        GameObject cursorInstance;
        List<string> logs;

        void Reset()
        {
            Painter = GetComponent<GTerrainTexturePainter>();
            AlternativeKey = KeyCode.LeftShift;
            NegativeKey = KeyCode.LeftControl;
        }

        void OnEnable()
        {
            if (ResetButton != null)
            {
                ResetButton.onClick.AddListener(OnResetButtonClicked);
            }

            logs = new List<string>();
        }

        void OnDisable()
        {
            if (ResetButton != null)
            {
                ResetButton.onClick.RemoveListener(OnResetButtonClicked);
            }
        }

        void Update()
        {
            try
            {
                UpdateParameters();
                if (Camera.main == null)
                    return;
                if (Painter == null)
                    return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (GStylizedTerrain.Raycast(ray, out hit, 1000, Painter.GroupId))
                {
                    DrawCursor(hit, true);
                    Paint(hit);
                }
                else
                {
                    DrawCursor(hit, false);
                }
            }
            catch (System.Exception e)
            {
                logs.Add(e.ToString());
                ShowLogs();
            }
        }

        void UpdateParameters()
        {
            if (Painter == null)
                return;
            Painter.Mode = ModeDropdown.value == 0 ? GTexturePaintingMode.Elevation : GTexturePaintingMode.Albedo;
            Painter.BrushRadius = RadiusSlider.value;
            Painter.BrushOpacity = OpacitySlider.value;

            Color c = new Color(
                RedSlider.value, GreenSlider.value, BlueSlider.value, AlphaSlider.value);
            ColorImage.color = c;
            Painter.BrushColor = c;
        }

        void DrawCursor(RaycastHit hit, bool isHit)
        {
            if (CursorPrefab == null)
                return;

            if (cursorInstance == null)
            {
                cursorInstance = GameObject.Instantiate(CursorPrefab);
                cursorInstance.transform.parent = transform;
            }

            if (isHit)
            {
                cursorInstance.gameObject.SetActive(true);
                cursorInstance.transform.position = hit.point;
                cursorInstance.transform.localScale = 2 * Vector3.one * Painter.BrushRadius;
            }
            else
            {
                cursorInstance.gameObject.SetActive(false);
            }
        }

        void Paint(RaycastHit hit)
        {
            if (!Input.GetMouseButton(0))
                return;

            GTexturePainterArgs args = new GTexturePainterArgs();
            args.HitPoint = hit.point;
            args.Collider = hit.collider;
            args.Transform = hit.transform;
            args.UV = hit.textureCoord;
            args.TriangleIndex = hit.triangleIndex;
            args.BarycentricCoord = hit.barycentricCoordinate;
            args.Distance = hit.distance;
            args.Normal = hit.normal;
            args.LightMapCoord = hit.lightmapCoord;

            args.MouseEventType =
                Input.GetMouseButtonDown(0) ? GPainterMouseEventType.Down :
                Input.GetMouseButton(0) ? GPainterMouseEventType.Drag :
                GPainterMouseEventType.Up;
            args.ActionType =
                Input.GetKey(AlternativeKey) ? GPainterActionType.Alternative :
                Input.GetKey(NegativeKey) ? GPainterActionType.Negative :
                GPainterActionType.Normal;
            Painter.Paint(args);
        }

        void OnResetButtonClicked()
        {
            IEnumerator<GStylizedTerrain> terrains = GStylizedTerrain.ActiveTerrains.GetEnumerator();
            while (terrains.MoveNext())
            {
                GStylizedTerrain t = terrains.Current;
                if (t.TerrainData != null)
                {
                    t.TerrainData.Geometry.ResetFull();
                }
            }
        }

        void ShowLogs()
        {
            string s = GUtilities.ListElementsToString(logs, "\n");
            LogText.text = s;
        }
    }
}
#endif
