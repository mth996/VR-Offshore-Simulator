# VR Offshore Simulator

An immersive **industrial safety and offshore training simulation** developed in Unity, focused on hands-on VR interaction, PPE procedures, hazard-response workflows, equipment operation, and embodied XR interaction.

> **Portfolio Project:** Industrial XR / VR Training Simulation

## Overview

Developed for **InfoSoft Solution Group** as an industrial VR training simulation focused on offshore safety procedures, hazard awareness, PPE compliance, and interactive equipment training.

VR Offshore Simulator recreates interactive offshore training scenarios where the player can perform safety-oriented tasks inside a virtual industrial environment. The project combines procedural task logic with physical VR interactions and an embodied player system to create a practical training experience rather than a passive walkthrough.

The implementation includes systems for hazard progression, PPE handling, welding interactions, valve operation, task validation, XR avatar embodiment, and safety/failure events.

## Core Systems

### XR Avatar IK & Player Embodiment
The simulator includes a dedicated XR avatar system designed to represent the player's tracked movement through a full-body character. It combines XR-origin following with avatar IK and supporting character-positioning logic.

**Selected scripts:** `XRAvatarIK.cs`, `AvatarFollowXROrigin.cs`, `OfflinePlayerAvatar.cs`, `FreezeMixamoHips.cs`

### Hazard Training System
A structured hazard-management system controls safety scenarios and progression through hazard-related training steps.

**Selected scripts:** `HazardManager.cs`, `HazardStep.cs`, `ExplosionFailSafe.cs`

### PPE System
The PPE workflow represents safety equipment as wearable/interactable objects and connects equipment state with the training experience.

**Selected scripts:** `PPEManager.cs`, `PPEItemWearable.cs`, `PPEType.cs`, `PPEUIController.cs`, `WearPoint.cs`

### Industrial Equipment Interaction
Dedicated interaction logic supports industrial equipment tasks including valve operation and welding.

**Selected scripts:** `ValveRotation.cs`, `AutoValveRotator.cs`, `ValveAutoTrigger.cs`, `WeldZone.cs`

### Task & UI Progression
Supporting systems manage task zones, validation, interface states, and progression through the training workflow.

**Selected scripts:** `TaskZone.cs`, `EnableButtonWhenAllTogglesOn.cs`, `UIPanelSwitcher.cs`, `SceneChangeTriggerSimple.cs`

## Technical Highlights

- Full-body **XR Avatar IK / player embodiment**
- Modular **hazard and safety scenario management**
- Interactive **PPE workflow**
- VR **valve operation mechanics**
- Interactive **welding task zones**
- Safety and failure-event handling
- Task validation and progression logic
- XR-oriented UI and scene-flow systems

## Technology Stack

- **Unity 6.2** — 6000.2.10f1
- **C#**
- **Universal Render Pipeline (URP) 17.2.0**
- **XR Interaction Toolkit 3.2.1**
- **OpenXR 1.15.1**
- **Unity XR Management 4.5.3**
- **Oculus XR Plugin 4.5.2**
- **Android XR OpenXR 1.0.2**
- **Animation Rigging 1.3.1**
- **Unity Input System 1.14.2**

## My Role

I worked on the Unity development and implementation of the simulator's interactive VR systems, including XR interaction, player embodiment, task progression, safety mechanics, hazard logic, and industrial equipment interactions.

The project demonstrates my work in **industrial XR development**, with an emphasis on translating real-world training activities into interactive VR systems.

## Repository Structure

```text
VR-Offshore-Simulator/
├── Scripts/               # Selected C# systems used in the simulation
├── manifest.json          # Unity package configuration
├── ProjectVersion.txt     # Unity editor version
└── README.md              # Portfolio documentation
```

## Media

Screenshots and demonstration footage will be added here to document the offshore environment, PPE workflow, hazard scenarios, avatar embodiment, and industrial interactions.
