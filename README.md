
## 1. New Input System

### Step 1: Import the Input System from Unity Registry
1. Go to the **Package Manager**.
2. Search for `Input System` and install it.

### Step 2: Create Input Actions
1. Right-click in the **Project** window.
2. Select **Create -> Input Actions**.
3. Define bindings and all the controls you need (e.g., Movement, Jump, etc.).
4. In the editor, **toggle the Generate Code** option.
   - This will auto-generate a script (e.g., `PlayerInputActions.cs`) which you can use in your own code.

### Step 3: Enabling Action Maps in Code
1. In your script, enable the action map:
   ```csharp
   playerInputActions.Player.Enable();
   ```

2. Assign callbacks (events) to trigger your actions:
   ```csharp
   playerInputActions.Player.Movement.performed += PlayerInputAction_Player_Movement;
   ```

3. Example of handling movement:
   ```csharp
   private void PlayerInputAction_Player_Movement(InputAction.CallbackContext context) {
       throw new NotImplementedException();
   }
   ```

This will continuously read input values. If no input is provided, it will return `(0,0)`, which can be used to detect the direction.

### Step 4: Switching Between Action Maps
If you want to switch between action maps (e.g., between Player and UI action maps), simply:

- Enable the desired action map:
  ```csharp
  playerInputActions.UI.Enable();
  ```

- Disable the current action map:
  ```csharp
  playerInputActions.Player.Disable();
  ```

### Step 5: Create Control Schemes
You can create multiple control schemes such as:

- **Keyboard/Mouse**
- **Gamepad**

Configure them within the Input Actions editor to allow flexibility in input methods.

---

## 11. Unity Shortcuts

Learn key shortcuts like `W` for moving objects, `E` for rotating, and using `Shift` while rotating to snap to 15-degree increments.

---

## 2. Singleton Pattern

Singleton ensures a class only has one instance globally, often used for managers or game controllers.

### Setup Singleton:
```csharp
public static Player Instance { get; private set; }

private void Awake() {
    if (Instance != null && Instance != this) {
        Destroy(gameObject);
        return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);
}
```

### Access Singleton:
```csharp
Player player = Player.Instance;
```

---