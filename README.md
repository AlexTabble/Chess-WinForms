All logic goes through the click eventhandler in the board class

The LegalMoves list first contains all the possible moves a piece can make and is then overridden once the illegal moves are filtered out in the gameflow class

The Move class handles all logic for what needs to happen when a piece is moved.
The GameMoves class records all the moves and checks for draws after a piece is moved.

Sources:

Class for Fen Strings:
https://youtu.be/4EmtLV1JQW4?si=OxTnOAlVohXqDw9T

Special Rules:
https://www.chess.com/learn-how-to-play-chess#special-rules-chess
