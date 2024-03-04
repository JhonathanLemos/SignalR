// chat.service.ts
import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection, HubConnectionState, IHttpConnectionOptions } from "@microsoft/signalr";
import { AuthService } from "src/app/services-security/auth.service";
import { TokenService } from "src/app/services-security/token.service";
import { UserAndCode } from '../entidades/UserAndCode';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  hubConnection: HubConnection | null = null;

  constructor(private tokenService: TokenService, private authService: AuthService) { }

  async getConncetion() {
    const token = this.tokenService.getToken('token');
    const options: IHttpConnectionOptions = {
      accessTokenFactory: () => token
    };
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7136/chatHub', options) // URL do seu backend
      .build();

    await this.hubConnection.start();
    
  }

  async sendMessages(userId: string | null, userNewMessageId: string | null, message: string|null) {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      await this.hubConnection.send("newMessage", userId, userNewMessageId, message);
    }
  }


  async sendSolicitation(userAndCode: UserAndCode) {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      await this.hubConnection.send("newSolicitation", userAndCode);
    }
  }


  async stopConnection(){
    await this.hubConnection?.stop();
  }
}