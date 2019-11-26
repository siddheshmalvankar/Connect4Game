import {Injectable} from '@angular/core';
import { ka4GameModel, GameBoardUserRequest } from './ka4-game-model';
import { HttpService } from '../shared/http.service';
import { HttpClient } from '@angular/common/http';
import { AppConstant } from '../shared/app.constants';

@Injectable()
export class ka4Service extends HttpService{
    gameboard: ka4GameModel;  
    _endpoint:string;
    constructor(httpClient: HttpClient) {
        super(httpClient); 
        this.gameboard= new ka4GameModel(); 
     }
     postData(gameboard:ka4GameModel):GameBoardUserRequest
     {
         let result = new GameBoardUserRequest()
         {
         result.selectedColumn = gameboard.selectedColumn;
         result.columns = AppConstant.GridColumns;
         result.rows = AppConstant.GridRows;
         result.isPlayer = gameboard.currentPlayer===1;
         result.gameboard =[...gameboard.gameboard] ;
      
         }
         return result;
     }
     resetGameBoard() {
         this._endpoint=AppConstant.WebApiURL.resetGameBoard+"?rows="+AppConstant.GridRows+"&columns="+AppConstant.GridColumns;  
         return this.Request(this._endpoint);
 
     }
 
     PlayNextMove(gameboard:ka4GameModel)
     {
         this._endpoint=AppConstant.WebApiURL.playeMove;  
         return this.Execute(this._endpoint,this.postData(gameboard))
     }
}
