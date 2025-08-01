# The documentation of FQLab

Welcome to the full documentation of **FQLab** - a spectral audio sandbox written in C#.

This project was created as part of courses **NPRG035: Programming in C#** and **NPRG038: Advanced C# Programming** at Charles University.

---

## Core Features

- **Streaming audio playback with live modulation**
    - Change volume, EQ, effects all in real time.
- **Custom plugin support**
    - Code your own plugin to apply effects such as *reverb* or *distortion* to the audio chain.
- **Built-in EQ options**
    - Change the strength of the 3 main frequency ranges.
- **Live frequency graph**
    - Stunning visuals which help you better understand the decomposition of sound.

*All this packaged in a lightweight terminal only app.*

---

## Architecture

The core principle of the program is a 3 step pipeline:

- Fetching stage - gets samples from a file.
- Processing - applies effects, and other settings.
- Playing stage - takes processed samples and plays them on an audio source.

Visit [architecture.md](architecture.md) for more details.

## Dependencies

- `.NET 8.0`
- `Windows OS`
- `Terminal with unicode support`
- [`Terminal.Gui`](https://github.com/gui-cs/Terminal.Gui)
- [`Math.NET`](https://numerics.mathdotnet.com/)
- [`NAudio`](https://github.com/naudio/NAudio)

## User Guide

For a usage guide/walkthrough visit: [user_manual.md](user_manual.md)



