INCLUDE ../globals.ink
EXTERNAL hackRiver()
{ river_controls == true: -> off}


-> Intro

===Intro ===

A control panel that controls the flow of the river. There are options displayed for water flow and purity.

*[Turn off the water.]
->2nd



===2nd===
The terminal denies your access, flashing the error message: “Denizen’s are not permitted to modify environmental settings.”

*[Short circuit the control panel.]
~hackRiver()
->3rd
~river_controls = true

->END

===3rd===
<u>The water is no longer flowing to the north</u>.
->END
~riveroff = true

===off===
The control panel for the river is broken. The river is no longer flowing <u><b>north</b></u>.
->END
