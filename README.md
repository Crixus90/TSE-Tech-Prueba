# TSE-Tech-Prueba

This repo contains a technical test for a Unity developer role. 


## Requirements

- Unity 2019.4.5f1
- Build Platform: Android (Min API level 23)
- Scripting backend: IL2CPP

## Features

- Requests Android location permissions
- Editor Scene manager window
- Editor Cube Spawner

## Improvements

- Using callbacks would be a better approach for requesting permissions (not supported in Unity 2019)
- Use `MaterialPropertyBlock` for each new instantiated prefab rather than creating new material each time. 
