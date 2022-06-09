# Video Script/Sequence

Demonstrate each use case/req one-by-one

## Demonstration of Fulfilled Requirements

### Requirements
* FR1: D major scale (since C will not be able to be played in full)
* FR2: Note name        \
* FR3: Note freq         | can be done all at once
* FR4: Note on piano    /
* FR5: Recording Button
  * show all facets of this, including null recordings
* FR6: Recording Playback
* FR7: partial, show a chromatic scale from D5-D6 i guess
* FR8: partial, show the several Db alternates

### Use Cases
* Play Note: yes, no additional demo needed
* Adjust tempo: no, met not impl
* Enable met: no, "
* Disable met: no, "
* Close app: ... yeah
* Launch app: ... yeah that too i guess
* Playback track: yes, shown by FR6
* Toggle piano: oops forgot lol
* Stop recording: yeah, FR5
* Start Recording: yeah, FR5
* save to midi: placeholder

so it looks like all but the last UC are covered in the FRs

## Order of demonstration

### Physical demo

brief introduction

Note detection:
    FR2,FR3,FR4
    make note of the signifigance of the green color
    individual finger articulation
    fingerings without flute
    fingerings with flute



D major, D chromatic, Db alternates
    FR1,FR7,FR8

recording demos
    FR5,FR6
    perhaps integrate the previous section into this
    we might need a way to verify that the notes being produced are the correct ones
    comparatively with a real flute recording

and a note on the placeholder nature of the export button

### Code demo

note the fact that we are actually using MIDI to play stuff back

integrate jared's part into this as well
