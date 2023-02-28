VAR body = false
VAR death = false
-> Intro

=== Intro ===
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
