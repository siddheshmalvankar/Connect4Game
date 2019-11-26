import { HttpService } from '../shared/http.service';
import { HttpClient } from '@angular/common/http';
import { AppConstant } from '../shared/app.constants';

export enum CellContent {Empty = 0, Winning = 5}
export type LastMovePosition = {column: number, cell: number};

export class GameBoardUserRequest
{
    selectedColumn:number;
    isPlayer:boolean;
    gameboard:any[][];
    columns:number;
    rows:number;
}
export class GameBoardUserResponse
{
    Player:any;
    gameboard:any[][];
    columns:number;
    rows:number;
    Errors:string[];
    Success:boolean;
    Message:string;   
}
export class GameCell {
    constructor(public content: number) {};
}

export class ka4GameModel 
{   
    currentPlayer: number;
    gameOver: boolean;
    winner: number;  
    lastMove: LastMovePosition;
    gameboard: GameCell[][];
    selectedColumn: number;
    message:string;
    errors:string[];
    constructor() {    
       
    }
    

}