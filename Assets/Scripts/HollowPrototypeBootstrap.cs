using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Creates a playable greybox room and tracked ape hands at runtime.
/// Requires an OpenXR Android build with Quest Touch controllers enabled.
/// </summary>
public class HollowPrototypeBootstrap : MonoBehaviour
{
    [SerializeField] private float handRadius = 0.105f;
    [SerializeField] private float maxMovePerStep = 0.22f;
    [SerializeField] private float maxSpeed = 5.5f;

    private Transform rig;
    private Camera headCamera;
    private Transform leftHand;
    private Transform rightHand;
    private Vector3 previousLeft;
    private Vector3 previousRight;
    private bool hasHandHistory;
    private InputDevice leftDevice;
    private InputDevice rightDevice;
    private readonly Collider[] overlapResults = new Collider[16];

    private void Start()
    {
        Application.targetFrameRate = 72;
        BuildRoom();
        BuildPlayerRig();
        RefreshDevices();
    }

    private void Update()
    {
        if (!leftDevice.isValid || !rightDevice.isValid)
            RefreshDevices();

        UpdateHead();
        UpdateHand(leftDevice, leftHand);
        UpdateHand(rightDevice, rightHand);
    }

    private void FixedUpdate()
    {
        if (leftHand == null || rightHand == null)
            return;

        Vector3 l = leftHand.position;
        Vector3 r = rightHand.position;

        if (!hasHandHistory)
        {
            previousLeft = l;
            previousRight = r;
            hasHandHistory = true;
            return;
        }

        Vector3 push = Vector3.zero;
        if (HandTouchesWorld(l)) push += previousLeft - l;
        if (HandTouchesWorld(r)) push += previousRight - r;

        push.y = Mathf.Clamp(push.y, -maxMovePerStep, maxMovePerStep);
        Vector3 horizontal = new Vector3(push.x, 0f, push.z);
        if (horizontal.magnitude > maxMovePerStep)
            horizontal = horizontal.normalized * maxMovePerStep;

        rig.position += horizontal;
        previousLeft = leftHand.position;
        previousRight = rightHand.position;
    }

    private void RefreshDevices()
    {
        leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    private void UpdateHead()
    {
        Vector3 headPosition = InputTracking.GetLocalPosition(XRNode.Head);
        Quaternion headRotation = InputTracking.GetLocalRotation(XRNode.Head);
        headCamera.transform.localPosition = headPosition;
        headCamera.transform.localRotation = headRotation;
    }

    private void UpdateHand(InputDevice device, Transform hand)
    {
        if (device.isValid &&
            device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position) &&
            device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
        {
            hand.localPosition = position;
            hand.localRotation = rotation;
            hand.gameObject.SetActive(true);
        }
        else
        {
            hand.gameObject.SetActive(false);
        }
    }

    private bool HandTouchesWorld(Vector3 position)
    {
        int count = Physics.OverlapSphereNonAlloc(position, handRadius, overlapResults);
        for (int i = 0; i < count; i++)
        {
            Collider hit = overlapResults[i];
            if (hit != null && !hit.isTrigger && !hit.transform.IsChildOf(rig))
                return true;
        }
        return false;
    }

    private void BuildPlayerRig()
    {
        rig = new GameObject("HOLLOW Player Rig").transform;
        rig.position = new Vector3(0f, 0.05f, -2.2f);

        GameObject cameraObject = new GameObject("HMD Camera");
        cameraObject.transform.SetParent(rig, false);
        headCamera = cameraObject.AddComponent<Camera>();
        headCamera.nearClipPlane = 0.03f;
        headCamera.farClipPlane = 100f;
        headCamera.stereoTargetEye = StereoTargetEyeMask.Both;
        cameraObject.tag = "MainCamera";

        leftHand = CreateHand("Left Ape Hand", rig);
        rightHand = CreateHand("Right Ape Hand", rig);
        leftHand.localPosition = new Vector3(-0.25f, 1.1f, 0.45f);
        rightHand.localPosition = new Vector3(0.25f, 1.1f, 0.45f);
    }

    private Transform CreateHand(string handName, Transform parent)
    {
        GameObject hand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hand.name = handName;
        hand.transform.SetParent(parent, false);
        hand.transform.localScale = Vector3.one * 0.19f;
        Collider collider = hand.GetComponent<Collider>();
        Destroy(collider);
        Renderer renderer = hand.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        renderer.material.color = new Color(0.32f, 0.22f, 0.16f);
        return hand.transform;
    }

    private void BuildRoom()
    {
        CreateBlock("Floor", new Vector3(0f, -0.15f, 0f), new Vector3(12f, 0.3f, 12f), new Color(0.16f, 0.17f, 0.19f));
        CreateBlock("Back Wall", new Vector3(0f, 2f, 5.9f), new Vector3(12f, 4f, 0.2f), new Color(0.12f, 0.13f, 0.16f));
        CreateBlock("Left Wall", new Vector3(-5.9f, 2f, 0f), new Vector3(0.2f, 4f, 12f), new Color(0.12f, 0.13f, 0.16f));
        CreateBlock("Right Wall", new Vector3(5.9f, 2f, 0f), new Vector3(0.2f, 4f, 12f), new Color(0.12f, 0.13f, 0.16f));
        CreateBlock("Front Wall", new Vector3(0f, 2f, -5.9f), new Vector3(12f, 4f, 0.2f), new Color(0.12f, 0.13f, 0.16f));
        CreateBlock("Climbable Test Ledge", new Vector3(0f, 2.2f, 3.5f), new Vector3(3f, 0.25f, 0.6f), new Color(0.25f, 0.28f, 0.31f));

        GameObject lightObject = new GameObject("Room Light");
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 0.8f;
        lightObject.transform.rotation = Quaternion.Euler(45f, -30f, 0f);

        RenderSettings.ambientLight = new Color(0.24f, 0.25f, 0.29f);
    }

    private void CreateBlock(string blockName, Vector3 position, Vector3 scale, Color color)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.name = blockName;
        block.transform.position = position;
        block.transform.localScale = scale;
        Renderer renderer = block.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        renderer.material.color = color;
    }
}
