# Documentation and Examples for the Metadata of Synapse Images
## New Combined Synapse Image of all Inputs
- `combined_synapse.json` file: combined synapse metadata
    - `NumberOfSources`: an integer denoting the total number of source media used to create the synapse. A video clip is considered as one individual source, and an **entire folder containing stills** is considered as a single source
    - `TotalWidth`: an integer denoting the resulting width of the combined synapse image
    - `SynapseBoundries`: an array of integers, each denoting the starting x-axis coordinate of the sub-synapse of a single source media
    - `IndividualSources`: an array of JSON objects, each containing the metadata of a sub-synapse from a single source media, in the same order as `SynapseBoundries`
        - `SourceType`: a string of either `video` or `stills`
        - `SourceFileName`: `null` for stills synapse, or a string denoting the name of the source video clip for video synapse
        - `ImageWidth`: an integer denoting the width of the current sub-synapse image
        - `NumberOfScenes`: an integer, 0 for stills, or the number of scenes for video
        - `NumberOfShots`: an integer, 0 for video, or the number of shots included from the given stills collections
        - `SceneStartFrameNumbers`: an array of integers, `null` for stills, or the starting frame number of each scene for video
        - `ShotFileNames`: an array of strings, `null` for video, or the filenames of individual still images chosen from the collection for stills