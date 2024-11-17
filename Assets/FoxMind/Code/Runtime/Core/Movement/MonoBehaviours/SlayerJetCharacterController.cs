using System;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.Serialization;

namespace FoxMind.Code.Runtime.Core.Movement.MonoBehaviours
{
    /// <summary>
    /// Отвечает за мувмент главного героя
    /// </summary>
    public class SlayerJetCharacterController : MonoBehaviour, ICharacterController
    {
        [SerializeField] private KinematicCharacterMotor motor;
        
        [Header("Stable Movement")]
        [SerializeField] public float MaxStableMoveSpeed = 10f;
        [SerializeField] private float StableMovementSharpness = 15;
        [SerializeField] private float OrientationSharpness = 10;

        [Header("Air Movement")]
        [SerializeField] private float MaxAirMoveSpeed = 10f;
        [SerializeField] private float AirAccelerationSpeed = 5f;
        [SerializeField] private float Drag = 0.1f;
        
        [Header("Animation Parameters")]
        public float ForwardAxisSharpness = 10;
        public float TurnAxisSharpness = 5;
        
        [Header("Jumping")]
        public bool AllowJumpingWhenSliding = false;
        public bool AllowDoubleJump = false;
        public bool AllowWallJump = false;
        public float JumpSpeed = 10f;
        public float JumpPreGroundingGraceTime = 0f;
        public float JumpPostGroundingGraceTime = 0f;
        
        [Header("Misc")]
        [SerializeField] private Vector3 Gravity = new Vector3(0, -30f, 0);
        
        [SerializeField] public Vector3 _moveInputVector;
        [SerializeField] public Vector3 _lookInputVector;
        
        [SerializeField] public Vector3 MoveRootMotionVector;
        [SerializeField] public Quaternion LookRootMotionQuaternion;
        
        [SerializeField] public bool RootMotion = false;
        
        [SerializeField] private bool _jumpRequested = false;
        [SerializeField] private bool _jumpConsumed = false;
        [SerializeField] private bool _jumpedThisFrame = false;
        [SerializeField] private float _timeSinceJumpRequested = Mathf.Infinity;
        [SerializeField] private float _timeSinceLastAbleToJump = 0f;
        [SerializeField] private bool _doubleJumpConsumed = false;
        [SerializeField] private bool _canWallJump = false;
        [SerializeField] private Vector3 _wallJumpNormal;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        
        private float _forwardAxis;
        private float _rightAxis;
        private float _targetForwardAxis;
        private float _targetRightAxis;

        private void Start()
        {
            motor.CharacterController = this;
        }

        private void Update()
        {
            // Handle animation
            _forwardAxis = Mathf.Lerp(_forwardAxis, _targetForwardAxis, 1f - Mathf.Exp(-ForwardAxisSharpness * Time.deltaTime));
            _rightAxis = Mathf.Lerp(_rightAxis, _targetRightAxis, 1f - Mathf.Exp(-TurnAxisSharpness * Time.deltaTime));
        }

        public void SetMoveDirection(Vector3 normalizedMoveDirection)
        {
            _moveInputVector = normalizedMoveDirection;
            _lookInputVector = normalizedMoveDirection;

            _targetForwardAxis = normalizedMoveDirection.y;
            _targetRightAxis = normalizedMoveDirection.x;
        }

        public void SetJumpRequest()
        {
            _timeSinceJumpRequested = 0f;
            _jumpRequested = true;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (RootMotion)
            {
                currentRotation = LookRootMotionQuaternion * currentRotation;
            }
            else
            {
                if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
                {
                    // Smoothly interpolate from current to target look direction
                    Vector3 smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                    // Set the current rotation (which will be used by the KinematicCharacterMotor)
                    currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
                }
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Vector3 targetMovementVelocity = Vector3.zero;
            if (motor.GroundingStatus.IsStableOnGround)
            {
                if (RootMotion)
                {
                    if (deltaTime > 0)
                    {
                        // The final velocity is the velocity from root motion reoriented on the ground plane
                        currentVelocity = MoveRootMotionVector / deltaTime;
                        currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;
                    }
                    else
                    {
                        // Prevent division by zero
                        currentVelocity = Vector3.zero;
                    }
                }
                else
                {
                    // Reorient source velocity on current ground slope (this is because we don't want our smoothing to cause any velocity losses in slope changes)
                    currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                    // Calculate target velocity
                    Vector3 inputRight = Vector3.Cross(_moveInputVector, motor.CharacterUp);
                    Vector3 reorientedInput = Vector3.Cross(motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                    targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                    // Smooth movement Velocity
                    currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
                }
            }
            else
            {
                if (RootMotion)
                {
                    if (_forwardAxis > 0f)
                    {
                        // If we want to move, add an acceleration to the velocity
                        Vector3 rootMotionTargetMovementVelocity = motor.CharacterForward * _forwardAxis * MaxAirMoveSpeed;
                        Vector3 velocityDiff = Vector3.ProjectOnPlane(rootMotionTargetMovementVelocity - currentVelocity, Gravity);
                        currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                    }
                }
                else
                {
                    // Add move input
                    if (_moveInputVector.sqrMagnitude > 0f)
                    {
                        targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                        // Prevent climbing on un-stable slopes with air movement
                        if (motor.GroundingStatus.FoundAnyGround)
                        {
                            Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp).normalized;
                            targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                        }

                        Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                        currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                    }
                }

                // Gravity
                currentVelocity += Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }
            
            _jumpedThisFrame = false;
            _timeSinceJumpRequested += deltaTime;
            if (_jumpRequested)
            {
                // Handle double jump
                if (AllowDoubleJump)
                {
                    if (_jumpConsumed && !_doubleJumpConsumed && (AllowJumpingWhenSliding ? !motor.GroundingStatus.FoundAnyGround : !motor.GroundingStatus.IsStableOnGround))
                    {
                        motor.ForceUnground(0.1f);

                        // Add to the return velocity and reset jump state
                        currentVelocity += (motor.CharacterUp * JumpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
                        _jumpRequested = false;
                        _doubleJumpConsumed = true;
                        _jumpedThisFrame = true;
                    }
                }

                // See if we actually are allowed to jump
                if (_canWallJump ||
                    (!_jumpConsumed && ((AllowJumpingWhenSliding ? motor.GroundingStatus.FoundAnyGround : motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime)))
                {
                    // Calculate jump direction before ungrounding
                    Vector3 jumpDirection = motor.CharacterUp;
                    if (_canWallJump)
                    {
                        jumpDirection = _wallJumpNormal;
                    }
                    else if (motor.GroundingStatus.FoundAnyGround && !motor.GroundingStatus.IsStableOnGround)
                    {
                        jumpDirection = motor.GroundingStatus.GroundNormal;
                    }

                    // Makes the character skip ground probing/snapping on its next update. 
                    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                    motor.ForceUnground(0.1f);

                    // Add to the return velocity and reset jump state
                    currentVelocity += (jumpDirection * JumpSpeed) - Vector3.Project(currentVelocity, motor.CharacterUp);
                    _jumpRequested = false;
                    _jumpConsumed = true;
                    _jumpedThisFrame = true;
                }
            }

            // Reset wall jump
            _canWallJump = false;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            // Handle jump-related values
            {
                // Handle jumping pre-ground grace period
                if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                {
                    _jumpRequested = false;
                }

                if (AllowJumpingWhenSliding ? motor.GroundingStatus.FoundAnyGround : motor.GroundingStatus.IsStableOnGround)
                {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame)
                    {
                        _doubleJumpConsumed = false;
                        _jumpConsumed = false;
                    }
                    _timeSinceLastAbleToJump = 0f;
                }
                else
                {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }
            
            MoveRootMotionVector = Vector3.zero;
            LookRootMotionQuaternion = Quaternion.identity;
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            // We can wall jump only if we are not stable on ground and are moving against an obstruction
            if (AllowWallJump && !motor.GroundingStatus.IsStableOnGround && !hitStabilityReport.IsStable)
            {
                _canWallJump = true;
                _wallJumpNormal = hitNormal;
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            
        }
    }
}