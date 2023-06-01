INCLUDE ../globals.ink
EXTERNAL openDoor()
{ window_seen == true && conveyer_seen == true && cloning_seen == true: -> Over | -> Intro}

-> Over

===Intro ===
 It would be a good idea to see what you can recall from around the cabin before going outside. ({interactables_seen}/3 items have been discovered).

 -> END
 
 === Over ===
P1-USH: You’re not going out into the forest again are you?


*[What’s out there?]
 P1-USH: Nothing to worry about, but you shouldn’t be out in the forest. You should stay here where it’s safe.
~openDoor()
->2nd_part

===2nd_part ===
 P1-USH: If you need anything, you can let me know and the cabin can make it.
*[I just need some fresh air.]
P1-USH: Okay. Stay safe!
 -> END

