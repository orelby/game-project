# Game Project

This Web App has been developed as an exercise for myself. You're welcome to extend it or modify it as an exercise yourself.

## Demo

Check out the demo at Heroku: [https://game-project-demo.herokuapp.com/games/tic-tac-toe](https://game-project-demo.herokuapp.com/games/tic-tac-toe).

## Background

The back end is built on C#/.NET Core, while the front end is written in TypeScript and utilizes Angular 6 (using ngx-bootstrap). No database is involved at this time.

The project uses SignalR for real-time, bi-directional communication between the server and the clients.

## Overview: Tic Tac Toe

The project is currently made up of one game - Tic Tac Toe.

It has two modes: Player vs. Player and Player vs. AI.

### PvP

Player vs. Player mode operates on the fly, without a DB connection, using SignalR.

### AI

Player vs. AI mode has a few settings:

- Difficulty (Low, high, impossible)
- First Player (Player, AI, random)

The AI is based on the Minimax algorithm with Alpha-Beta Pruning and some features:

- Saving the AI from committing a suicidal move by encouraging it to **get as far as it can** into the game.
- Adding some **randomness** - the AI chooses a random move among the best moves (all moves with the same score).

## Roadmap

There are many ways in which this project can be developed further, and you are welcome to use it as an exercise in your own repository or outside GitHub.

Here are several ideas, some of which I might do myself.

### Short term

- Refactor
  - Combine AI and PvP behind the scenses while reducing code duplication and allowing extensibility, e.g. AI logs
  - Review everything and possibly clean up spaghetti code or bad practices and improve the code
 - Improvements:
   - Handle cases where the user has waited too long for another player, both in-game and on matchmaking
   - Prevent accidentally navigating away mid-game
   - Add routing with game menu options
   - Adjust global routing:
	   - `/` -> `/games/tic-tac-toe`
	   - `/games` -> `/games/tic-tac-toe`
 
### Long term / general ideas

- Add i18n and RTL support
- Adjust styling in mobile (full page without scroll)
- Verify/improve browser support
- Connect to a DB and save logs
- Show stats for each AI
- Add anonymous URL game invites
- Build a User System
	- Personal stats
	- Friend requests
	- Game invites between friends
	- Personal/global stats
	- Chat (in-game)
- ...

## Licence

This project is licensed under the terms of the GNU General Public License v3.0.
