# Radiant Knights - Match 3 Puzzle Game


This project was made during the 3 months of an advanced mobile programming bootcamp. The main focus of the bootcamp was to learn and apply some important design patterns, understand the solid principles and know some tools that are useful on live mobile games.

To accomplish that I created a game that is scalable, can be tested using unit tests and has a good separation of responsabilities across its features.

The game can be downloaded from the Play Store: https://play.google.com/store/apps/details?id=com.EnygmaGames.RadiantKnightsMatch3

## Puzzle Game - Combat System

The game is a simple Match 3 Puzzle game with a turn-based combat system. Performing better matches will create booster that will help you defeat the hordes of monsters.

Both the board and the combat system are built using the MVC pattern to have clear separation of responsabilities. The view just reads the inputs and lets the controller
know that some input has to be executed. The controller doesn't have to know about the view, it just reads the input and performs the changes on the model if necessary and
then notifies through events if something has changed. 

This way of handling the data allows me to create unit tests to test all the controller's functionality without the need of a view and have a clear separation of responsabilities.

<p float="left" align="center">
  <img src="https://i.imgur.com/iRfSuvI.jpg" width="250" height="500"/>
</p>

## Data Driven Approach

I wanted to create a system that is ready to react to remote changes on live builds, making it easier to add new content or make balance changes without the need of publishing a new build.

Using the Unity Gaming Services's tools <b> Remote Config </b> and <b> Cloud Content Delivery </b> I can change configuration values or send new assets to the build.

### Resources

Players will obtain new resources as they progress through levels. The resources types and initial amount of each one of them can be changed from the remote configuration, this way
I can add new resources and send their assets through the <b> Cloud Content Delivery </b> tool.

For example I could add a new resource for a seasonal Halloween Event by just adding it to the configuration and sending the new assets through the CCD.

<p float="left" align="center">
  <img src="https://i.imgur.com/cWBgSrq.png" width="425"/>
  <img src="https://i.imgur.com/lfNWJBj.jpg" width="250" height="500"/>
</p>


### Shop

The game includes a shop where the player can buy new boosters using the soft currency obtained while progressing or cosmetics using the hard currency obtained with IAP.
The player can also watch ADs to obtain rewards.

The shop was built on the same way as the board, using the MVC pattern.

Following the same data driven approach, shop items and prices can be updated through the Remote Config.

<p float="left" align="center">
  <img src="https://i.imgur.com/NDvgZtu.png" width="425"/>
  <img src="https://i.imgur.com/f01YEV2.jpg" width="250" height="500"/>
  <img src="https://i.imgur.com/TB5J3gl.jpg" width="250" height="500"/>
</p>

## Tools

As we all know, editing JSON files can very easily cause errors and since all the remote configurations have to be sent on a JSON format I created an editor tool that uses a
script located in google's App Scripts to transform the data on a sheet from Google Sheets into a JSON. This tool saves a file into my resources folder with all the data from the
sheet on a Json so I can easily copy & paste it on my remote configuration. This tool can be found on <b> Assets/Scripts/Editor </b> 
