<h1 align="center">MadLad</h1>

<p align="center"><img src="https://user-images.githubusercontent.com/42365887/116344002-265ff080-a7ab-11eb-8254-b496ec349a00.png"></p>

<h1 align="center"> <img src="https://forthebadge.com/images/badges/made-with-c-sharp.svg"> <img src="https://forthebadge.com/images/badges/you-didnt-ask-for-this.svg"></h1>

MadLad is an esolang (a joke programming language) meant to be similar to C# but with a twist. MadLad is a very angry language and will yell at you for everything. Featuring screaming syntax and unhelpful   error messages, this language hates everything and will express it with rage.

MadLad is a work in progress. Feel free to star the project if you want to follow the development.

<h2 align="center"> Contributing </h2>

If you wish to contribute please do the following:
* Fork the repo,
* Make the changes and commit them to the forked repo,
* Make a pull request,
* I will review them and consider the changes.

<h2 align="center"> Issues </h2>

If you have issues, please do the following:
* Create an issue with a short, yet descriptive title
* In the issue, please explain what the issue is in depth and how to recreate it


<h2 align="center"> Using & Installing </h2>
To use MadLad, make sure you have .NET 5 installed and do the following:

*Not Supported Yet. Coming Soon*:tm:

* ~~Clone the repo~~
* ~~Run `run.cmd`. If nothing goes wrong you should be greeted by the REPL prompt. This is where you can play around with current features of MadLad. Use should be familiar if you use C# or another similar language.~~

<h3 align="center"> MadLad Syntax </h3>
MadLad's syntax is meant to be angry. It is mad after all. Common C# syntax has been replaced by its angry counterpart. This is what is supported so far.
<br>

|   C# Syntax   |    var   |        if        |    while   |     for     | true | false |       else      |
|:-------------:|:--------:|:----------------:|:----------:|:-----------:|:----:|:-----:|:---------------:|
| MadLad Syntax | WHATEVER | BUTWHATFUCKINGIF | DOTHETHING | GOAROUNDPLS | FINE | NO    | WHATTHEFUCKELSE |


<h3 align="center"> REPL Arguments </h3>

`#help`: to show the supported commands.

`#DEBUG`: to go into debug mode. Debug commands will become useable in this mode. To exit debug mode, just type this command again.

`#clear`: clears the screen.

`#exit`: exit the REPL.

*The following commands work in DEBUG mode only*

`#showtree`: shows the syntax tree representing what is going on under the hood.

`#showlexer --basic`: shows the basic lexer. There will be no evaluation output. Just the lexed tokens with minimal info. Only shows tokens you can see.

`#showlexer --full`: shows the full lexer. Unlike the basic lexer, this lexer shows all tokens. Whitespace, EOF tokens, all of them - plus the details of what makes up the token.

`#reset`: resets the scope so that all variables are removed from the stack. You can start with fresh global variables.

<br><br>

<h3 align="center">This repository is a part of the <b><a href="https://github.com/salty-sweet/TLoDLiBSsf">The List of Developing Languages in Brackeys Server so far.</a> Check it out for some other awesome languages</h4>
