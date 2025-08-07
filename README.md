## Table of Contents
- [Overview](#overview)
- [Dependencies](#dependencies)
- [Compatibility](#compatibility)
- [Installation](#installation)
- [How To Use](#how-to-use)
- [Testing](#testing)
- [License](#license)

## Overview

This package (`com.orbitalnine.core`) provides foundational building blocks for common Unity development patterns:

-   **Singleton**: Create a `MonoBehaviour`-based singleton by inheriting from `SingletonMonoBehaviour<T>`.
-   **Persistence**: The `DontDestroyOnLoad` component provides a way to keep any GameObject alive across scene loads.
  
## Dependencies

This package has no external dependencies and relies only on the core Unity Engine libraries.

## Compatibility
Important: This package has only been tested on Unity version 6.1 for Editor, iOS and Android. Compatibility with other Unity versions and platforms is not guaranteed.

## Installation

### Using Unity Package Manager (recommended)

You can install this package in Unity's Package Manager using the "Add package from git URL" option.

1.  Open the Package Manager (`Window > Package Manager`).
2.  Click the `+` button in the top-left corner.
3.  Select "Add package from git URL...".
4.  Enter the repository's HTTPS URL (`https://github.com/shirleyman/unity-core.git`) and click "Add".

### Manual Installation

1. Clone or download this repository.
2. Copy the `Runtime` and `Tests` (optional) folders into your Unity project's `Assets` or `Packages` directory.

## How To Use

### SingletonMonoBehaviour

To create your own singleton manager (e.g., a `GameManager`), create a new C# script and have it inherit from `SingletonMonoBehaviour<T>`, where `T` is your new class.

**Example: Creating a GameManager**
```csharp
// Create a new file: GameManager.cs
using UnityEngine;

// Inherit from SingletonMonoBehaviour, passing your class as the type.
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public void DoSomething()
    {
        Debug.Log("GameManager is doing something!");
    }
}
```

**Example: Accessing the Singleton**

You can now access the `GameManager` instance from any other script in your project.

```csharp
using UnityEngine;

public class MyOtherScript : MonoBehaviour
{
    void Start()
    {
        // Access the single instance of GameManager and call a public method.
        GameManager.Instance.DoSomething();
    }
}
```

### DontDestroyOnLoad

To make a GameObject persist across different scenes:

1.  Select the GameObject you want to keep in your scene hierarchy.
2.  In the Inspector, click "Add Component".
3.  Search for `DontDestroyOnLoad` and add it.

This GameObject will now not be destroyed when a new scene is loaded.

## Testing

This package includes a suite of tests for the `SingletonMonoBehaviour` and `DontDestroyOnLoad` components. You can run these tests using the Unity Test Runner window (`Window > General > Test Runner`).

## License

This project is licensed under the terms of the license specified in the [LICENSE](LICENSE) file.

## Featured In
* Used in [Brain It On! (iOS, Android) by Orbital Nine Games](https://orbitalnine.com) for singleton managers, as well as dependencies for other Orbital Nine packages.
