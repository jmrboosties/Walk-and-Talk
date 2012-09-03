This is a game engine I'm working on. You can just walk around a map and talk using a global chatbox.

I'm using some Pokemon spritesheets for fun. I don't own it though, I'm sorry Shigeru. If you want me to
stop using Pokemon sprites please send me a signed copy of Mario Teaches Typing 2.

TODO LIST:

 - Server-side collision detection. The client will have collision detection too just to reduce the # of 
unnecessary signals to server and give client a faster feel. The tricky part about server-side collision
detection is that collision detection is handled currently by parsing a Tiled map which is saved in an
XML format and turned into a map object. Doing this requires a content manager which isn't really used 
on console applicatons (which the server is). Looking into different solutions...

 - Get the players to display their proper names on client-side.
 
 - Include some kind of identifier for which character sprite to use in the PlayerPrimitive class.
 
 - General clean up of any retard code where you see it.