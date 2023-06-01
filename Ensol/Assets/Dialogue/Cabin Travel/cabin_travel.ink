INCLUDE ../globals.ink

{checkpointscount ==0:->  nounlocks| ->Hub}


===nounlocks===
An opening going deep underground and into a service tunnel system. It’s not safe to travel through here when you don’t know where it leads to.
    -> END

===Hub===

An opening going deep underground and into a service tunnel system. You can travel through undetected if you know where you are going.
 
->Hub_options

===Hub_options===
+[Go to the first path.]
->DONE
+[Go to the second path.]
->DONE
+[More Options.]
->Hub2
+[Exit]
->DONE

===Hub2===

*[Go to the third path.]
->DONE

*[Go to the fourth path.]
->DONE

+[Previous Options.]
->Hub_options
+[Exit]
->DONE