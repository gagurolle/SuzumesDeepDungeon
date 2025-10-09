import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Message, SignalRService } from '../services/SignalRService/signal-rservice.service';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [
    FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class ChatComponent implements OnInit, OnDestroy {
  messages: Message[] = [];
  newMessage: string = '';
  currentUser: string = '';
  isConnected: boolean = false;
  
  private messageSubscription!: Subscription;
  private connectionSubscription!: Subscription;

  constructor(private signalRService: SignalRService) {}

  ngOnInit() {
    // Подписываемся на получение новых сообщений
    this.messageSubscription = this.signalRService.messageReceived.subscribe(
      (message: Message) => {
        this.messages.push(message);
      }
    );

    // Подписываемся на изменения статуса соединения
    this.connectionSubscription = this.signalRService.connectionEstablished.subscribe(
      (status: boolean) => {
        this.isConnected = status;
      }
    );

    // Запрашиваем имя пользователя
    this.currentUser = prompt('Введите ваше имя:') || 'Аноним';
  }

  sendMessage() {
    if (this.newMessage.trim() !== '') {
      this.signalRService.sendMessage(this.currentUser, this.newMessage);
      this.newMessage = '';
    }
  }

  ngOnDestroy() {
    // Отписываемся от подписок при уничтожении компонента
    this.messageSubscription.unsubscribe();
    this.connectionSubscription.unsubscribe();
    this.signalRService.stopConnection();
  }
}

