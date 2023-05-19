INCLUDE ../globals.ink

{ river_controls == false: -> Intro | -> Over}

-> Intro

===Intro ===
Boot footprints are visible in the mud on the bank of the river. They lead directly into the water, then reappear coming out on the other side. The water is moving too fast for you to attempt to cross it. <u>The source seems to be coming from the <b>south</b></U>.
    ~footprints = true
    ~footprints_found = true

-> END

=== Over ===
Boot footprints are imprinted in the mud.

-> END
