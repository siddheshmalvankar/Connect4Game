import { Component, OnInit, OnDestroy } from '@angular/core';
import { ka4Service } from './ka4/ka4.service';
import { ka4GameModel } from './ka4/ka4-game-model';
import { Subscription } from 'rxjs/internal/Subscription';
import { delay } from 'q';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: [ka4Service]
})
export class AppComponent implements OnInit, OnDestroy{
   formSubscriptions:Subscription[]=[];
  gameBoard: ka4GameModel; 
  currentPlayer:boolean;
  formIsLoading:boolean;
  gameBoardMessage:string;
  playerName: string[] = [];

  constructor(private gameService: ka4Service)
  {
    this.gameBoard= new ka4GameModel();
  }
  ngOnInit() {
    this.playerName[1] = 'Player';
    this.playerName[2] = 'Computer';
    this.resetGame();
    this.gameBoardMessage="Welcome to Connect 4 Game!";
    

  }
  ngOnDestroy() {        
    this.formSubscriptions.forEach(subscription=>subscription.unsubscribe())    
  }
  clickColumn(column: number) { 
    if(this.gameBoard.gameOver)
     return;
    if(this.gameBoard.currentPlayer===1)  
    { 
          this.gameBoard.selectedColumn=column+1;  
          this.pushBall(); 
          if(this.gameBoard.winner==1)  
          {        
            this.gameBoard.gameOver=true;
            return;
          }
          if(this.gameBoard.gameOver)   
          return;
           //Computer turn to play
          this.switchPlayer();
          setTimeout(() => {
            this.pushBall();
            this.switchPlayer();
            if(this.gameBoard.winner==2)  
            {        
              this.gameBoard.gameOver=true;
              return;
            }           
          }, 1000); //excute after 1 sec
         
          if(this.gameBoard.gameOver)   
          return;
          //alert(this.gameBoard.currentPlayer);
    }
        
  }
  resetGame() {
   let resetSubscription= this.gameService.resetGameBoard().subscribe(
  
      x=>{
        console.log(x)  ;
        if(x.success)
        {
          console.log('Board Reset Completed')   
          this.gameBoard.gameOver=false;
          this.gameBoard.message=x.message;
          this.gameBoard.winner=-1;      
          this.gameBoard.currentPlayer=1;
          this.gameBoard.gameboard=[...x.gameboard];
          this.gameBoard.lastMove=x.lastmoveposition;   
          console.log('Binding Done') ;
        //  console.log(this.gameBoard);             
        }
        else
        console.log('Board Reset Error Occurred')
       
      }
    );
    this.formSubscriptions.push(resetSubscription);

  }

  makeMoves()
  {
  }
  pushBall()
  {
    //console.log('PushBall');
    this.formIsLoading=true;
    this.gameService.PlayNextMove(this.gameBoard).subscribe(
      board =>{
        if(board.success)
        {         
        this.gameBoard.gameboard=[...board.gameboard];
        this.gameBoard.lastMove=board.lastmoveposition;
        this.gameBoard.winner=board.winner;
        this.gameBoard.gameOver=board.isBoardFull;  
        this.gameBoard.message=board.message;   
      //  this.gameBoardMessage=  board.message;
        //console.log('Binding Done') ;
        //console.log(this.gameBoard);
        this.formIsLoading=false;
        }
        else
        {
        console.log('Move was Completed with Exception');
        this.gameBoard.errors=board.Errors;
        this.formIsLoading=false;
        }
      }
    )
  }
  switchPlayer()
  {
    (this.gameBoard.currentPlayer==1)? 
    this.gameBoard.currentPlayer=2: this.gameBoard.currentPlayer=1;   
    
    if(this.gameBoard.currentPlayer==1)
    this.gameBoardMessage="Player move is awaited..";
    else
    this.gameBoardMessage="Computer is thinking...";
  }

}
