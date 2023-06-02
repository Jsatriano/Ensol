INCLUDE ../globals.ink

{ deerdead == true: ->Intro3}
{ deadtodeer == true: -> After_death}
{ abletoleave == false && deadtodeer == false: -> Intro | -> Over}


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
P1-USH: Still don’t remember? With that cloning machine right over there you don’t ever have to worry about dying. Using samples of your DNA from your previous body, it can replicate you along with all your memories. I’m not sure why you didn’t keep your memories last time.
~body = true
->After_death_options

*{body} [What do you mean by my previous body?]
P1-USH: As in your previous body! It gets cleaned up while I wait for you to wake up, so you don’t need to worry about it. Anything you had with you will be set aside for you to pick back up.
->After_death_options

*[What was that outside?]
P1-USH: Oh! That was a power-gathering model. Of course, all the units outside are also programmed with the retrieval protocol, to ensure you stay safe.
~death = true
->After_death_options

*{death}[That deer killed me! How is that keeping me safe?]
P1-USH: The retrieval protocol allows the unit to bring you back here in the fastest manner possible, so that you can be cloned back home safe and in good health.
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
P1-USH: Oh! That was a power-gathering model. They are quick and efficient, but also pretty sturdy! They gather solar power, then transfer that power to other machines.
-> forest_questions

+{rabbit_deafeated}[Why are there robots in the shape of a bunny?]
P1-USH: Those are the gardening models. The bunny design, while small in stature, allows easy access to both small and large areas of flora that need attention. They are typically sent out in packs to maximize efficiency.
-> forest_questions


+{bear_defeated}[(More Options.)]
P1-USH: Anything else?
->forest_questions_v2

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock

===plush_questions===
*{dear_defeated}[ Why do you keep staring at my equipment.]
P1-USH: It’s not every day I see a solar panel shaped like that. Those edges look a bit sharp so please be careful! I’m glad you’re getting back into your building hobby though, it’s always a good idea to reinvigorate the mind! 
    ->plush_questions
    
+[Can you play my favorite tune? (Not working)]
P1-USH: Sure! 
    ->plush_questions
    
+[Can you play my favorite game? (Not working)]
P1-USH: Sure! 
    ->plush_questions
    
+{dear_defeated == false}[Can I pet you?(Not working)]
P1-USH: Absolutely! 
    ->plush_questions

+{dear_defeated} [(More Options.)]
    ->Plush_unlock_2
+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock

===Plush_unlock_2===
+[Can I pet you?(Not working)]
P1-USH: Absolutely! 
    ->plush_questions

*{dear_defeated}[ Why do you keep staring at my equipment.]
P1-USH: It’s not every day I see a solar panel shaped like that. Those edges look a bit sharp so please be careful! I’m glad you’re getting back into your building hobby though, it’s always a good idea to reinvigorate the mind! 
    ->plush_questions


+[(Previous Options.)]
P1-USH: Anything else?
->forest_questions

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock





===forest_questions_v2===
+{bear_defeated}[The bear robots, what are they?]
P1-USH: That’s the recycling model. It uses a combination of powerful magnets and industrial arms to gather refuse in its back for later recycling. Wouldn’t want to be in front of one while it's working.
    ~bearSeen = true
->forest_questions

+{spider_defeated} [Those spider robots, what were they used for?]
P1-USH: Those are power-line models. They can build and climb infrastructure to erect and maintain power lines that cover great distances. They’re usually only found in power grids, so I’m not sure how you saw one.
->forest_questions

+[(Previous Options.)]
P1-USH: Anything else?
->forest_questions

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock

===forest_other===
+{footprints_found}[Are there robots that leave human footprints?]
P1-USH: None of the models have human designs. Are you sure you haven’t been to the area before? Those woods can be pretty confusing, but that’s why it’s important to stay here.
    ->Plush_talk
    
*{repair_used}[What are those metal pads in the forest?]
P1-USH: Oh! You might be talking about the repair stations. Those stations use nanobots to repair any organic and inorganic material! They are out in the open so that any damaged model can get repairs easily.
    ->Plush_talk
+{river_controls}[(More Options.)]
 ->forest_other2

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock


===forest_other2===
+{river_controls}[Why was I called a denizen? What is this place exactly?]
P1-USH: It's because denizens are denizens silly! This place is meant to take care of you in the best way possible, so don't you worry about a thing. 
    ->Plush_talk
+{scrap_change or grass_change or lighting_change}[Have you noticed something different?]
P1-USH: Oh? About what?
->environment_change
+[(Previous Options.)]
P1-USH: Anything else?
->forest_other

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock

===environment_change===
+{scrap_change}[There is trash everywhere]
P1-USH: The recycling models are not picking those up? More should be made then. Hopefully the other models are fine, wouldn't want them to be breaking down.
    ->environment_change
+{grass_change}[The grass is growing really tall]
P1-USH: Oh no! The gardening models should have trimmed the grass before they reached those levels. Hopefully new ones get made to help out.
    ->environment_change
+{lighting_change}[It’s getting darker, but not that much time has passed]
P1-USH: Strange. The environment should be getting plenty of energy from the power-gathering models. Maybe something is wrong with the system? Hopefully it's not the power-gathering models themselves, that could be detrimental to everything!
    ->environment_change

+[I want to ask about something else.]
P1-USH: Okay!
-> Plush_unlock
