INCLUDE ../globals.ink

{ river_controls == false: -> Intro | -> Over}

-> Intro

===Intro ===
Boot footprints are visible in the mud on the bank of the river. They lead directly into the water, then reappear on the other side. The water is moving too fast to attempt to cross it. <u>Perhaps its source can be found further south.</U>
    ~footprints = true
    ~footprints_found = true

-> END

=== Over ===
Boot footprints are imprinted in the mud.

-> END
