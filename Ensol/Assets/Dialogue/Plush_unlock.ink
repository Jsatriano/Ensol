VAR dear_defeated = false
VAR footprints_found = false
VAR bear_defeated = false

-> Intro



===Intro===

Hey friend! Is there something you need? Some tunes? How about your favorite game?
->Questions

===Questions===

*{dear_defeated}[ You keep staring at my equipment.]
Plush: Oh, sorry! It’s just not every day I get to see a solar panel shaped like that. Those edges look a bit sharp so please be careful! I’m glad you’re getting back into your building hobby though, it’s always a good idea to reinvigorate the mind!
    ->Questions

*{footprints_found}[Are there robots that leave behind human footprints?]
Plush: None of our designs uses any human feet. Are you sure you haven’t looped back to the area before? Those woods can be pretty confusing for a human to navigate through successfully without being confused.
    ->Questions

*{dear_defeated}[The bear robots, what are they?]
Plush: You must mean the trash-collecting model. It uses a combination of powerful magnets with its strong arms to gather and collect trash onto its back.
    ->Questions

+[I don't need anything else.]
->DONE

