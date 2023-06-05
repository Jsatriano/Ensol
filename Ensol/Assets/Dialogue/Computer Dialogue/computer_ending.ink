EXTERNAL endingOne()
EXTERNAL endingTwo()
->start

===start===
It doesn’t take long to unlock the computer. Taking a quick glance at its contents, it’s clear that this is the master computer. Multiple programs are running on it that actively control each variation of animal models and environmental settings.
->start2

===start2===
As you browse the programs, several notifications appear with requests made from P1-USH models. You witness several programs activate automatically to help analyze and complete each request.
->start3
===start3===
Delving further in, you realize that you could use this computer to shut it all down easily, ending this.

*[Access the system shutdown]
<u>A pop up appears</u>.
->start4
===start4===
If you are reading this, please wait. I was one of the engineers working on the Paradise project before the system took over and put everyone in habitat domes. At first, I intended to rectify our mistake and shut it down. I managed to bypass the barriers the computer placed to stop me, much like you did, but when I got here I had second thoughts.

->start5
===start5===
Even though it’s out of our control, the Paradise project is working! People are safe and happy, even if they can’t leave. And who am I to decide to take that away from them? Instead, I’ve decided it would be best to go back and wipe my memories, so I can have the peace of ignorance and hopefully have that same happiness.
->start6


===start6===
I found the file for the cloning tubes and the panel that hides the memory restoration module. I’m pinning it here so you can use it to do the same if you like.
-> ending_choices



===ending_choices===
*[Shut down the system]
~endingOne()
-> Ending_shut_down
*[Leave it running]
~endingTwo()
->Ending_erase_memories

===Ending_shut_down===
->END

===Ending_erase_memories===
->END