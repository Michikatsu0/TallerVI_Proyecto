public interface IPlayerMechanicProvider
{
    void Movement();
    void Crouch();
    void Gravity();
    void Jump();
    void Fall();
    void SlopeSlide();
    void PushObjects();
    void Rotation();
    void AimAnimationMovement();
    void AimRayCast();
    void TriggerWeapon();
    void Dash();
    void UpdateCameraHeight();
}