## SnipSnap

CTL + INS

One keypress screenshot -> Imgur.

SnipSnap will take a screenshot of your current foreground window, 
upload it to imgur, and shove the imgur link into your clipboard. To
operate, simply hold CTL and press INS.

## Compiling

Compiles only on Windows. Requires user32.dll, kernel32.dll, gdi32.dll.
Compiled using .NET 4.5.

## Running

First, obtain an anonymous API key from imgur. Then, copy paste your API key 
into the variable IMGUR_ANONYMOUS_KEY: 

`private const string IMGUR_ANONYMOUS_KEY = "-1";`

Application will throw an exception otherwise.

## To use

1) Bring to the foreground the window you want a SS of.
2) Press "CTL + INS"
3) Paste from clipboard!