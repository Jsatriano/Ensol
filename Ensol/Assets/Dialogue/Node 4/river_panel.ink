INCLUDE ../globals.ink
{ river_controls == false: -> Intro | -> Over}


-> Intro

===Intro ===

A control panel that controls the flow of the river.

*[Turn off the water flow.]
->2nd



===2nd===
The terminal denies your access, flashing the error message: “Denizen’s are not permitted to modify environmental settings.”
~river_controls = true

->END

===off===
The control panel for the river is broken.
->END
