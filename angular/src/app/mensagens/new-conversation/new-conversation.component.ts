import { AfterViewChecked, AfterViewInit, Component, ElementRef, Input, OnInit, SimpleChanges, ViewChild } from "@angular/core";
import { FormControl } from "@angular/forms";
import { HubConnectionBuilder, HubConnectionState, IHttpConnectionOptions } from "@microsoft/signalr";
import { firstValueFrom } from "rxjs";
import { Conversation } from "src/app/entidades/Conversation";
import { Message } from "src/app/entidades/Message";
import { PaginationMessages } from "src/app/entidades/PaginationMessages";
import { UserName } from "src/app/entidades/Username";
import { AuthService } from "src/app/services-security/auth.service";
import { TokenService } from "src/app/services-security/token.service";
import { ChatService } from "src/app/services/chat.service";
import { ConversationService } from "src/app/services/conversation.service";
import { UsersService } from "src/app/services/users.service";
import { capitalizeFirstLetter } from "src/app/util/capitalizeFirstLetter";

@Component({
  selector: 'app-new-conversation',
  templateUrl: './new-conversation.component.html',
  styleUrls: ['./new-conversation.component.css']
})
export class NewConversationComponent implements OnInit, AfterViewChecked {
  listMessages!: Message[];
  userNewMessageId!: string | null;
  userId!: string | null;
  pagina = 1;
  userName!: UserName;
  messageControl = new FormControl('');
  @ViewChild('container') container!: ElementRef;
  private _updateNewConversation: boolean = false;
  @Input()
  set updateNewConversation(value: boolean) {
    this._updateNewConversation = value;
    this.pagina = 1;
    this.chatScrool();
    this.loadMessages();
  }

  constructor(private chatService: ChatService, private tokenService: TokenService, private authService: AuthService, private conversationService: ConversationService, private userService: UsersService) { }

  async ngAfterViewChecked() {
  }

  async ngOnInit() {
    this.userNewMessageId = this.authService.getCookie("userNewMessage");
    this.userId = this.authService.getCookie('userId');
    // await this.loadMessages();
    // await this.chatService.getConncetion();
    await this.startConnection();
    await this.chatScrool()

  }

  async chatScrool() {

    const minhaDiv = document.querySelector('.segura-mensagens')
    if (minhaDiv) {
      setTimeout(() => {
      this.container.nativeElement.scrollTop = this.container.nativeElement.scrollHeight;
        
      }, 500);
    }
  }


  async startConnection() {
    this.userId = this.authService.getCookie('userId');
    this.chatService.hubConnection?.on("newMessage", async () => {
      await this.loadMessages();
      if (this.container.nativeElement.scrollTop == this.container.nativeElement.scrollHeight )
      await this.chatScrool();
    });
  }

  async sendMessages() {
    await this.chatService.sendMessages(this.userId, this.userNewMessageId, this.messageControl.value);
    this.messageControl.setValue("");
    // await this.loadMessages();

  }

  async loadMessages() {
    this.userNewMessageId = this.authService.getCookie("userNewMessage");
    this.userService.getUserNameById(this.userNewMessageId).subscribe(res => {
      this.userName= res;
    })
    this.userId = this.authService.getCookie('userId');
    let conversation = new Conversation();
    conversation.firstUserId = this.userId;
    conversation.secondUserId = this.userNewMessageId;
    try {
      setTimeout(async () => {
        var pagination =  new PaginationMessages();
        pagination.pagina = this.pagina; 
        this.listMessages = await this.conversationService.getAllMessagesFromConversationByUserId(conversation, pagination);
        this.messageControl.setValue('')
      }, 200);
    } catch (error) {
    }
  }

  capitalize(str: string): string {
    return capitalizeFirstLetter(str); // Use a função
  }
}
