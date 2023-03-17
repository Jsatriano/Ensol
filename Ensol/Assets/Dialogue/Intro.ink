INCLUDE globals.ink

{ abletoleave == false: -> Intro | -> Over}

-> Intro

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

