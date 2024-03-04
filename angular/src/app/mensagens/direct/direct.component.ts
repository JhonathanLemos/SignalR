import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Conversation } from 'src/app/entidades/Conversation';
import { UserAndName } from 'src/app/entidades/UserAndName';
import { UserAndNameAndLastMessage } from 'src/app/entidades/UserAndNameAndLastMessage';
import { AuthService } from 'src/app/services-security/auth.service';
import { ChatService } from 'src/app/services/chat.service';
import { ConversationService } from 'src/app/services/conversation.service';
import { capitalizeFirstLetter } from 'src/app/util/capitalizeFirstLetter';
@Component({
  selector: 'app-direct',
  templateUrl: './direct.component.html',
  styleUrls: ['./direct.component.css']
})
export class DirectComponent implements OnInit {
  @Input() userId!: string | null;
  @Input() show!: boolean;
  @Output() sendInfo = new EventEmitter<boolean>();
  @Output() updateConversation = new EventEmitter<boolean>();
  userCode!: string | null;
  listaMensagens!: UserAndNameAndLastMessage[];
  constructor(private chatService: ChatService, private conversationService: ConversationService, private route: Router, private authService: AuthService) { }

  async ngOnInit() {
    this.getAllConversationsByUserId();
    this.userCode = this.authService.getCookie("userCode")
    // await this.chatService.getConncetion();
    await this.startConnection();
  }
  async startConnection() {
    this.chatService.hubConnection?.on("attConversation", () => {
      this.getAllConversationsByUserId()
    });

      this.chatService.hubConnection?.on("newMessage", (userName: string, message: string, userId: Number) => {
        this. getAllConversationsByUserId();
      });
  }

  getAllConversationsByUserId() {
    this.conversationService.getAllConversationsByUserId(this.userId).subscribe(res => {
      this.listaMensagens = res
    }
    )
  }

  changeShow(){
    this.sendInfo.emit(true)
  }

  openNew(user: UserAndNameAndLastMessage) {
    this.authService.setCookie('userNewMessage', user.userId, 1);
    this.updateConversation.emit(this.show)
  }

  capitalize(str: string): string {
    return capitalizeFirstLetter(str); // Use a função
  }

}
