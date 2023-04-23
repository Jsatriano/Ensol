INCLUDE globals.ink
VAR deadtodeer = false
VAR deerdead = false

{ deerdead == true: ->Intro3}
{ deadtodeer == true: -> Intro2}
{ abletoleave == false: -> Intro | -> Over}


=== Intro ===
???: Ah, finally! That took a lot longer than usual. I was getting a bit worried.

->Dialogue_options



===Dialogue_options===
*[What is going on? Where am I?]
???: What do you mean? You’re back home! Would you like me to start off the day with your morning tunes? How about your favorite game?
->Dialogue_options


*[ Who are you? What are you?]
P1-USH: You don’t remember me? I’m P1-USH, your at-home entertainment and social system! Would you like me to start off the day with your morning tunes? How about your favorite game?
~ abletoleave = true
->Dialogue_options


* {abletoleave} [Just need to walk around for a little.]
P1-USH: Okay! If you need anything let me know.
-> END

=== Over ===
P1-USH: Hello Friend! If you need anything else, feel free to let me know.

-> END

VAR body = false
VAR death = false
-> Intro

=== Intro2 ===
Plush: Hello Friend! I’m glad you’re back.
 -> Intro_Options
 
 
 
===Intro_Options===

*[How am I still alive?]
Plush: Still don’t remember? With that cloning machine right over there you don’t ever have to worry about dying. Using samples of your DNA from your previous body, it can replicate you along with all your memories. I’m not sure why you didn’t keep your memories last time
~body = true
->Intro_Options

*{body} [My old body?]
Plush: Yup! It gets cleaned up while I wait for you to wake up, so you don’t need to worry about it. Anything you had with you will be set aside for you to pick back up.
->Intro_Options

*[What was that outside?]
Plush: Oh! That was a power-gathering model. Of course, all the units outside are also programmed with the retrieval protocol, to ensure you stay safe in your home.
~death = true
->Intro_Options

*{death}[It killed me!]
Plush: It did. Allowing it to bring you back here in the fastest manner possible, so that you could be cloned back home in good health.
->Intro_Options

*[Can I have a weapon?]
A weapon?! Why would you need a weapon? It’s perfectly safe here in the cabin. I don’t want you to hurt yourself with something dangerous.
 ->Intro_Options
 
*[I should get up.]
PLush: Please don't go outside again!
->DONE




-> Intro



===Intro3===

P1-USH: Hey friend! Is there something you need? Some tunes? How about your favorite game?
->Questions

===Questions===

*{dear_defeated}[ You keep staring at my equipment.]
P1-USH: Oh, sorry! It’s just not every day I get to see a solar panel shaped like that. Those edges look a bit sharp so please be careful! I’m glad you’re getting back into your building hobby though, it’s always a good idea to reinvigorate the mind!
    ->Questions

*{footprints_found}[Are there robots that leave behind human footprints?]
P1-USH: None of our designs uses any human feet. Are you sure you haven’t looped back to the area before? Those woods can be pretty confusing for a human to navigate through successfully without being confused.
    ->Questions

*{bear_defeated}[The bear robots, what are they?]
P1-USH: You must mean the trash-collecting model. It uses a combination of powerful magnets with its strong arms to gather and collect trash onto its back.
    ->Questions

+[I don't need anything else.]
->END