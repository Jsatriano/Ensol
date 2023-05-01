INCLUDE globals.ink
EXTERNAL openDoor()
{ window_seen == true && conveyer_seen == true && cloning_seen == true: -> Over | -> Intro}

-> Intro

===Intro ===
 It would be a good idea to see what you can recall from around the cabin before going outside. ({interactables_seen}/3 items have been discovered).

 -> END
 
 === Over ===
P1-USH: You’re not going out into the forest again are you?


*[What’s out there?]

 P1-USH: You shouldn’t be out in the forest. You should stay here where it’s safe. If you need anything, you can let me know and the cabin can make it.
 ->2nd_part

===2nd_part ===
~openDoor()
*[I need some fresh air.]
P1-USH: Don't go too far!
 -> END

