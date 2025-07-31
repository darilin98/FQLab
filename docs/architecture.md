# Project architecture overview

The core of this project is an *audio pipeline* separated into 3 parts:

    - Fetching samples -> Processing (applying settings and effects) -> Outputing modified samples

Above this pipeline exists a *GUI* which interacts with the *Processing* by changing its settings and visualizing its outputs.

## Fetching stage

### IAudioReader

Reads actual data from the audio file. 

Exposes metadata about the file - sample rate and channel count.

### IAudioStream 

Is created upon successful decoding of an audio file and creates an abstraction for the input. Can be consumed by calling `ReadFrame(int size)` at a desired pace.

Makes sure that all samples are in a set size of frames.

It also enforces the IDisposable contract and is therefore intended to cease to exist when the connected audio stops playing or is aborted.

## Audio Processing

### Audio Engine

The core class of the processing stage. Runs the entire pipeline.

Is created with the AudioStream and shares its lifespan with it.

Runs processes that consume data from a stream and add it to a player in parallel.

Uses and `IFftProcessor` to decompose sound and extract information about frequencies.

From there it exports to the `IFreqDataReceiver` if necessary.

Applies all *plugin effects*, *volume changes*, *eq settings* to the audio samples.

Processed samples get sent to the `IAudioPlayer`.


## Output stage

### IAudioPlayer

Receives samples from the audio engine and buffers them in order to output them at a steady pace.

Must be fed enough samples to prevent skips or distortion.

## GUI

### UIController

Runs the GUI logic and makes calls to the `AudioEngine` based on requests from `Views`.

### FreqViewDataProcessor

Implements the `IFreqDataReceiver` interface and accepts data from the `AudioEngine`.

It further processes this data so that the desired Views can render it straight away.

### FreqGraphView

Draws the current state of the frequency graph every couple of miliseconds.

---

### Plugins

Plugins get instantiated from a set folder via reflection thanks to the `IAudioPlugin` interface.

The 3rd party developer has access to normalized PCM samples and can edit them freely.

### Role of external libraries

1. `NAudio`
    - Used on Windows for decoding audio files in `IAudioReader` and playing PCM samples in `IAudioPlayer`.

2. `Math.NET`
    - `IFftProcessor` wraps around its Fourier transform to create frame-sized transformations.

3. `Terminal.Gui`
    - Takes care of the GUI part of the project, manages windows and input registering. Allows for graph drawing.

### Further possible expansions

The project is laid out in such a way that making it work cross-platform should be straightforward and painless.

Readers/Players are abstracted behind an interface. Linux/macOS support could work by just adding some more dependecies that decode audio files and connect to audio sinks.
