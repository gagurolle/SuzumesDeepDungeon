// services/signalr.service.ts
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';

export interface Message {
  user: string;
  text: string;
  timestamp: Date;
}

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  public messageReceived = new Subject<Message>();
  public connectionEstablished = new Subject<boolean>();
  private connectionUrl = 'http://localhost:5000/chatHub'; // Замените на ваш URL

  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  private createConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.connectionUrl)
      .build();
  }

  private startConnection() {
    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
        this.connectionEstablished.next(true);
      })
      .catch(err => {
        console.log('Error while starting connection: ' + err);
        setTimeout(() => this.startConnection(), 5000); // Переподключение через 5 секунд
      });
  }

  private registerOnServerEvents() {
    // Слушаем событие ReceiveMessage от сервера
    this.hubConnection.on('ReceiveMessage', (user: string, text: string) => {
      const message: Message = {
        user: user,
        text: text,
        timestamp: new Date()
      };
      this.messageReceived.next(message);
    });

    // Слушаем события подключения/отключения пользователей
    this.hubConnection.on('UserConnected', (connectionId: string) => {
      console.log(`User connected: ${connectionId}`);
    });

    this.hubConnection.on('UserDisconnected', (connectionId: string) => {
      console.log(`User disconnected: ${connectionId}`);
    });
  }

  // Метод для отправки сообщения на сервер
  public sendMessage(user: string, message: string) {
    this.hubConnection.invoke('SendMessage', user, message)
      .catch(err => console.error(err));
  }

  // Закрытие соединения при уничтожении компонента
  public stopConnection() {
    this.hubConnection.stop().catch(err => console.log(err));
  }
}
