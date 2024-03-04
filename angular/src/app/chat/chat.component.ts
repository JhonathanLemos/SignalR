import { Component } from '@angular/core';
import { Message } from '../entidades/Message';
import { FormControl } from '@angular/forms';
import * as signalR from "@microsoft/signalr";
import { TokenService } from '../services-security/token.service';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { AuthService } from '../services-security/auth.service';
import { BasicUser } from '../entidades/BasicUser';
@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent {
  messages!: [];
  userName!: "Joao";
  hubConnection: any;

  constructor(private tokenService: TokenService, private authService: AuthService) {
    const token = this.tokenService.getToken('token');
    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => token
    };
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7136/chatHub', options ) // URL do seu backend
    .build();
    this.startConnection();
  }

  messageControl = new FormControl('');
  startConnection() {
    this.hubConnection.on("newMessage", (userName: string, message: string, userId: Number) => {
    });
  }

  sendMessages() {
    var userName = this.authService.getCookie('userName');
    this.hubConnection.send("newMessage", userName, this.messageControl.value)
      .then(() => {
        this.messageControl.setValue("");
        console.log(this.hubConnection.state);
        
      })
      .catch((error: any) => {
        console.error('Erro ao enviar mensagem:', error);
        // Aqui você pode adicionar lógica para lidar com o erro, se necessário
      });
  }

}
