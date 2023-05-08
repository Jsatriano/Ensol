INCLUDE ../globals.ink
{ river_controls == true: -> off}


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
The control panel for the river is broken. The river is no longer flowing <b>north</b>.
->END
