using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MRCH.Common.Interact
{
    public abstract class TouchManager : MonoBehaviour
    {
        [InfoBox("Add Collider and TouchableObject.cs to the object you want to be touchable")]
        [Required,
         InfoBox("Assign this and all touchable Objects to a (special) layer", InfoMessageType.Error,
             "TouchableLayerAssigned")]
        public LayerMask touchableLayer; // Assign this in the Inspector to include only the touchable objects

        private bool TouchableLayerAssigned => touchableLayer == 0;

        private static bool _isTouchable = true;

        [Space(10), Header("Universal Touch Event"), SerializeField]
        protected UnityEvent universalTouchEvent;

        [SerializeField] protected UnityEvent universalReturnEvent;

        [Title("Setting"), PropertyRange(0.1f, 300f), SerializeField]
        private float touchRange = 10f;

        private Camera _mainCam;

        [Space, SerializeField,
         InfoBox("Enable this if you want other objects to be unable to interact after one is touched"),
         Tooltip("Enable this if you want other objects to be unable to interact after one is touched")]
        private bool disableTouchOfOtherObjects;

        [Space] public float clickInterval = 0.5f;
        private float _timeCnt = float.MaxValue;

        // Input System actions
        protected InputAction touchAction;
        //[SerializeField] protected InputAction clickAction;

        [Space, SerializeField] protected bool showGizmos = true;


        protected virtual void Start()
        {
            if (Camera.main == null)
            {
                Debug.LogError("Main Camera not found!!!");
            }

            _mainCam = Camera.main;

            if (touchableLayer == 0)
                Debug.LogWarning("Please check if you forgot to assign the touchable layer on " + gameObject.name);

            touchAction = new InputAction(binding: "<Touchscreen>/press");
            touchAction.AddBinding("<Mouse>/leftButton");


            touchAction.Enable();
        }

        protected virtual void OnEnable()
        {
            touchAction?.Enable();
        }

        protected virtual void OnDisable()
        {
            touchAction.Disable();
        }


        protected virtual void Update()
        {
            _timeCnt += Time.deltaTime;

            if (touchAction.WasPressedThisFrame())
            {
                Vector3 inputPosition;

                // Check if the input is from touchscreen or mouse
                if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
                {
                    inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
                }
                else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
                {
                    inputPosition = Mouse.current.position.ReadValue();
                }
                else
                {
                    return;
                }

                var ray = _mainCam.ScreenPointToRay(inputPosition);
                if (Physics.Raycast(ray, out var hit, touchRange, touchableLayer))
                {
                    var touchable = hit.transform.GetComponent<TouchableObject>();
                    if (touchable)
                    {
                        if (_timeCnt <= clickInterval) return;
                        _timeCnt = 0f;
                        if (touchable.isReturn)
                        {
                            universalTouchEvent?.Invoke();
                            touchable.OnTouch();

                            if (touchable.isReturn) OnReturn();
                        }
                        else
                        {
                            if (disableTouchOfOtherObjects && !_isTouchable) return;

                            if (disableTouchOfOtherObjects)
                                _isTouchable = false;

                            Debug.Log("Universal Touch/Click Event triggered");
                            universalTouchEvent?.Invoke();
                            touchable.OnTouch();
                        }
                    }
                    else
                    {
                        Debug.LogWarning(hit.transform.name + " has no TouchableObject component");
                    }
                }
            }
        }

        public virtual void OnReturn()
        {
            if (disableTouchOfOtherObjects)
                _isTouchable = true;
            Debug.Log("Universal Return Event triggered");
            universalReturnEvent?.Invoke();
        }

        protected void OnDrawGizmosSelected()
        {
            if (!enabled || !showGizmos) return;

            Gizmos.color = new Color(1, 0.5f, 0.5f, 0.75f);
            Gizmos.DrawWireSphere(transform.position, touchRange);
#if UNITY_EDITOR
            var labelPosition = transform.position + Vector3.forward * touchRange;
            Handles.Label(labelPosition, "Touch Range");
#endif
        }
    }
}