INCLUDE ../globals.ink
{ river_controls == true: -> off}


-> Intro

===Intro ===

A control panel that controls the flow of the river. You attempt to turn off the river.


The terminal denies your access, flashing the error message: “Denizen’s are not permitted to modify environmental settings.”

*[Break the control panel.]
->2nd



===2nd===
You thrust your weapon into the control panel, breaking it.
~river_controls = true

->END

===off===
The control panel for the river is broken. The river is no longer flowing <b>north</b>.
->END
