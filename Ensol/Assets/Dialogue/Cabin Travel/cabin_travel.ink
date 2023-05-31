INCLUDE ../globals.ink

{checkpointscount ==0:->  nounlocks| ->Hub}


===nounlocks===
An opening leading deep underground that leads into a tunnel system. The passages seems crudely made, as if someone manualy dug them out. Following these tunnels lead into blocked exits from the otherside.  
    -> END

===Hub===
An opening leading deep underground that leads into a tunnel system. The passages seems crudely made, as if someone manualy dug them out. Following these tunnels lead into blocked exits from the otherside. 
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