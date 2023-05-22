INCLUDE ../globals.ink

{ deerdead == true: ->Intro3}
{ deadtodeer == true: -> After_death}
{ abletoleave == false: -> Intro | -> Over}


=== Intro ===
???: Ah, finally! That took a lot longer than usual. I was getting a bit worried.

->Dialogue_options



===Dialogue_options===
*[What is going on? Where am I?]
{abletoleave:P1-USH|???}: What do you mean? You’re back home! Would you like me to start off the day with your morning tunes? How about your favorite game?
~ abletoleave2 = true

->Dialogue_options


*[ Who are you? What are you?]
P1-USH: You don’t remember me? I’m P1-USH, your at-home entertainment and social system! Would you like me to start off the day with your morning tunes? How about your favorite game?
~ abletoleave = true
->Dialogue_options


* {abletoleave && abletoleave2} [I just want to walk around for a bit.]
P1-USH: Okay! If you need anything, let me know.
-> END

=== Over ===
P1-USH: Hello Friend! If you need anything else, feel free to let me know.

-> END

VAR body = false
VAR death = false

===After_death===
P1-USH: Hello Friend! I’m glad you’re back.
->After_death_options
===After_death_options===
*[How am I still alive?]
P1-USH: Still don’t remember? With that cloning machine right over there you don’t ever have to worry about dying. Using samples of your DNA from your previous body, it can replicate you along with all your memories. I’m not sure why you didn’t keep your memories last time
~body = true
->After_death_options

*{body} [What do you mean by my old body?]
P1-USH: As in your old body! It gets cleaned up while I wait for you to wake up, so you don’t need to worry about it. Anything you had with you will be set aside for you to pick back up.
->After_death_options

*[What was that outside?]
P1-USH: Oh! That was a power-gathering model. Of course, all the units outside are also programmed with the retrieval protocol, to ensure you stay safe in your home.
~death = true
->After_death_options

*{death}[That deer killed me! Is that the retrieval protocal?]
P1-USH: It did, and it is. The retrieval protocol allows the unit to bring you back here in the fastest manner possible, so that you could be cloned back home in good health.
->After_death_options

*[Can I have a weapon?]
P1-USH: A weapon?! Why would you need a weapon? It’s perfectly safe here in the cabin. I don’t want you to hurt yourself with something dangerous.
 ->After_death_options
 
*[I need to get going.]
P1-USH: Sure thing, but please don't go into the forest again!
~deadtodeer = false
~abletoleave = true
->END






===Intro3===

P1-USH: Hey friend! Is there something you need? Some tunes? How about your favorite game?
->Plush_unlock

===Plush_unlock===

+[I want to know more about one of the units in the forest.]
P1-USH: Sure thing! 
->forest_questions
+{footprints_found}[I want to know more about something else in the forest.]
P1-USH: Sure thing!
->forest_other
+[I want to ask something about you.]
P1-USH: Of course! 
->plush_questions


+[I don't need anything else.]
P1-USH: Okay! If you ever need anything else, feel free to let me know!
->END

===Plush_talk===
P1-USH: Anything else I can help you with friend?
->Plush_unlock


===forest_questions===
+[What is the purpose of those deer robots?]
P1-USH: Oh! That was a power-gathering model. They are quick and efficent, but also pretty sturdy! They are used to gather solar power and then transfer that power into other machines.
-> forest_questions

+{rabbit_deafeated} [Why are there robots in the shape of a bunny?] 
P1-USH: Those are the gardening models. The bunny design, while small in stature, allows the robot easy access to both small and large areas of flora that need attention. Typically, they are sent out in packs to maximize efficiency.
->forest_questions

+{bear_defeated}[(More Options.)]
P1-USH: Anything else?
->forest_questions_v2

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock

===plush_questions===
*{dear_defeated}[ Why do you keep staring at my equipment.]
P1-USH: Oh, sorry! It’s just not every day I get to see a solar panel shaped like that. Those edges look a bit sharp so please be careful! I’m glad you’re getting back into your building hobby though, it’s always a good idea to reinvigorate the mind! 
    ->plush_questions
    
+[Can you play my favorite tune? (Not working)]
P1-USH: Sure! 
    ->plush_questions
    
+[Can you play my favorite game? (Not working)]
P1-USH: Sure! 
    ->plush_questions
    
+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock

===forest_questions_v2===
+{bear_defeated}[The bear robots, what are they?]
P1-USH: That’s the trash-collecting model. It uses a combination of powerful magnets with its strong arms to gather and collect trash onto its back. Wouldn’t want to be in front of a trash-collecting model while it's working.
    ~bearSeen = true
->forest_questions

+{spider_defeated} [Those spider robots, what were they used for?]
P1-USH: They are used to make powerlines effectively. They’re kept in that area specifically and are rarely sent out anywhere else. They can climb walls extremely well, and send out powerlines from a great distance. Be careful when they’re working as you wouldn’t want one to fall on you, they’re heavy.
->forest_questions

+[(Previous Options.)]
P1-USH: Anything else?
->forest_questions

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock

===forest_other===
+{footprints_found}[Are there robots that leave behind human footprints?]
P1-USH: None of our designs uses any human feet. Are you sure you haven’t looped back to the area before? Those woods can be pretty confusing for a human to navigate through successfully without being confused.
    ->Plush_talk

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock