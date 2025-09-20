# Parking OpenGL Project

This project was developed as part of my Computer Science degree at HIT.  
It is a 3D parking lot simulation built with **C#** and **OpenGL**, featuring cars, gates, traffic lights, textures, reflections, and shadows.  
The application provides an interactive WinForms interface to control and animate the parking environment.

---

## Technologies
- Language: C#  
- Graphics API: OpenGL  
- Framework: WinForms (Windows Forms)  
- 3D Models: Milkshape (`.ms3d`)  
- Project Type: .NET Framework  

---

## Features
- 3D Parking Lot Simulation with interactive objects  
- Cars: Animated along predefined routes with adjustable speed  
- Gate: Open/close animation  
- Traffic Light: Automatic and manual modes  
- Lighting: Point lights, global ambient light, adjustable intensity and position  
- Textures & Materials: Enable/disable textures, choose between Gold, Emerald, Chrome  
- Camera Control: Rotate, zoom, pan; switch between Perspective and Orthogonal projection  
- Reflections & Shadows: Floor reflections and dynamic shadows using stencil buffer  
- Scene Save/Load: Save the current scene to a `.parking` file and reload it later  

---

## Controls
- Mouse:  
  - Drag = rotate scene  
  - Ctrl + Drag = pan scene  
  - Wheel = zoom in/out  

- Keyboard:  
  - Arrow keys = move camera (pan)  
  - P = toggle Perspective/Orthogonal projection  
  - T = enable/disable textures  

- UI Controls (right panel):  
  - Lighting: Toggle lights, adjust positions  
  - Objects: Scale, rotate, and move car, gate, traffic light  
  - Animation: Start cars, open/close gate, adjust car speed  
  - Textures: Enable/disable and choose material  
  - View: Change projection, adjust zoom  
  - Scene: Reset view, adjust rotation/translation manually  
  - File: Save or load scene  

---

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/moriyash/ParkingOpenGL_Project.git
