INCLUDE globals.ink
EXTERNAL openDoor()
{ window_seen == true && conveyer_seen == true && cloning_seen == true: -> Over | -> Intro}

-> Intro

===Intro ===

 It would be a good idea to see what you can recall from around the cabin before going outside.
 -> END
 
 === Over ===
Plush: Wait! Do you remember what’s out there?


*[What’s out there?]

 Plush: You shouldn’t be out in the forest. You should stay here where it’s safe. If you need anything, you can let me know and the cabin can make it.
 ->2nd_part

===2nd_part ===
~openDoor()
I see…

-> END