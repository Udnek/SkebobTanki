using UnityEngine;

public class TankTracksAnimation : MonoBehaviour
{
    public Texture[] leftTrackTextures;
    public Texture[] rightTrackTextures;
    public Texture idleTexture;
    
    [SerializeField] private float animationSpeed = 0.03f;
    [SerializeField] private float turnSlowFactor = 0.5f;

    private Renderer leftTrackRenderer;
    private Renderer rightTrackRenderer;
    
    private int currentLeftTextureIndex = 0;
    private int currentRightTextureIndex = 0;
    private float leftTimer = 0f;
    private float rightTimer = 0f;
    private bool isMovingForward = false;
    private bool isMovingBackward = false;
    private bool isTurningRight = false;
    private bool isTurningLeft = false;

    void Start()
    {
        Transform leftTrack = transform.Find("left");
        Transform rightTrack = transform.Find("right");
        
        if (leftTrack != null)
        {
            leftTrackRenderer = leftTrack.GetComponent<Renderer>();
        }
        
        if (rightTrack != null)
        {
            rightTrackRenderer = rightTrack.GetComponent<Renderer>();
        }
        
        if (leftTrackRenderer != null && idleTexture != null)
        {
            leftTrackRenderer.material.mainTexture = idleTexture;
        }
        
        if (rightTrackRenderer != null && idleTexture != null)
        {
            rightTrackRenderer.material.mainTexture = idleTexture;
        }
    }

    void Update()
    {
        HandleInput();
        AnimateTracks();
    }

    private void HandleInput()
    {
        bool wPressed = Input.GetKey(KeyCode.W);
        bool sPressed = Input.GetKey(KeyCode.S);
        bool aPressed = Input.GetKey(KeyCode.A);
        bool dPressed = Input.GetKey(KeyCode.D);

        isMovingForward = wPressed && !sPressed;
        isMovingBackward = sPressed && !wPressed;
        isTurningRight = dPressed && !aPressed;
        isTurningLeft = aPressed && !dPressed;
        
        if (!wPressed && !sPressed && !aPressed && !dPressed)
        {
            if (leftTrackRenderer != null && idleTexture != null)
            {
                leftTrackRenderer.material.mainTexture = idleTexture;
            }
            
            if (rightTrackRenderer != null && idleTexture != null)
            {
                rightTrackRenderer.material.mainTexture = idleTexture;
            }
            return;
        }
    }

    private void AnimateTracks()
    {
        if (leftTrackRenderer == null || leftTrackTextures == null || leftTrackTextures.Length == 0)
            return;
        if (rightTrackRenderer == null || rightTrackTextures == null || rightTrackTextures.Length == 0)
            return;
        if (!isMovingForward && !isMovingBackward && !isTurningRight && !isTurningLeft)
            return;

        leftTimer += Time.deltaTime;
        rightTimer += Time.deltaTime;
        
        float speedRightMultiplier = 1f;
        float speedLeftMultiplier = 1f;
        bool reverseRightAnimation = false;
        bool reverseLeftAnimation = false;
        
        if (isMovingForward)
        {
            if (isTurningRight)
            {
                speedLeftMultiplier = 1f;
                speedRightMultiplier = turnSlowFactor;
            }
            else if (isTurningLeft)
            {

                speedLeftMultiplier = turnSlowFactor;
                speedRightMultiplier = 1f;
            }
            else
            {
                speedLeftMultiplier = 1f;
                speedRightMultiplier = 1f;
            }
        }
        else if (isMovingBackward)
        {
            if (isTurningRight)
            {
                speedLeftMultiplier = turnSlowFactor;
                speedRightMultiplier = 1f;
            }
            else if (isTurningLeft)
            {
                speedLeftMultiplier = 1f;
                speedRightMultiplier = turnSlowFactor;
            }
            else
            {
                speedLeftMultiplier = 1f;
                speedRightMultiplier = 1f;
            }
            reverseRightAnimation = true;
            reverseLeftAnimation = true;
        }
        else
        {
            if (isTurningRight)
            {
                speedLeftMultiplier = 1f;
                speedRightMultiplier = turnSlowFactor;
                reverseRightAnimation = true;
            }
            else if (isTurningLeft)
            {
                speedRightMultiplier = 1f;
                speedLeftMultiplier = turnSlowFactor;
                reverseLeftAnimation = true;
            }
        }

        if (leftTimer >= animationSpeed / Mathf.Abs(speedLeftMultiplier))
        {
            leftTimer = 0f;
            if (!reverseLeftAnimation)
            {
                currentLeftTextureIndex++;
                if (currentLeftTextureIndex >= leftTrackTextures.Length)
                    currentLeftTextureIndex = 0;

            }
            else
            {
                currentLeftTextureIndex--;
                if (currentLeftTextureIndex < 0)
                    currentLeftTextureIndex = leftTrackTextures.Length - 1;
            }
            
            leftTrackRenderer.material.mainTexture = leftTrackTextures[currentLeftTextureIndex];
        }
        
        if (rightTimer >= animationSpeed / Mathf.Abs(speedRightMultiplier))
        {
            rightTimer = 0f;
            if (!reverseRightAnimation)
            {
                currentRightTextureIndex++;
                if (currentRightTextureIndex >= rightTrackTextures.Length)
                    currentRightTextureIndex = 0;
            }
            else
            {
                currentRightTextureIndex--;
                if (currentRightTextureIndex < 0)
                    currentRightTextureIndex = rightTrackTextures.Length - 1;
            }

            rightTrackRenderer.material.mainTexture = rightTrackTextures[currentRightTextureIndex];
        }
    }
}