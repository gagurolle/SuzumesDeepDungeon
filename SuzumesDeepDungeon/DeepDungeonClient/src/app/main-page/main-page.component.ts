import { Component } from '@angular/core';
import { SafeHtml } from '@angular/platform-browser';


@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css'
})
export class MainPageComponent {

iterator: number = 0;

  handleCopyButtonClick(): void {
  this.iterator++;
  
  switch(true) {
    case this.iterator === 1:
      this.showNotification('Зачем тебе копировать? Не надо так делать, просто читай и наслаждайся.');
      break;
    case this.iterator === 2:
      this.showNotification('Да нет тут реализации копирования по кнопке, просто забей.');
      break;
    case this.iterator === 3:
      this.showNotification('( ͡° ͜ʖ ͡°) ну раз ты так хочешь, тыкай дальше');
      break;
    case this.iterator >= 4 && this.iterator <= 10:
      this.showNotification('(づ ◕‿◕ )づ');
      break;
      case this.iterator >10 && this.iterator <= 99:
      this.showNotification(`тыкнул ${this.iterator} раз/а`);
      break;
      case this.iterator ===100:
      this.showNotification(`${this.iterator} РАЗ - БЕЗУМИЕ КАКОЕ-ТО! /╲/\╭( ͡°͡° ͜ʖ ͡°͡°)╮/\╱\ обнуляю счетчик...`);
      this.iterator = 0;
      break;
    default:
      this.iterator = 0;
      break;
  }
}


  private showNotification(message: string): void {

    alert(message);
  }

}
