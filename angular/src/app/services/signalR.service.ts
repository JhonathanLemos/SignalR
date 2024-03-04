// signalr.service.ts
import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;

  constructor() { }

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7136/chatHub') // URL do seu backend
      .build();

    this.hubConnection.start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  public addReceiveMessageListener = (callback: (user: string, message: string) => void) => {
    this.hubConnection.on("ReceiveMessage", callback);
  }

  sendMessage(recipient: string, message: string) {
}

}
