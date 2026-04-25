using UnityEngine;

public class VRCameraManager : MonoBehaviour
{
    public static VRCameraManager instance;

    [Tooltip("Kéo VR Camera Rig (hoặc XR Origin) vào đây")]
    public Transform vrRig;

    [Tooltip("Kéo Object làm điểm bắt đầu (vị trí & góc xoay) vào đây")]
    public Transform startPoint;

    [Tooltip("Kéo Object làm điểm dịch chuyển sau khi ráp xong 1 mảnh tháp vào đây")]
    public Transform afterBuildPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // Đặt vị trí ban đầu cho người chơi khi game mở lên
        if (startPoint != null)
        {
            TeleportTo(startPoint);
        }
    }

    /// <summary>
    /// Dịch chuyển người chơi đến điểm đặt sẵn sau khi xây xong tháp
    /// </summary>
    public void TeleportToAfterBuildPoint()
    {
        if (afterBuildPoint != null)
        {
            TeleportTo(afterBuildPoint);
        }
    }

    /// <summary>
    /// Hàm lõi thực hiện thay đổi vị trí Offset nhưng không khóa cứng chuyển động
    /// Dịch chuyển 1 lần duy nhất (Teleport)
    /// </summary>
    public void TeleportTo(Transform targetPoint)
    {
        if (vrRig == null || targetPoint == null) return;

        vrRig.position = targetPoint.position;
        vrRig.rotation = targetPoint.rotation;
    }
}
