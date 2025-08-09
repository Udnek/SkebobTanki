using Tank.Parts;
using UnityEngine;

namespace Test
{
    public class Track : MonoBehaviour
    {
        private Rigidbody tankRigidbody;
        private Controls controls;
        private bool isLeftTrack;

        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float FIRST_TRACK_STATUS = 1f;
        [SerializeField] private float SECOND_TRACK_STATUS = 1f; 
        [SerializeField] float steeringForceMultiplier = 1f;
        [SerializeField] private BoxCollider tankTrackTriggerCollider;
        
        private float trackMovementSpeedRight;
        private float trackMovementSpeedLeft;
        
        private Vector3 trackPositionLocal;
        private Vector3 trackPositionWorld;

        private void Awake()
        {
            tankRigidbody = GetComponent<Part>().tank.GetComponent<Rigidbody>();
            controls = new Controls();
            DefineTrackSide();
        }

        private void Start()
        {
            if (groundLayer.value == 0)
                groundLayer = LayerMask.GetMask("Default");
        }
        
        private void OnEnable()
        {
            controls.Player.Move.Enable();
            controls.Player.Turn.Enable();
        }

        private void DefineTrackSide()
        {
            Vector3 centerInTankSpace = GetComponent<Part>().tank.transform.position;
            isLeftTrack = centerInTankSpace.x < 0;
            
            if (isLeftTrack)
            {
                trackMovementSpeedRight = FIRST_TRACK_STATUS;
                trackMovementSpeedLeft = SECOND_TRACK_STATUS;
            }
            else
            {
                trackMovementSpeedRight = SECOND_TRACK_STATUS;
                trackMovementSpeedLeft = FIRST_TRACK_STATUS;
            }
        }

        private void FixedUpdate()
        {
            Vector2 readMoveVector = controls.Player.Move.ReadValue<Vector2>();
            Vector2 readTurnVector = controls.Player.Turn.ReadValue<Vector2>();
            if (CheckTrackGrounded(tankTrackTriggerCollider)) 
                TrackMovement(readMoveVector, readTurnVector);
        }
        
        private bool CheckTrackGrounded(BoxCollider trackCollider)
        {
            if (trackCollider == null) return false;
        
            Vector3 center = trackCollider.transform.TransformPoint(trackCollider.center);
            Vector3 halfExtents = trackCollider.size * 0.5f;
            Quaternion rotation = trackCollider.transform.rotation;
        
            return Physics.CheckBox(
                center, 
                halfExtents, 
                rotation, 
                groundLayer
            );
        }

        private void TrackMovement(Vector2 readMoveVector, Vector2 readTurnVector)
        {
            float readMoveValue = readMoveVector.y;
            float readTurnValue = readTurnVector.x;
            trackPositionWorld = transform.TransformPoint(trackPositionLocal);
            Vector3 trackForce = transform.forward * (readMoveValue * steeringForceMultiplier);
            if (readMoveValue != 0)
            {
                if (readTurnValue != 0)
                    steeringForceMultiplier = (readTurnValue > 0) ? trackMovementSpeedRight : trackMovementSpeedLeft;
                trackForce = transform.forward * (readMoveValue * steeringForceMultiplier);
            }
            else
            {
                if (readTurnValue > 0 || readTurnValue < 0)
                    trackForce = (isLeftTrack)
                        ? transform.forward * (readTurnValue * 1f)
                        : transform.forward * (-1 * readTurnValue * 1f);
            }
            tankRigidbody.AddForceAtPosition(trackForce, trackPositionWorld);
        }
    }
}