import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';
import {  CellContent, ka4GameModel } from './ka4-game-model';
import { trigger, state, transition, style, animate, keyframes } from '@angular/animations';

@Component({
  selector: 'app-ka4',
  templateUrl: './ka4.component.html',
  styleUrls: ['./ka4.component.scss'],
  animations: [
    trigger('dropState', [
        // state('up', style({transform: 'translateY(0)', opacity: .7})),
        state('down', style({transform: 'translateY(0)', opacity: 1})),
        transition('void => down', [
            animate('200ms ease-in', keyframes([
                style({transform: 'translateY(-300%)', offset: 0}),
                style({transform: 'translateY(0)', offset: .6}),
                style({transform: 'translateY(-40px)', offset: .7}),
                style({transform: 'translateY(0)', offset: 1}),
            ]))
        ])])]
})
export class Ka4Component implements OnInit {

  @Input() game: ka4GameModel;
  @Output() onColumnClick = new EventEmitter<number>();

  IMAGE_FOR_PLAYER = ['assets/img/Empty.png', 'assets/img/BallPlayer.png', 'assets/img/BallComputer.png'];
  IMAGE_FOR_WINNER = 'assets/img/BallWinner.png';
  constructor() { }

  ngOnInit() {
  }

  getImageForCell(cell: number): string {
    if (cell === null || cell === undefined) 
        cell=0;       
    return (cell === CellContent.Winning) ?
        this.IMAGE_FOR_WINNER : this.IMAGE_FOR_PLAYER[cell];
}

getDropStateForCell(column: number, cell: number): string {
    return (column === this.game.lastMove.column && cell === this.game.lastMove.cell) ? 'down' : null;
}

clickColumn(column: number) {
    this.onColumnClick.emit(column);
}

}
